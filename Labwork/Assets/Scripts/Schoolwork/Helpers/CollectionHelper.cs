using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork.Helpers
{
    public class CollectionHelper : MonoBehaviour
    {
        public ThirdPersonCharacter TPC;
        private void OnTriggerEnter(Collider other)
        {
            TPC.ItemInRangeCollision(other);
        }
    }
}
