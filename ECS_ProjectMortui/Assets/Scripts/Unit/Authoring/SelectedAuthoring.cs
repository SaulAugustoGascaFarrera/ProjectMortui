using Unity.Entities;
using UnityEngine;

public class SelectedAuthoring : MonoBehaviour
{
    public float showScale;
    public GameObject visualEntityGameObject;
    public class Baker: Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Selected
            {
                showScale = authoring.showScale,
                visualEntity = GetEntity(authoring.visualEntityGameObject,TransformUsageFlags.Dynamic)
            });

            SetComponentEnabled<Selected>(entity, false);
        }
    }
}

public struct Selected : IComponentData,IEnableableComponent
{
    public Entity visualEntity;
    public bool OnSelected;
    public bool OnDeselected;
    public float showScale;
}
