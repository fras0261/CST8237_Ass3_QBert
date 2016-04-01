using UnityEngine;
using System.Collections;

public class ValueStorage: MonoBehaviour {
     //Variables for CreateStage
     public int currentLevel = 1;
     public int currentRound = 1;
     public int roundsPerLevel = 4;

     //Variables for GameController
     public int currentLives = 3;
     public int currentScore = 0;
    
     //Variable for ChangeTile
     public int colourChangePerRound = 1;

     //Variables for MoveEnemy
     public float enemyMoveDelayTime = 1.5f;
     public float enemyMoveRepeatTime = 1.0f;

     //Variables for SpawnEnemy
     public float enemySpawnDelayTime = 0.0f;
     public float enemySpawnRepeatTime = 5.0f;
     public int maxEnemySpawnedCount = 3;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
