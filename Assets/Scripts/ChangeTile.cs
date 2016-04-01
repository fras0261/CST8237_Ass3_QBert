using UnityEngine;
using System.Collections;

public class ChangeTile : MonoBehaviour {

    private ValueStorage valueStorage;
    Renderer tileRenderer = new Renderer();
    GameController gameController;

    private Color[] _colourPalette = new Color[6];
    private int _colourSelect = 0;
    private int _changeCount = 0;
    private int _maxColorChangesOnLevel;
    private int _pointsValue = 50;

	// Use this for initialization
	void Start () {
        valueStorage = GameObject.FindGameObjectWithTag("Storage").GetComponent<ValueStorage>();
        _maxColorChangesOnLevel = valueStorage.colourChangePerRound;

        tileRenderer = this.gameObject.GetComponent<Renderer>();
        CreateColorPalet();

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SetNumColorChangesPerLevel(_maxColorChangesOnLevel);
	}

    /// <summary>
    /// Create an array of colors to cycle through
    /// </summary>
    void CreateColorPalet()
    {
        _colourPalette[0] = Color.red;
        _colourPalette[1] = new Color(1, 1, 0);
        _colourPalette[2] = Color.green;
        _colourPalette[3] = new Color(0, 1, 1);
        _colourPalette[4] = Color.blue;
        _colourPalette[5] = new Color(1, 0, 1);
    }

    /// <summary>
    /// Whent the player lands on a tile whose colour can still be changed then the tile's colour will change to the next colour in the colour palette.
    /// </summary>
    /// <param name="obj"></param>
    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player") && _changeCount < _maxColorChangesOnLevel )
        {
            tileRenderer.material.color = _colourPalette[_colourSelect];
            _colourSelect = ++_colourSelect % 6;
            _changeCount++;

            gameController.UpdateScore(_pointsValue);
            gameController.IncreaseTileChangeCount();
        }
    }
}
