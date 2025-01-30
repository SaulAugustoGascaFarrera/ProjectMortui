using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShootAttackAuthoring : MonoBehaviour
{
    public float timerMax;
    public Transform spawnPointLocation;
    public float shootAttackDistance;
    public class Baker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ShootAttack
            {
                timerMax = authoring.timerMax,
                spawnPointLocation = authoring.spawnPointLocation.localPosition,
                shootAttackDistance = authoring.shootAttackDistance
            });
        }
    }
}

public struct ShootAttack : IComponentData
{
    public float timer;
    public float timerMax;
    public int damageAmount;
    public float3 spawnPointLocation;
    public float shootAttackDistance;
    
}
