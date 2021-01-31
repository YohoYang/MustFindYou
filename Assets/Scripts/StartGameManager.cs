using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartGameManager : MonoBehaviour
{
    private Button btn_StartGame;
    private Button btn_Exit;
    private Button btn_Full;
    private Button btn_window;
    private Button btn_Group;

    private GameObject GroupPanel;
    private void Awake()
    {
        btn_StartGame = GameObject.Find("StartGame").GetComponent<Button>();
        btn_Exit= GameObject.Find("Exit").GetComponent<Button>();
        btn_Full = GameObject.Find("FullScreenBtn").GetComponent<Button>();
        btn_window= GameObject.Find("WindowScreenBtn").GetComponent<Button>();
        btn_Group = GameObject.Find("Credits").GetComponent<Button>();
        GroupPanel = GameObject.Find("GroupMessage");
    }
    private void Start()
    {
        btn_StartGame.onClick.AddListener(StartGameEvent);
        btn_Exit.onClick.AddListener(ExitEvent);
        btn_Full.onClick.AddListener(FullScreen);
        btn_window.onClick.AddListener(WindowScreen);
        btn_Group.onClick.AddListener(GroupEvent);
        GroupPanel.SetActive(false);
        if(Screen.fullScreen)
        {
            btn_Full.interactable = false;
            btn_window.interactable = true;
        }
    }

    
    private void GroupEvent()
    {
        GroupPanel.SetActive(!GroupPanel.activeSelf);
    }
    private void StartGameEvent()
    {
        SceneManager.LoadScene(1);
    }
    private void ExitEvent()
    {
        Application.Quit();
    }

    private void FullScreen()
    {        
        Screen.SetResolution(1920, 1080, true);
        btn_Full.interactable = false;
        btn_window.interactable = true;
    }

    private void WindowScreen()
    {
        Screen.SetResolution(1920, 1080, false);
        btn_Full.interactable = true;
        btn_window.interactable = false;
    }
}
