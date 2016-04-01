using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour {

    private ValueStorage valueStorage;

    AudioSource _audio;

    public GameObject enemyObject;
    public AudioClip spawnAudio;
    public AudioClip hitAudio;
    public AudioClip fallAudio;

    private int _enemyCount = 0;
    private int _maxEnemyCount = 3;
    private float _delayTime = 0.0f;
    private float _spawnTime;

    private List<GameObject> _enemyRoster = new List<GameObject>(); //List of enemy game objects

    private Vector3 _spawnCoordinate1 = new Vector3(-14, 10, 3); //Left spawn location
    private Vector3 _spawnCoordinate2 = new Vector3(-13, 10, 4); //Right Spawn location

    // Use this for initialization
    void Start () {
        _audio = GetComponent<AudioSource>();

        valueStorage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ValueStorage>();
        _maxEnemyCount = valueStorage.maxEnemySpawnedCount;

        _delayTime = valueStorage.enemySpawnDelayTime;
        _spawnTime = valueStorage.enemySpawnRepeatTime;

        InvokeRepeating("SpawnEnemyObject", _delayTime, _spawnTime);
	}
	
    /// <summary>
    /// Generates a random number to determine if the enemy object will spawn at either left or right location
    /// </summary>
    private void SpawnEnemyObject()
    {
        //Only spawn a new enemy object when the number currently spawned are less than max number of enemy spawnable
        if (_enemyCount < _maxEnemyCount)
        {
            int rndVal = (int)Random.Range(1, 1000);

            if (rndVal < 500)
            {
                 _enemyRoster.Add((GameObject) Instantiate(enemyObject, _spawnCoordinate1, Quaternion.identity));

                _enemyCount++;
            }
            else
            {
                _enemyRoster.Add((GameObject)Instantiate(enemyObject, _spawnCoordinate2, Quaternion.identity));
                _enemyCount++;
            }

            _audio.PlayOneShot(spawnAudio);
        }
    }

    /// <summary>
    /// Decrease the count of the number of enemies currently spawned
    /// </summary>
    public void DestoryEnemyObject(GameObject enemyToDestory)
    {
        _audio.PlayOneShot(fallAudio, 0.5f);

        int enemyIndex = _enemyRoster.IndexOf(enemyToDestory);
        _enemyRoster.RemoveAt(enemyIndex);

        Destroy(enemyToDestory, 0.3f);
        _enemyCount--;
    }

    /// <summary>
    /// Destroy all enemies generated
    /// </summary>
    public void DestoryAllEnemies()
    {
        foreach (GameObject enemy in _enemyRoster)
            GameObject.Destroy(enemy);

        _enemyRoster.Clear();
        _enemyCount = 0;
        _audio.PlayOneShot(hitAudio);
    }
    
}
