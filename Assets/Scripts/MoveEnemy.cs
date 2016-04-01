using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public AudioClip jumpAudio;
   
    private ValueStorage valueStorage;
    private GameObject _playerPosition;

    Animator enemyAnimator;
    Transform enemyParent;
    SpawnEnemy spawnEnemy;
    AudioSource _audio;
    
    private bool _inMotion = false;
    private Transform _playerCurrentPosition;
    private float _delayTime = 1.5f;
    private float _repeatTime;

    // Use this for initialization
    void Start () {
        _audio = GetComponent<AudioSource>();

        valueStorage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ValueStorage>();

        _delayTime = valueStorage.enemyMoveDelayTime;
        _repeatTime = valueStorage.enemyMoveRepeatTime;

        enemyAnimator = GetComponent<Animator>();
        enemyParent = this.gameObject.transform.parent;

        spawnEnemy = GameObject.Find("GameController").GetComponent<SpawnEnemy>();

        _playerPosition = GameObject.Find("PlayerPosition");

        _playerCurrentPosition = _playerPosition.transform;

        if (!IsInvoking("EnemyTileMovement"))
           InvokeRepeating("EnemyTileMovement", _delayTime, _repeatTime);
	}

    /// <summary>
    /// 
    /// </summary>
    public void EnemyTileMovement()
    {
        if (_inMotion == false)
        {
            _playerCurrentPosition.position = _playerPosition.transform.position;
            float xDif = Mathf.Abs(this.transform.position.x - _playerCurrentPosition.transform.position.x);
            float zDif = Mathf.Abs(this.transform.position.z - _playerCurrentPosition.transform.position.z);

            //If there is a smaller distance between the player and the enemy on the x Axis then the enemy will move to the southeast
            if (xDif > zDif)
            {
                _inMotion = true;
                enemyAnimator.SetInteger("MovementDirection", 1);
                StartCoroutine(WaitForAnimationCompletion(southEastMovement));
            }
            //If there is a smaller distance between the player and the enemy on the z Axis then the enemy will move to the southwest
            else if (xDif < zDif)
            {
                _inMotion = true;
                enemyAnimator.SetInteger("MovementDirection", 2);
                StartCoroutine(WaitForAnimationCompletion(southWestMovement));
            }
            //If the distance on the x and z axis are equal then the direction the enemy will move will be randomly selected 
            else
            {
                int rndVal = (int)Random.Range(1, 1000);
                
                if (rndVal > 500)
                {
                    _inMotion = true;
                    enemyAnimator.SetInteger("MovementDirection", 1);
                    StartCoroutine(WaitForAnimationCompletion(southEastMovement));
                }
                else
                {
                    _inMotion = true;
                    enemyAnimator.SetInteger("MovementDirection", 2);
                    StartCoroutine(WaitForAnimationCompletion(southWestMovement));
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="movementDirection"></param>
    /// <returns></returns>
    private IEnumerator WaitForAnimationCompletion(Vector3 movementDirection)
    {
        _audio.PlayOneShot(jumpAudio, 0.5f);
        yield return new WaitForSeconds(0.533f);
        enemyParent.transform.Translate(movementDirection);
        transform.localPosition = Vector3.zero;

        enemyAnimator.SetInteger("MovementDirection", 0);
        _inMotion = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("BackPlane"))
        {
            spawnEnemy.DestoryEnemyObject(transform.parent.gameObject);
           // Destroy(transform.parent.gameObject);
        }

        if (obj.CompareTag("PlayerHead"))
        {

            spawnEnemy.DestoryAllEnemies();
        }
    }

    public Vector3 southEastMovement { get { return new Vector3(1, -1, 0); } }
    public Vector3 southWestMovement { get { return new Vector3(0, -1, -1); } }

}
