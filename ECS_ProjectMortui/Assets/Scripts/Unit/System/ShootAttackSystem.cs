using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem.Haptics;

partial struct ShootAttackSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReference>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReference entitiesReference = SystemAPI.GetSingleton<EntitiesReference>();  

        foreach((RefRO<Target> target,RefRW<ShootAttack> shootAttack,RefRW<LocalTransform> localTransform,RefRW<UnitMover> unitMover) in SystemAPI.Query<RefRO<Target>, RefRW<ShootAttack>, RefRW<LocalTransform>, RefRW<UnitMover>>().WithDisabled<MoveOverride>())
        {
            if(!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);


            if(math.distance(localTransform.ValueRO.Position, targetLocalTransform.Position) > shootAttack.ValueRO.shootAttackDistance)
            {


                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;
                continue;

            }
            else
            {
                //close enough
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;

            }


            float3 aimDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;


            aimDirection = math.normalize(aimDirection);

            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(aimDirection, math.up()), unitMover.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);

            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            if(shootAttack.ValueRW.timer > 0.0f )
            {
                continue;
            }

            shootAttack.ValueRW.timer = shootAttack.ValueRO.timerMax;


            Entity bulletPrefab = state.EntityManager.Instantiate(entitiesReference.bulletEntity);
            float3 bulletSpawnPoint = localTransform.ValueRO.TransformPoint(shootAttack.ValueRO.spawnPointLocation);
            SystemAPI.SetComponent(bulletPrefab, LocalTransform.FromPosition(bulletSpawnPoint));


            RefRW<Bullet> bulletEntity = SystemAPI.GetComponentRW<Bullet>(bulletPrefab);
            shootAttack.ValueRW.damageAmount = bulletEntity.ValueRO.damageAmount;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletPrefab);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;


        }


    }

    
}

//[BurstCompile]
//public partial struct ShootAttackJob : IJobEntity
//{
//    public void Execute(in Target target,ref ShootAttack shootAttack,ref UnitMover unitMover,ref LocalTransform localTransform)
//    {
//        if (!SystemAPI.Exists(target.targetEntity))
//        {

//        }
//    }
//}
