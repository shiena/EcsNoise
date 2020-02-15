using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shiena.EcsNoise
{
    [HelpURL("https://qiita.com/simplestar/items/17b0886be0170f79aa2e")]
    public class OneCube : MonoBehaviour
    {
        private void Start()
        {
            CreateCube();
        }

        private void CreateCube()
        {
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            // Entityが持つComponentsを設計
            var archetype = manager.CreateArchetype(
                ComponentType.ReadWrite<LocalToWorld>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadOnly<RenderMesh>()
            );
            // 上記のComponentsを持つEntityを作成
            var entity = manager.CreateEntity(archetype);
            // EntityのComponentの値をセット(位置)
            manager.SetComponentData(entity, new Translation() {Value = new float3(0, 1, 0)});

            // キューブオブジェクトの作成
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // EntityのComponentの値をセット(描画メッシュ)
            manager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = cube.GetComponent<MeshFilter>().sharedMesh,
                material = cube.GetComponent<MeshRenderer>().sharedMaterial,
                subMesh = 0,
                castShadows = ShadowCastingMode.Off,
                receiveShadows = false
            });

            // キューブオブジェクトの削除
            Destroy(cube);
        }
    }
}