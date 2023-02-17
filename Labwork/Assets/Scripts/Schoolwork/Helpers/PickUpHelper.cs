using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork.Helpers
{
    public class PickUpHelper : MonoBehaviour
    {
        public ThirdPersonCharacter TPC;

        private void OnTriggerEnter(Collider other)
        {
            TPC.PickUpItem(other);
        }
    }
}
