using System.Collections.Generic;
using UnityEngine;
using static Schoolwork.Enemy;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public struct TransformData
    {
        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
    }
    [System.Serializable]
    public struct HealthData
    {
        public float _currentHealth;
        public float _maxHealth;
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
    public class PlayerData
    {
        public TransformData transformData;
        public HealthData healthData;
        public LevelData levelData;

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
    public PlayerData player = new PlayerData();
    public List<EnemyData> enemyDataList = new List<EnemyData>();
}


