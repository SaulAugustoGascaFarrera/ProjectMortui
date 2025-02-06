using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance;

    public event EventHandler OnSelectionAreaStart;
    public event EventHandler OnSelectionAreaEnd;

    Vector2 selectionStartMousePosition;

    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        SelectionManager();
    }

    public void SelectionManager()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectionStartMousePosition = Input.mousePosition;

            OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
        }

        if(Input.GetMouseButtonUp(0))
        {
            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);

            Rect selectionAreaRect = GetSelectionAreaRect();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Selected> selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);

            for(int i=0;i<entityArray.Length;i++)
            {
                Selected selected = selectedArray[i];

                entityManager.SetComponentEnabled<Selected>(entityArray[i], false);

                selected.OnSelected = false;
                selected.OnDeselected = true;

                entityManager.SetComponentData(entityArray[i], selected);
            }


            entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform,UnitMover>().WithDisabled<Selected>().Build(entityManager);


            entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<LocalTransform> localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            

            for(int i = 0;i< localTransformArray.Length;i++)
            {
                LocalTransform localTransform = localTransformArray[i];

                Selected selected = entityManager.GetComponentData<Selected>(entityArray[i]);
              

                Vector2 unitPosition = Camera.main.WorldToScreenPoint(localTransform.Position);

                if(selectionAreaRect.Contains(unitPosition))
                {
                    entityManager.SetComponentEnabled<Selected>(entityArray[i], true);

                    selected.OnSelected = true;
                    selected.OnDeselected = false;

                   entityManager.SetComponentData(entityArray[i], selected);

                }
            }

           

        }


        if(Input.GetMouseButtonDown(1))
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().WithPresent<MoveOverride>().Build(entityManager);


            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<MoveOverride> moveOverrideArray = entityQuery.ToComponentDataArray<MoveOverride>(Allocator.Temp);


            for(int i=0;i<entityArray.Length;i++)
            {
                MoveOverride moveOverride = moveOverrideArray[i];

                moveOverride.targetPosition = MouseManager.Instance.GetMousePosition();

                entityManager.SetComponentData(entityArray[i], moveOverride);  

                entityManager.SetComponentEnabled<MoveOverride>(entityArray[i], true);
            }


        }

    }

    public Rect GetSelectionAreaRect()
    {
        Vector2 selectionEndMousePosition = Input.mousePosition;

        Vector2 lowerLeftCorner = new Vector2(Mathf.Min(selectionStartMousePosition.x,selectionEndMousePosition.x),Mathf.Min(selectionStartMousePosition.y,selectionEndMousePosition.y));

        Vector2 upperRightCorner = new Vector2(Mathf.Max(selectionStartMousePosition.x,selectionEndMousePosition.x),Mathf.Max(selectionStartMousePosition.y,selectionEndMousePosition.y));

        Rect rect = new Rect(lowerLeftCorner.x, lowerLeftCorner.y, upperRightCorner.x - lowerLeftCorner.x, upperRightCorner.y - lowerLeftCorner.y);

        return rect;
    }

}
