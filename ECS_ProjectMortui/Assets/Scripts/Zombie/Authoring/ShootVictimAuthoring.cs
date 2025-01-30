using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShootVictimAuthoring : MonoBehaviour
{

    public Transform hitLocalPosition;
    public class Baker : Baker<ShootVictimAuthoring>
    {
        public override void Bake(ShootVictimAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ShootVictim
            {
                hitLocalPosition = authoring.hitLocalPosition.localPosition
            });
        }
    }
}


public struct ShootVictim : IComponentData
{
    public float3 hitLocalPosition;
}