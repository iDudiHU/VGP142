using UnityEngine;
using Schoolwork.Helpers;

namespace Schoolwork
{
    public class KickStarter : PersistentSingleton<KickStarter>
    {
        public GameObject globalPlayer;
        // Start is called before the first frame update
        void Start()
        {
            globalPlayer = GameObject.FindWithTag("Player");
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

