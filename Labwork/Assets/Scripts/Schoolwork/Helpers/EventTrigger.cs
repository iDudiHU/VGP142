using UnityEngine;
using UnityEngine.Events;

namespace Schoolwork.Helpers
{
    public class EventTrigger : MonoBehaviour
    {
        public UnityEvent onPlayerEnter;
        void OnTriggerEnter(Collider other)
        {
                onPlayerEnter.Invoke();
                Time.timeScale = 0;
        }
    }
}
