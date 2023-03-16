﻿using Schoolwork.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is meant to be used on buttons as a quick easy way to load levels (scenes)
/// </summary>
namespace Schoolwork.Helpers
{
    public class LevelLoadButton : MonoBehaviour
    {
        /// <summary>
        /// Description:
        /// Loads a level according to the name provided
        /// Input:
        /// string levelToLoadName
        /// Return:
        /// void (no return)
        /// </summary>
        /// <param name="levelToLoadName">The name of the level to load</param>
        public void LoadLevelByName(string levelToLoadName)
        {
            Time.timeScale = 1;
            SceneLoadSystem.LoadScene(levelToLoadName);
        }
    }
}

