using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;


[UpdateInGroup(typeof(SimulationSystemGroup),OrderFirst = true)]
partial struct ResetTargetSystem : ISystem
{
    private ComponentLookup<LocalTransform> localTransformComponentLookup;
    private EntityStorageInfoLookup entityStorageInfoLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        localTransformComponentLookup = state.GetComponentLookup<LocalTransform>(true);
        entityStorageInfoLookup = state.GetEntityStorageInfoLookup();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        localTransformComponentLookup.Update(ref state);

        entityStorageInfoLookup.Update(ref state);

        ResetTargetJob resetTargetJob = new ResetTargetJob
        {
            localTransformComponentLookup = localTransformComponentLookup,
            entityStorageInfoLookup = entityStorageInfoLookup
        };

        resetTargetJob.ScheduleParallel();
    }

  
}


[BurstCompile]
public partial struct ResetTargetJob : IJobEntity
{

    [ReadOnly] public ComponentLookup<LocalTransform> localTransformComponentLookup;
    [ReadOnly] public EntityStorageInfoLookup entityStorageInfoLookup;

    public void Execute(ref Target target)
    {

        if (target.targetEntity == Entity.Null) return;

       
        if (!entityStorageInfoLookup.Exists(target.targetEntity) || !localTransformComponentLookup.HasComponent(target.targetEntity))
        {
            target.targetEntity = Entity.Null;

           
        }
       
    }
}