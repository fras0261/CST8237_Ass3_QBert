using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonControls : MonoBehaviour {

    public AudioClip clickSound;

    private AudioSource _audio;

     private ValueStorage valueStorage;

	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
        valueStorage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ValueStorage>();
    }

    public void StartNewGame()
    {
        _audio.PlayOneShot(clickSound);
        ResetValueStorage();
        SceneManager.LoadScene("GameScene");
    }

    public void GoToHowToScene()
    {
        _audio.PlayOneShot(clickSound);
        SceneManager.LoadScene("HowToScene");
    }

    public void ReturnToMenuScene()
    {
        _audio.PlayOneShot(clickSound);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitGame()
    {
        _audio.PlayOneShot(clickSound);
        Destroy(valueStorage.gameObject);
        Application.Quit();
    }

    /// <summary>
    /// 
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

}
