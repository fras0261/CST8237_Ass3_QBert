using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    private int _leftElevatorX = -15;
    private int _rightElevatorX = -9;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        GameObject player = GameObject.FindGameObjectWithTag("PlayerHead").gameObject;

        if (col.gameObject.CompareTag("Elevator"))
        {
            GameObject elevatorObject = col.gameObject;
            if (elevatorObject.gameObject.transform.position.x == _leftElevatorX)
            {
                player.GetComponent<MovePlayer>().MovePlayerWithElevator("left");
                col.gameObject.GetComponentInChildren<MoveElevator>().AnimateElevator();
            }
            else if (elevatorObject.gameObject.transform.position.x == _rightElevatorX)
            {
                player.GetComponent<MovePlayer>().MovePlayerWithElevator("right");
                col.gameObject.GetComponentInChildren<MoveElevator>().AnimateElevator();
            }
        }
    }
}

