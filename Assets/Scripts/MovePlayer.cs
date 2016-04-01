using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    private ValueStorage valueStorage;
    Animator playerAnimator;
    Transform playerParent;
    AudioSource _audio;
    
    public GameController gameController;
    public AudioClip jumpAudio;
    public AudioClip fallAudio;

    private bool _inMotion = false;

    // Use this for initialization
    void Start()
    {
        playerParent = this.gameObject.transform.parent;
        playerAnimator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTileMovement();
    }

    /// <summary>
    /// Controls the player movement based on the input of the from the arrow keys
    /// </summary>
    private void PlayerTileMovement()
    {
        //Inputs are not taken while the player is in motion
        if (_inMotion == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _inMotion = true; 
                playerAnimator.SetInteger("MovementDirection", 1);
                StartCoroutine(WaitForInputAnimationCompletion(northEastMovement));
                
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _inMotion = true;
                playerAnimator.SetInteger("MovementDirection", 2);
                StartCoroutine(WaitForInputAnimationCompletion(southEastMovement));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _inMotion = true;
                playerAnimator.SetInteger("MovementDirection", 3);
                StartCoroutine(WaitForInputAnimationCompletion(southWestMovement));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _inMotion = true;
                playerAnimator.SetInteger("MovementDirection", 4);
                StartCoroutine(WaitForInputAnimationCompletion(northWestMovement));
            }

            
        }
    }

    /// <summary>
    /// Waits for the movement animation to complete before moving the parent 
    /// </summary>
    /// <param name="movementDirection">A Vector3 containing the values for parent translation</param>
    /// <returns></returns>
    private IEnumerator WaitForInputAnimationCompletion(Vector3 movementDirection)
    {
        _audio.PlayOneShot(jumpAudio, 0.50f);
        yield return new WaitForSeconds(0.533f);
        playerParent.transform.Translate(movementDirection); 
        transform.localPosition = Vector3.zero; //Reset to the parent location

        playerAnimator.SetInteger("MovementDirection", 0); //Return to idle animation
        _inMotion = false;
    }

    /// <summary>
    /// System waits for the elevator animation to complete
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForElevatorAnimationCompletion()
    {
        yield return new WaitForSeconds(1.0f);
        playerParent.transform.position = startCoordinates;
        transform.localPosition = Vector3.zero; //Reset to the parent location

        playerAnimator.SetInteger("MovementDirection", 0); //Return to idle animation
        _inMotion = false;
    }

    /// <summary>
    /// Checks which elevator the player lands on animates the appropriate motion
    /// </summary>
    /// <param name="elevatorSide"></param>
    public void MovePlayerWithElevator(string elevatorSide)
    {        
        if (elevatorSide.Equals("left")) //If the player lands on the left elevator
        {
            _inMotion = true;
            playerAnimator.SetInteger("MovementDirection", 5);
            StartCoroutine(WaitForElevatorAnimationCompletion());
        }
        else if (elevatorSide.Equals("right")) //If the player lands on the right elevator
        {
            _inMotion = true;
            playerAnimator.SetInteger("MovementDirection", 6);
            StartCoroutine(WaitForElevatorAnimationCompletion());
        }
    }

    public IEnumerator WaitForAudioEnd()
    {
        _audio.PlayOneShot(fallAudio);
        yield return new WaitForSeconds(fallAudio.length);
        playerParent.transform.position = new Vector3(-14, 5.27f, 4);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    void OnTriggerEnter(Collider obj)
    {
        //If the player falls off the platform
        if (obj.CompareTag("BackPlane"))
        {
            StartCoroutine(WaitForAudioEnd());
            gameController.LoseLife();
        }

        //If the player collides with an enemy body
        if (obj.CompareTag("EnemyBody"))
        {
            gameController.LoseLife();
        }
    }
    public Vector3 northEastMovement { get { return new Vector3(0, 1, 1); } }
    public Vector3 southEastMovement { get { return new Vector3(1, -1, 0); } }
    public Vector3 southWestMovement { get { return new Vector3(0, -1, -1); } }
    public Vector3 northWestMovement { get { return new Vector3(-1, 1, 0); } }
    public Vector3 startCoordinates  { get { return new Vector3(-14, 5.275f, 4);  } }
}
