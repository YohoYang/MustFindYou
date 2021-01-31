using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumSpace;

public class SwitchManager : MonoBehaviour
{
    public bool player1play;
    public int distance;
    public int wholeLife;
    public static bool gameStop ;
    private CameraManager cameraManager;
    private bool winEndPlayed;
    private bool lostEndPlayed;
    private PanelManager panelManager;
    // Start is called before the first frame update
    void Start()
    {
        player1play = true;
        cameraManager = GameObject.Find("CameraController").GetComponent<CameraManager>();
        panelManager= GameObject.Find("PanelManager").GetComponent<PanelManager>();
        gameStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTheDistance();
        Invoke("CalculateWholeLife", 1);

        if (cameraManager.IsPlayTimeline)
        {
            gameStop = true;
        }
        else
        {
            gameStop = false;
        }
        if (!lostEndPlayed)
        {
            Invoke("GameOver", 1.5f);
        }
        if (!winEndPlayed)
        {
            if (distance <= 1)
            {
                gameStop = true;
                cameraManager.WinEnd();
                winEndPlayed = true;
            }
            
        }        
        panelManager.UpdateDistanceUI(distance, PanelType.Left);
        panelManager.UpdateDistanceUI(distance, PanelType.Right);
        panelManager.UpdateHpUI(P1MoveManager.RemainStep, PanelType.Left);
        panelManager.UpdateHpUI(P2MoveManager.RemainStep, PanelType.Right);



    }
    public  void SwitchPlayer()
    {
        if (player1play == true)
        {
            player1play = false;
        }
        else
        {
            player1play = true;
        }
    }
    void CalculateTheDistance()
    {
        distance = Mathf.RoundToInt((GameObject.Find("Player1").transform.position -GameObject.Find("Player2").transform.position).magnitude);
    }
    void CalculateWholeLife()
    {
        wholeLife = P1MoveManager.RemainStep + P2MoveManager.RemainStep;
    }
    void GameOver()
    {
        if (wholeLife == 0&& distance > 1)
        {
            gameStop = true;
            cameraManager.LostEnd();
            lostEndPlayed = true;
        }
    }

}
