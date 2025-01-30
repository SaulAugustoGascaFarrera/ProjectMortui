using Unity.Entities;
using UnityEngine;

public class EntitiesReferenceAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;

   public class Baker : Baker<EntitiesReferenceAuthoring>
    {
        public override void Bake(EntitiesReferenceAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EntitiesReference
            {
                bulletEntity = GetEntity(authoring.bulletPrefab,TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct EntitiesReference : IComponentData
{
    public Entity bulletEntity;
}
