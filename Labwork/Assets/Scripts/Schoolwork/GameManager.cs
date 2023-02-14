using System.Collections;
using System.Collections.Generic;
using Schoolwork.Helpers;
using UnityEngine;

namespace Schoolwork
{
	public class GameManager : Singleton<GameManager>
	{
    
		// Start is called before the first frame update
		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}	
}
