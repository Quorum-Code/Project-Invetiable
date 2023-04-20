using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState { MainMenu, Settings, Dungeon, Hideout, None };

    [SerializeField] bool startLevelController;

    [SerializeField] LevelController LC;

    // Start is called before the first frame update
    void Start()
    {
        // Test state for jumping straight into a generated level
        if (startLevelController) 
        {
            LC.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
