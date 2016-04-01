using UnityEngine;
using System.Collections;

public class CreateStage : MonoBehaviour {

    public GameObject platform;
    public GameObject platformSurface;
    public GameObject elevatorPlatform;

    private Vector3 _startCoordinate = new Vector3(-14, -2, -2);
    private Vector3 _elevatorCoordinate1 = new Vector3(-15, 0.35f, -1); //Spawn coordinate of the left elevator
    private Vector3 _elevatorCoordinate2 = new Vector3(-9, 0.35f, 5); //Spawn coordinate of the right elevator

    private const float SURFACE_PANEL_HEIGHT = 0.501f; //Distance above the platforms the panels should spawn
    private const int NUM_OF_FLOORS = 7;
    private const int PLATFORMS_PER_FLOOR = 7;

	// Use this for initialization
	void Start () {
        GenerateStage();
	}
	
    /// <summary>
    /// Generates the stage level by level in the shape of a pyramid
    /// </summary>
    private void GenerateStage()
    {
        int platformsCounter = PLATFORMS_PER_FLOOR;
        int platformPositionCounter = 0;
      
        Vector3 nextPlatformCoordinate = _startCoordinate;
        //Take the coordinates of cube platform to generated and set the coordinates for the platform surfaces pixels above the cube
        Vector3 nextSurfaceCoordinate = nextPlatformCoordinate; 
        nextSurfaceCoordinate.y += SURFACE_PANEL_HEIGHT;

        //Loop once for each floor to be generated.
        for (int i = 0; i < NUM_OF_FLOORS; i++)
        {
            //Loop to instantiate the platforms on this floor
            for (int j = 0; j < platformsCounter; j++)
            {
                Instantiate(platform, nextPlatformCoordinate, Quaternion.identity);
                Instantiate(platformSurface, nextSurfaceCoordinate, Quaternion.identity);

                nextPlatformCoordinate.x++;
                nextPlatformCoordinate.z++;

                nextSurfaceCoordinate = nextPlatformCoordinate;
                nextSurfaceCoordinate.y += SURFACE_PANEL_HEIGHT;
            }
            platformsCounter--;
            platformPositionCounter++;
            
            nextPlatformCoordinate = _startCoordinate;
            nextPlatformCoordinate.y += platformPositionCounter;
            nextPlatformCoordinate.z += platformPositionCounter;

            nextSurfaceCoordinate = nextPlatformCoordinate;
            nextSurfaceCoordinate.y += SURFACE_PANEL_HEIGHT;
            
        }

        //Create the elevator platforms
        Instantiate(elevatorPlatform, _elevatorCoordinate1, Quaternion.identity);
        Instantiate(elevatorPlatform, _elevatorCoordinate2, Quaternion.identity);

    }
}
