using UnityEngine;
using System.Collections;

public class MoveElevator : MonoBehaviour {

    Animator elevatorAnimator;
    //Transform elevatorParent;

    //private bool _inMotion = false;
    private int _leftElevatorX = -15;
    private int _rightElevatorX = -9;

    // Use this for initialization
    void Start () {
        elevatorAnimator = gameObject.GetComponent<Animator>();
	}

    public void AnimateElevator()
    {
        if (transform.position.x == _leftElevatorX)
        {
            elevatorAnimator.SetInteger("MovementDirection", 1);
            StartCoroutine(WaitForAnimation());
        }
        if (transform.position.x == _rightElevatorX)
        {
            elevatorAnimator.SetInteger("MovementDirection", 2);
            StartCoroutine(WaitForAnimation());
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
