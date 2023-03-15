using Schoolwork.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Schoolwork.Systems 
{
    public class SceneLoadSystem : MonoBehaviour
    {
		public void LoadLevel1()
		{
			LoadScene("Level1");
		}
		public void LoadScene(string levelName)
		{
			SceneManager.LoadScene(levelName);
		}
	}
}

