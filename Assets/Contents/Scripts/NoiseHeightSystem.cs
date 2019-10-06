using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Shiena.EcsNoise
{
	public class NoiseHeightSystem : JobComponentSystem
	{
		[BurstCompile]
		struct TranslationNoise : IJobForEach<Translation>
		{
			public float time;

			public void Execute(ref Translation translation)
			{
				translation.Value.y = 3 * noise.snoise(new float2(time + 0.02f * translation.Value.x,
					                      time + 0.02f * translation.Value.z));
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			GetEntityQuery(new EntityQueryDesc
			{
				All = new[] {ComponentType.ReadWrite<Translation>(),}
			});
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var job = new TranslationNoise {time = Time.realtimeSinceStartup};
			return job.Schedule(this, inputDeps);
		}
	}
}
