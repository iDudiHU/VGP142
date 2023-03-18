using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Schoolwork
{
    public class WaypointPath : MonoBehaviour
    {
        [SerializeField] List<Transform> pathPoints;
        public List<Vector3> PathPoints => pathPoints.Select(t => t.position).ToList();
    }
}
