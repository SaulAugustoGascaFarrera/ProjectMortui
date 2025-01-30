using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MoveOverrideSytem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((RefRW<LocalTransform> localTransform,RefRW<UnitMover> unitMover,RefRW<MoveOverride> moveOverride,EnabledRefRW<MoveOverride> enabledMoveOverride) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<UnitMover>, RefRW<MoveOverride>,EnabledRefRW<MoveOverride>>())
        {
            if(math.distancesq(localTransform.ValueRO.Position,moveOverride.ValueRO.targetPosition) > 2.0f)
            {
                unitMover.ValueRW.targetPosition = moveOverride.ValueRO.targetPosition;
            }
            else
            {
                enabledMoveOverride.ValueRW = false;
            }
        }
    }

   
}
