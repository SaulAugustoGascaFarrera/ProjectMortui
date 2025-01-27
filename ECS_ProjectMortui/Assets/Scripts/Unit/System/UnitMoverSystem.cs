using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        UnitMoverJob unitMoverJob = new UnitMoverJob{
            deltaTime = SystemAPI.Time.DeltaTime
        };


        unitMoverJob.ScheduleParallel();

    }

    
}

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{

    public float deltaTime;
    public void Execute(in UnitMover unitMover,ref LocalTransform localTransform,ref PhysicsVelocity physicsVelocity)
    {

        float3 moveDirection = new float3(0, 0, 0) - localTransform.Position;

        float targetDistanceToStop = 2.0f;

       
        if (math.lengthsq(moveDirection) > targetDistanceToStop)
        {
                

            moveDirection = math.normalize(moveDirection);

            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(moveDirection, math.up()), unitMover.rotationSpeed * deltaTime);

            physicsVelocity.Linear = moveDirection * unitMover.movementSpeed;

            physicsVelocity.Angular = float3.zero;
        }
        else
        {
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;

        }
        

        

        
            
    }
}
