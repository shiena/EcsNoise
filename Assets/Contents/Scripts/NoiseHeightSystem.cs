using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Shiena.EcsNoise
{
	public class NoiseHeightSystem : ComponentSystem
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			GetEntityQuery(new EntityQueryDesc
			{
				All = new[] {ComponentType.ReadWrite<Translation>(),}
			});
		}

		protected override void OnUpdate()
		{
			var time = Time.realtimeSinceStartup;
			Entities.ForEach((ref Translation translation) =>
				{
					translation.Value.y = 3 * noise.snoise(new float2(time + 0.02f * translation.Value.x,
						                      time + 0.02f * translation.Value.z));
				});
		}
	}
}
