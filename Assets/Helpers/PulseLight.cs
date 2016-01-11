using UnityEngine;
using System.Collections;

namespace Ascent
{
    [RequireComponent(typeof(Light))]
    public class PulseLight : MonoBehaviour
    {
        public float minIntensity = 0f;
        public float maxIntensity = 1f;

        private Light myLight;

        private void Start()
        {
            myLight = GetComponent<Light>();
        }

        private void Update()
        {
            myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Random.value);
        }
    }
}