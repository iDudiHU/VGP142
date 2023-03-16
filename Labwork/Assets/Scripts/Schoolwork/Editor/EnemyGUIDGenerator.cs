using UnityEngine;
using UnityEditor;
using System;

namespace Schoolwork.Editor
{
    public class EnemyGUIDGenerator : EditorWindow
    {
        [MenuItem("CustomTools/EnemyGUIDGenerator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(EnemyGUIDGenerator));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Generate Enemy GUIDs"))
            {
                GenerateEnemyGUIDs();
            }
        }

        private void GenerateEnemyGUIDs()
        {
            var enemies = GameObject.FindObjectsOfType<Enemy>();

            foreach (var enemy in enemies)
            {
                if (String.IsNullOrEmpty(enemy.Id))
                {
                    enemy.GenerateId();
                    EditorUtility.SetDirty(enemy);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

