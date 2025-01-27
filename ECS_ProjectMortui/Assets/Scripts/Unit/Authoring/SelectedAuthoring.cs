using Unity.Entities;
using UnityEngine;

public class SelectedAuthoring : MonoBehaviour
{
    public bool selected;
    public class Baker: Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Selected
            {
                selected = authoring.selected,
            });

            SetComponentEnabled<Selected>(entity, false);
        }
    }
}

public struct Selected : IComponentData,IEnableableComponent
{
    public bool selected;
    public bool OnSelected;
    public bool OnDeselected;
}
