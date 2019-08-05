using UnityEngine;

namespace FredericRP.DroneEngine
{
    public class ChangeParticleSystemRatioFromPower : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem[] reactorPSList;
        [SerializeField]
        AnimationCurve reactorEmissionCurve;
        [SerializeField]
        Rigidbody body;
        [SerializeField]
        DroneConfiguration droneConfiguration;

        private void Update()
        {
            for (int i = 0; i < reactorPSList.Length; i++)
            {
                var emission = reactorPSList[i].emission;
                emission.rateOverTime = reactorEmissionCurve.Evaluate(body.velocity.sqrMagnitude / droneConfiguration.maxPower);
            }
        }
    }
}