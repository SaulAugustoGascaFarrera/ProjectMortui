using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public int healthAmount;
    public int healthMax;
    public bool onHealthChange;
    public class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Health
            {
                healthAmount = authoring.healthAmount,
                healthMax = authoring.healthMax,    
                onHealthChange = false
            });
                 
        }
    }
}


public struct Health : IComponentData
{
    public int healthAmount;
    public int healthMax;
    public bool onHealthChange;
}

