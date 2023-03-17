using System.Collections.Generic;
using UnityEngine;
using static Schoolwork.Enemy;

[System.Serializable]
public class GameData
{
    public string sceneToLoad;
    [System.Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
    [System.Serializable]
    public struct HealthData
    {
        public float _currentHealth;
        public float _maxHealth;
        public int _currentLives;
        public TransformData respawnPoint;
    }
    [System.Serializable]
    public struct LevelData
    {
        public int _level;
        public float _currentExperience;
        public float _experienceNeededForNextLevel;
        public int _attributePoints;
    }
    [System.Serializable]
    public struct LeveledStats
	{
        public float walkSpeed;
        public float printSpeed;
        public float attackSpeed;
        public float jumpHeight;
	}
    [System.Serializable]
    public class PlayerData
    {
        public TransformData transformData;
        public HealthData healthData;
        public LevelData levelData;
        public LeveledStats leveledStats;
        public string currentWeapon;
        public HashSet<int> keyRing;

    }
    [System.Serializable]
    public class EnemyData
    {
        public string Id;
        public EnemyTypes enemyType;
        public TransformData transformData;
        public HealthData healthData;
        public EnemyState currentState;
    }
    [System.Serializable]
    public class CollectibleData
	{
        public string Id;
        public TransformData transformData;
    }
    public PlayerData player = new PlayerData();
    public List<EnemyData> enemyDataList = new List<EnemyData>();
    public List<CollectibleData> collectibleDataList = new List<CollectibleData>();
}


