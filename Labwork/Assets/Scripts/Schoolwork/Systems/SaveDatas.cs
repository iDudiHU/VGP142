using System.Collections.Generic;
using UnityEngine;
using static Schoolwork.Enemy;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemyDataList;

    public GameData(PlayerData playerData, List<EnemyData> enemyDataList)
    {
        this.playerData = playerData;
        this.enemyDataList = enemyDataList;
    }
}
[System.Serializable]
public class PlayerData
{
    public TransformData transformData;
    public HealthData healthData;
    public LevelData levelData;

    public PlayerData(TransformData transformData, HealthData healthData , LevelData levelData)
    {
        this.healthData = healthData;
        this.levelData = levelData;
    }
}
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;

    public TransformData(Vector3 position, Quaternion rotation)
	{
        this.position = position;
        this.rotation = rotation;
    }
}
public class HealthData
{
    public float _currentHealth;
    public float _maxHealth;

    public HealthData(float currentHealth, float maxHealth)
	{
        this._currentHealth = currentHealth;
        this._maxHealth = maxHealth;
	}
}
public class LevelData
{
    public int _level;
    public float _currentExperience;
    public float _experienceNeededForNextLevel;

    public LevelData(int level, float currentExperience, float experienceNeededForNextLevel)
	{
        this._level = level;
        this._currentExperience = currentExperience;
        this._experienceNeededForNextLevel = experienceNeededForNextLevel;
    }
}
[System.Serializable]
public class EnemyData
{
    public string Id;
    public EnemyTypes enemyType;
    public TransformData transformData;
    public HealthData healthData;
    public EnemyState currentState;

	public EnemyData(string id, EnemyTypes enemyType, TransformData transformData, HealthData healthData, EnemyState currentState)
	{
		Id = id;
		this.enemyType = enemyType;
		this.transformData = transformData;
		this.healthData = healthData;
		this.currentState = currentState;
	}
}

