using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SelectedVisualSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(RefRW<Selected> selected in SystemAPI.Query<RefRW<Selected>>().WithPresent<Selected>())
        {
            if(selected.ValueRO.OnDeselected)
            {
                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                localTransform.ValueRW.Scale = 0.0f;
            }

            if(selected.ValueRW.OnSelected)
            {
                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                localTransform.ValueRW.Scale = selected.ValueRO.showScale;
            }
        }
    }

}
