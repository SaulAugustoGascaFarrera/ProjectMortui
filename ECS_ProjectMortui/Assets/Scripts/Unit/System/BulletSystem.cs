using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct BulletSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((RefRO<Target> target,RefRW<LocalTransform> localTrasform,RefRW<Bullet> bullet,Entity entity) in SystemAPI.Query<RefRO<Target>, RefRW<LocalTransform>, RefRW<Bullet>>().WithEntityAccess())
        {
            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                entityCommandBuffer.DestroyEntity(entity);
                continue;
            }

            RefRO<LocalTransform> targetLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(target.ValueRO.targetEntity);

            ShootVictim targetPosition = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetHitLocalPosition = targetLocalTransform.ValueRO.TransformPoint(targetPosition.hitLocalPosition);

            float distanceBeforeSq = math.distancesq(localTrasform.ValueRO.Position, targetLocalTransform.ValueRO.Position);


            float3 moveDirection = targetHitLocalPosition - localTrasform.ValueRO.Position;

            moveDirection = math.normalize(moveDirection);

            localTrasform.ValueRW.Position += moveDirection * bullet.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;


            float distanceAfterSq = math.distancesq(localTrasform.ValueRO.Position, targetLocalTransform.ValueRO.Position);


            if(distanceAfterSq > distanceBeforeSq)
            {
                localTrasform.ValueRW.Position = targetLocalTransform.ValueRO.Position;
            }

            float distanceToDestroy = 0.2f;

            if(math.distancesq(localTrasform.ValueRO.Position,targetLocalTransform.ValueRO.Position) < distanceToDestroy)
            {
                entityCommandBuffer.DestroyEntity(entity);

                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.onHealthChange = true;
                targetHealth.ValueRW.healthAmount -= bullet.ValueRO.damageAmount;


            }


        }
    }

   
}
