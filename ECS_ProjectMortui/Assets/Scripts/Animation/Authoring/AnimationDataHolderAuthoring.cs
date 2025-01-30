using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationDataHolderAuthoring : MonoBehaviour
{
    public AnimationDataSO soldierIdle;
    public class Baker : Baker<AnimationDataHolderAuthoring>
    {
        

        public override void Bake(AnimationDataHolderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AnimationDataHolder animationDataHolder = new AnimationDataHolder();

            EntitiesGraphicsSystem entitiesGraphicsSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();


            BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

            ref BlobArray<AnimationData> animationDataBlobArray = ref blobBuilder.ConstructRoot<BlobArray<AnimationData>>();


            {
                BlobBuilderArray<AnimationData> animationDataBlobBuilderArray = blobBuilder.Allocate(ref animationDataBlobArray, 2);

                animationDataBlobBuilderArray[0].frameMax = authoring.soldierIdle.meshArray.Length;
                animationDataBlobBuilderArray[0].frameTimerMax = authoring.soldierIdle.frameTimerMax;


                BlobBuilderArray<BatchMeshID> batchMeshIdArray = blobBuilder.Allocate(ref animationDataBlobBuilderArray[0].batchMeshIdArray, authoring.soldierIdle.meshArray.Length);


                for (int i = 0; i < batchMeshIdArray.Length; i++)
                {
                    Mesh mesh = authoring.soldierIdle.meshArray[i];

                    batchMeshIdArray[i] = entitiesGraphicsSystem.RegisterMesh(mesh);
                }


                animationDataHolder.animationDataBlobArrayReference = blobBuilder.CreateBlobAssetReference<BlobArray<AnimationData>>(Allocator.Persistent);


                AddBlobAsset(ref animationDataHolder.animationDataBlobArrayReference, out Unity.Entities.Hash128 hash128);
            }


            //ref AnimationData animationData = ref blobBuilder.ConstructRoot<AnimationData>();

            //animationData.frameMax = 0;
            //animationData.frameTimerMax = 0;


            AddComponent(entity, animationDataHolder);
        }
    }
}

public struct AnimationDataHolder : IComponentData
{
    public BlobAssetReference<AnimationData> testSoldiderIdle;
    public BlobAssetReference<BlobArray<AnimationData>> animationDataBlobArrayReference;
}

public struct AnimationData
{
    public int frameMax;
    public float frameTimerMax;
    public BlobArray<BatchMeshID> batchMeshIdArray;
}
