using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shiena.EcsNoise
{
	public class Cubes : MonoBehaviour
	{
		private void Start()
		{
			CreateCubes();
		}

		private void CreateCubes()
		{
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

			// Entityが持つComponentsを設計(Prefabとして)
			var archetype = manager.CreateArchetype(
				ComponentType.ReadOnly<Prefab>(),
				ComponentType.ReadWrite<LocalToWorld>(),
				ComponentType.ReadWrite<Translation>(),
				ComponentType.ReadOnly<RenderMesh>()
			);

			// 上記のComponentsを持つEntityを作成
			var prefab = manager.CreateEntity(archetype);

			// EntityのComponentの値をセット(位置)
			manager.SetComponentData(prefab, new Translation {Value = new float3(0, 1, 0)});

			// キューブオブジェクトの作成
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

			// EntityのComponentの値をセット(描画メッシュ)
			manager.SetSharedComponentData(prefab, new RenderMesh
			{
				mesh = cube.GetComponent<MeshFilter>().sharedMesh,
				material = cube.GetComponent<MeshRenderer>().sharedMaterial,
				subMesh = 0,
				castShadows = ShadowCastingMode.Off,
				receiveShadows = false
			});

			// キューブオブジェクトの削除
			Destroy(cube);

			const int SIDE = 100;
			using (var entities =
				new NativeArray<Entity>(SIDE * SIDE, Allocator.Temp, NativeArrayOptions.UninitializedMemory))
			{
				// Prefab Entityをベースに10000個のEntityを作成
				manager.Instantiate(prefab, entities);
				// 平面に敷き詰めるようにTranslationを初期化
				for (var x = 0; x < SIDE; x++)
				{
					for (var z = 0; z < SIDE; z++)
					{
						var index = x + z * SIDE;
						manager.SetComponentData(entities[index], new Translation
						{
							Value = new float3(x, 0, z)
						});
					}
				}
			}
		}
	}
}