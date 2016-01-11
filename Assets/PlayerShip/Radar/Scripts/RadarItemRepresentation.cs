using Ascent.Managers.Pools;
using System;
using UnityEngine;

namespace Ascent.PlayerShip.Radar
{
    [IncludeInPoolManager]
    public class RadarItemRepresentation : MonoBehaviour
    {
        public void SetColor(Color color)
        {
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", color);
        }
    }
}
