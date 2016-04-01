using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private ValueStorage valueStorage;
    AudioSource _audio;

    public Text scoreText;
    public Text livesText;
    public Text levelText;
    public Text roundText;

    public AudioClip nextRoundAudio;
    public AudioClip nextLevelAudio;
    public AudioClip pointsAudio;

    private int _lives;
    private const int MAX_SCORE = 999999999;
    private int _score;

    private int _currentLevel;
    private int _currentRound;
    private int _roundsPerLevel;

    private const int NUM_TILES = 28; //The total number of platforms that make up the level
    private int _numTilesChanged = 0; //The number of tiles that have been changed after a player lands on them
    private int _maxColorChangesOnLevel;

    private bool _proceedingToNext = false;

    // Use this for initialization
    void Start () {
        _audio = GetComponent<AudioSource>();

        valueStorage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ValueStorage>();

        //Set values
        _lives = valueStorage.currentLives;
        _currentLevel = valueStorage.currentLevel;
        _currentRound = valueStorage.currentRound;
        _roundsPerLevel = valueStorage.roundsPerLevel;
        _score = valueStorage.currentScore;

        scoreText.text = "Score:"  + _score;
        livesText.text = "Lives: " + _lives;
        levelText.text = "Level: " + _currentLevel;
        roundText.text = "Round: " + _currentRound;
    }
	
	// Update is called once per frame
	void Update () {
        ToNextRound();
	}

    /// <summary>
    /// Called to decrease lives and updates the display updated life on the screeen
    /// </summary>
    public void LoseLife()
    {
        _lives--;
        livesText.text = "Lives: " + _lives;

        //If the player loses all of their lives then reset the values in storage and then load the game over scene
        if (_lives < 1)
        {
            ResetValueStorage();
            SceneManager.LoadScene("GameOverScene"); 
        }
            
    }

    /// <summary>
    /// Updates the user's score
    /// </summary>
    /// <param name="pointsToAdd">The number of points to add to the player's score</param>
    public void UpdateScore(int pointsToAdd)
    {
        _audio.PlayOneShot(pointsAudio);
        _score += pointsToAdd;
        if (_score > MAX_SCORE)
            _score = MAX_SCORE;
        scoreText.text = "Score: " + _score.ToString("D9");
    }

    /// <summary>
    /// Manages the level and round increments whenever a player finishes a round
    /// </summary>
    public void ToNextRound()
    {
        int colourChangesToWin = _maxColorChangesOnLevel * NUM_TILES;

        //Checks to see if the system is already in the process of moving the next round
        if (_proceedingToNext == false)
        {
            if (colourChangesToWin == _numTilesChanged)
            {
                //If the player is completing the last round in a level
                if (_currentRound == _roundsPerLevel)
                {
                    _currentLevel++;
                    _currentRound = 1;
                    SetValueStoragePerRound(true);
                    _proceedingToNext = true;
                    StartCoroutine(WaitForSound(nextLevelAudio));
                }
                else
                {
                    _currentRound++;
                    SetValueStoragePerRound(false);
                    _proceedingToNext = true;
                    StartCoroutine(WaitForSound(nextRoundAudio));
                } 
            }           
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nCCPL"></param>
    public void SetNumColorChangesPerLevel(int nCCPL)
    {
        _maxColorChangesOnLevel = nCCPL;
    }

    /// <summary>
    /// 
    /// </summary>
    public void IncreaseTileChangeCount()
    {
        _numTilesChanged++;
    }

    /// <summary>
    /// Set the lives, score, round and level into the Value Storage
    /// </summary>
    private void SetValueStoragePerRound(bool isNewLvl)
    {
        valueStorage.currentLives = _lives;        
        valueStorage.currentScore = _score;
        valueStorage.currentRound = _currentRound;
        valueStorage.currentLevel = _currentLevel;

        SetDifficultyValues(isNewLvl);        
    }

    /// <summary>
    /// Resets the values stored in the ValueStorage back to the default
    /// </summary>
    private void ResetValueStorage()
    {

        valueStorage.currentLevel = 1;
        valueStorage.currentRound = 1;
        valueStorage.roundsPerLevel = 4;

        valueStorage.currentLives = 3;
        valueStorage.currentScore = 0;

        valueStorage.colourChangePerRound = 1;

        valueStorage.enemyMoveDelayTime = 1.5f;
        valueStorage.enemyMoveRepeatTime = 1.0f;

        valueStorage.enemySpawnDelayTime = 0.0f;
        valueStorage.enemySpawnRepeatTime = 5.0f;
        valueStorage.maxEnemySpawnedCount = 3;
    }

    /// <summary>
    /// Increases the level based on the round and level that the system will be proceeding to
    /// </summary>
    private void SetDifficultyValues(bool isNewLvl)
    {
        //These values will increase only when proceeding to the next level
        if (isNewLvl)
        {
            //When the next level is an even number increase the number of times a tile needs to be landed on
            if (_currentLevel % 2 == 0)
                valueStorage.colourChangePerRound++;

            //When the next level is an odd number increase the number of enemy that can be spawned at one time
            if (_currentLevel % 2 == 1)
                valueStorage.maxEnemySpawnedCount++;

            //When the next level is a multiple of 4, increase the number of rounds in a level
            if (_currentLevel % 4 == 0)
                valueStorage.roundsPerLevel++; 
        }

        //Every second round will increase the speed of the enemy movements
        if (_currentRound % 4 == 2 && valueStorage.enemyMoveRepeatTime > 1.0f)
            valueStorage.enemyMoveRepeatTime -= 0.1f;

        //Every third round will decrease the enemy spawn time
        if (_currentRound % 4 == 3 && valueStorage.enemySpawnRepeatTime > 1.0f)
            valueStorage.enemyMoveRepeatTime -= 0.01f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForSound(AudioClip ac)
    {
        _audio.PlayOneShot(ac);
        yield return new WaitForSeconds(ac.length);
        SceneManager.LoadScene("GameScene");
    }

}
