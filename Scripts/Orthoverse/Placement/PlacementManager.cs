using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.Placement
{
    public class PlacementManager
    {
        public static Vector3 GetNewPosition(Vector3 origin){
            return origin + UnityEngine.Random.onUnitSphere * 0.3f;
        }
    }
}
