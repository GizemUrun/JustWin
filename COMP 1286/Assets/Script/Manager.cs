using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static int coins, lives;
    public enum DoorKeyColours { Red, Blue, Yellow};
    public static bool redKey, blueKey, yellowKey;
    public static Vector3 lastCheckPoint;
    public static bool gamePaused;
    static GameUI gameUI;
    public Transform[] cheatPoints;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameUI = FindObjectOfType<GameUI>();
        lives = 3;
        coins = 0;
        gameUI.UpdateCoins();
        gameUI.UpdateLives();
    }

    public static void AddCoins(int coinValue)
    {
        coins += coinValue;
        if (coins >= 100) //if there are more than 100 coins take them away and give 1 life to player
        {
            coins -= 100;
            AddLives(1);
        }
        gameUI.UpdateCoins();
    }

    public static void AddLives(int lifeValue)
    {
        lives += lifeValue;
        if(lives < 0)
        {
            gameUI.CheckGameState(GameUI.GameState.GameOver); //gameover if all lives are lost
        }
        else
        {
            gameUI.UpdateLives();
        }
    }

    public static void KeyPickup(DoorKeyColours keyColour)
    {
        switch (keyColour)
        {
            case DoorKeyColours.Red:
                redKey = true;
                break;
            case DoorKeyColours.Blue:
                blueKey = true;
                break;
            case DoorKeyColours.Yellow:
                yellowKey = true;
                break;
        }
        gameUI.UpdateKeys(keyColour);
    }

    public static void UpdateCheckPoints(GameObject flag)
    {
        lastCheckPoint = flag.transform.position;
        CheckPoint[] allCheckPoints = FindObjectsOfType<CheckPoint>();
        foreach (CheckPoint cp in allCheckPoints)
        {
            if(cp != flag.GetComponent<CheckPoint>())
            {
                cp.LowerFlag();
            }
        }
    }

    private void Update()
    {
        CheatModes();
    }

    void CheatModes()
    {
        if (Input.GetKeyDown(KeyCode.R)) KeyPickup(DoorKeyColours.Red);
        if (Input.GetKeyDown(KeyCode.B)) KeyPickup(DoorKeyColours.Blue);
        if (Input.GetKeyDown(KeyCode.Y)) KeyPickup(DoorKeyColours.Yellow);
        if (Input.GetKeyDown(KeyCode.L)) AddLives(1);
        if (Input.GetKeyDown(KeyCode.Alpha0)) GameObject.FindGameObjectWithTag("Player").transform.position = cheatPoints[0].position;
        if (Input.GetKeyDown(KeyCode.Alpha1)) GameObject.FindGameObjectWithTag("Player").transform.position = cheatPoints[1].position;
    }
}
