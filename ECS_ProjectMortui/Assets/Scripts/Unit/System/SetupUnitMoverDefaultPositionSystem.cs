using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SetupUnitMoverDefaultPositionSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((RefRO<SetupUnitMoverDefaultPosition> setupUnitMoverDefaultPosition,RefRW<UnitMover> unitMover,RefRW<LocalTransform> localTransform,Entity entity) in SystemAPI.Query<RefRO<SetupUnitMoverDefaultPosition>, RefRW<UnitMover>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;

            entityCommandBuffer.RemoveComponent<SetupUnitMoverDefaultPosition>(entity);
        }
    }

    
}
