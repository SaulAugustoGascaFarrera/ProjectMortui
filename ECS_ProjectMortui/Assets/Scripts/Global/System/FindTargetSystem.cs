using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct FindTargetSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);

        foreach((RefRW<FindTarget> findTarget,RefRW<Target> target,RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRW<FindTarget>,RefRW<Target>, RefRW<LocalTransform>>())
        {
            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            if(findTarget.ValueRO.timer > 0.0f)
            {
                continue;
            }

            findTarget.ValueRW.timer = findTarget.ValueRO.timerMax;    

            distanceHitList.Clear();

            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << 7,
                GroupIndex = 0
            };

            if(physicsWorldSingleton.OverlapSphere(localTransform.ValueRO.Position,findTarget.ValueRO.range,ref distanceHitList,collisionFilter))
            {
               

                foreach(DistanceHit distanceHit in distanceHitList)
                {
                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);

                    if (targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        target.ValueRW.targetEntity = distanceHit.Entity;
                    }
                }
            }

        }
    }
    
}
