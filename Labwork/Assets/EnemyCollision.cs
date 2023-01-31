using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public Enemy enemy;

    private void OnParticleCollision(GameObject other)
    {
        enemy.Die();
    }
}
