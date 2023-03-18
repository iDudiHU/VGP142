using UnityEngine;
using UnityEditor;
using System;
using Schoolwork.Helpers;
using Schoolwork.Enemies;

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
                GeneratePickupGUIDs();
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
        private void GeneratePickupGUIDs()
        {
            var pickups = GameObject.FindObjectsOfType<PickUp>();

            foreach (var pickup in pickups)
            {
                if (String.IsNullOrEmpty(pickup.Id))
                {
                    pickup.GenerateId();
                    EditorUtility.SetDirty(pickup);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

