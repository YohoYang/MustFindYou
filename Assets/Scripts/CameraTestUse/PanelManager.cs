using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using EnumSpace;
public class PanelManager : MonoBehaviour
{
    private Image panel_One;
    private Image panel_Two;    
    private PanelState panelOne_State;
    private PanelState panelTwo_State;    
    private bool panelOneIsChange;
    private bool panelTwoIsChange;
    private bool isPause;
    private bool isOverGame;

    public Transform followTarget_One;
    public Transform followTarget_Two;
    private Camera camera_One;
    private Camera camera_Two;


    //结算面板按钮
    private Button btn_win_Menu;
    private Button btn_win_Next;
    private Button btn_lost_Restart;
    private Button btn_lost_Menu;
    //暂停面板按钮
    private Button btn_pause_Restart;
    private Button btn_pause_Menu;
    private Button btn_pause_Resume;
    private Button btn_pause_Exit;

    private GameObject winPanel;
    private GameObject lostPanel;
    private GameObject pausePanel;
    private GameObject winPanel_menuAnchors;
    private GameObject hammerNumOnePanel;

    private TMP_Text tmp_OneDistance;
    private TMP_Text tmp_OneHP;
    private TMP_Text tmp_TwoDistance;
    private TMP_Text tmp_TwoHP;
    private TMP_Text tmp_hammerNumOne;

    public float fadeSpeed;//屏幕状态改变的速度
    public float minDistance;//距离达到该值后会显示距离UI

    public bool IsOverGame
    {
        get { return isOverGame; }
        set { isOverGame = value; }
    }

    private void Awake()
    {
        panel_One = GameObject.Find("PanelOne").GetComponent<Image>();
        panel_Two = GameObject.Find("PanelTwo").GetComponent<Image>();
        winPanel = GameObject.Find("WinPanel");
        lostPanel = GameObject.Find("LostPanel");
        pausePanel = GameObject.Find("PausePanel");
        hammerNumOnePanel = GameObject.Find("CameraOneCanvas/hammerNum");

        winPanel_menuAnchors = GameObject.Find("WinPanel/Panel/AnchorsPos");
        tmp_OneDistance = GameObject.Find("CameraOneCanvas/One_Distance").GetComponent<TMP_Text>();
        tmp_OneHP = GameObject.Find("CameraOneCanvas/One_HP").GetComponent<TMP_Text>();
        tmp_TwoDistance = GameObject.Find("CameraTwoCanvas/Two_Distance").GetComponent<TMP_Text>();
        tmp_TwoHP = GameObject.Find("CameraTwoCanvas/Two_HP").GetComponent<TMP_Text>();
        tmp_hammerNumOne = GameObject.Find("CameraOneCanvas/hammerNum/Nums").GetComponent<TMP_Text>();
        camera_One = GameObject.Find("PlayerOneCamera").GetComponent<Camera>();
        camera_Two = GameObject.Find("PlayerTwoCamera").GetComponent<Camera>();

        btn_win_Menu = GameObject.Find("WinPanel/Panel/ReStart").GetComponent<Button>();
        btn_win_Next = GameObject.Find("WinPanel/Panel/Next").GetComponent<Button>();
        btn_lost_Restart = GameObject.Find("LostPanel/Panel/ReStart").GetComponent<Button>();
        btn_lost_Menu = GameObject.Find("LostPanel/Panel/Menu").GetComponent<Button>();

        btn_pause_Restart = GameObject.Find("PausePanel/Pause_Restart").GetComponent<Button>();
        btn_pause_Menu = GameObject.Find("PausePanel/Pause_Menu").GetComponent<Button>();
        btn_pause_Resume = GameObject.Find("PausePanel/Pause_Resume").GetComponent<Button>();
        btn_pause_Exit = GameObject.Find("PausePanel/Pause_Exit").GetComponent<Button>();

        //绑定按钮事件
        btn_win_Menu.onClick.AddListener(BackMenuScene);
        btn_win_Next.onClick.AddListener(LoadNextScene);
        btn_lost_Restart.onClick.AddListener(RestartScene);
        btn_lost_Menu.onClick.AddListener(BackMenuScene);

        btn_pause_Restart.onClick.AddListener(RestartScene);
        btn_pause_Menu.onClick.AddListener(BackMenuScene);
        btn_pause_Exit.onClick.AddListener(QuitGame);
        btn_pause_Resume.onClick.AddListener(ResumeGame);
    }
    private void Start()
    {
        panelOne_State = PanelState.None;
        panelTwo_State = PanelState.None;
        winPanel.SetActive(false);
        lostPanel.SetActive(false);
        hammerNumOnePanel.SetActive(false);
        isOverGame = false;
        ActiveGame();
    }
    private void Update()
    {        
        PauseGameControl();
        PanelOneControl();
        PanelTwoControl();

        Up_DistanceUIPos();
        Up_HpUIPos();
    }


    private void Up_DistanceUIPos()
    {
        Vector3 newposOne = followTarget_One.position;
        tmp_OneDistance.transform.position = new Vector3(newposOne.x+0.05f, newposOne.y-0.7f, tmp_OneDistance.transform.position.z);

        Vector3 newposTwo = followTarget_Two.position;
        tmp_TwoDistance.transform.position = new Vector3(newposTwo.x + 0.05f, newposTwo.y - 0.7f, tmp_TwoDistance.transform.position.z);
    }
    private void Up_HpUIPos()
    {
        Vector3 newposOne = followTarget_One.position;
        tmp_OneHP.transform.position = new Vector3(newposOne.x + 0.05f, newposOne.y + 2f, tmp_OneDistance.transform.position.z);

        Vector3 newposTwo = followTarget_Two.position;
        tmp_TwoHP.transform.position = new Vector3(newposTwo.x + 0.05f, newposTwo.y + 2f, tmp_TwoDistance.transform.position.z);
    }

    /// <summary>
    /// 更新距离UI    
    /// </summary>
    /// <param name="currentDistance"></param>
    public void UpdateDistanceUI(float currentDistance,PanelType pt)
    {
        float cacuDis = currentDistance;

        switch (pt)
        {
            case PanelType.Left:
                if (cacuDis > minDistance || cacuDis < 2f)
                    tmp_OneDistance.text = "";
                else
                {
                    //tmp_OneDistance.gameObject.SetActive(true);
                    tmp_OneDistance.text = cacuDis.ToString() + "m";
                }
                break;
            case PanelType.Right:
                if (cacuDis > minDistance || cacuDis < 2f)
                    tmp_TwoDistance.text ="";
                else
                {
                    //tmp_TwoDistance.gameObject.SetActive(true);
                    tmp_TwoDistance.text = cacuDis.ToString() + "m";
                }
                break;
        }
        
    }
    /// <summary>
    /// 更新步数UI
    /// </summary>
    /// <param name="currentHp"></param>
    public void UpdateHpUI(float currentHp,PanelType pt)
    {
        switch (pt)
        {
            case PanelType.Left:
                tmp_OneHP.text = currentHp.ToString();
                if (currentHp <= 0)
                    tmp_OneHP.text = "";
                break;
            case PanelType.Right:
                tmp_TwoHP.text = currentHp.ToString();
                if (currentHp <= 0)
                    tmp_OneHP.text = "";
                break;
        }        
    }
    /// <summary>
    /// 更新锤子数量UI
    /// </summary>
    /// <param name="hammerNum"></param>
    public void UpdateHammerUI(float hammerNum)
    {
        if(hammerNum<=0)
        {
            hammerNumOnePanel.SetActive(false);
        }
        else
        {
            hammerNumOnePanel.SetActive(true);
            tmp_hammerNumOne.text = "x" + hammerNum;
        }
    }

    /// <summary>
    /// 游戏暂停控制
    /// </summary>
    private void PauseGameControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!isOverGame)
        {
            if (!isPause)
            {
                PauseGame();
                isPause = true;
            }
            else if (isPause)
            {
                ActiveGame();
                isPause = false;
            }
        }
    }
    private void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    private void ActiveGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    private void ResumeGame()
    {
        ActiveGame();
        isPause = false;
    }
    

    #region 屏幕遮罩控制
    private void PanelOneControl()
    {
        //根据传递的屏幕状态类型来改变屏幕状态
        if (!panelOneIsChange)
            return;

        switch (panelOne_State)
        {
            case PanelState.None:
                break;
            case PanelState.Fade:
                FadePanel(panel_One,PanelType.Left);
                break;
            case PanelState.Show:
                ShowPanel(panel_One, PanelType.Left);
                break;
        }
    }
    private void PanelTwoControl()
    {
        //根据传递的屏幕状态类型来改变屏幕状态
        if (!panelTwoIsChange)
            return;

        switch (panelTwo_State)
        {
            case PanelState.None:
                break;
            case PanelState.Fade:
                FadePanel(panel_Two, PanelType.Right);
                break;
            case PanelState.Show:
                ShowPanel(panel_Two, PanelType.Right);
                break;
        }
    }

    /// <summary>
    /// 隐藏屏幕
    /// </summary>
    private void FadePanel(Image targetPanel,PanelType pt)
    {       
        if(targetPanel.color.a<0.8f)
        {            
            //播放隐藏动画效果
            float ap = targetPanel.color.a + Time.deltaTime*fadeSpeed;
            targetPanel.color = new Color(targetPanel.color.r, targetPanel.color.g, targetPanel.color.b, ap);
        }
        else
        {
            //完成隐藏动画
            targetPanel.color = new Color(targetPanel.color.r, targetPanel.color.g, targetPanel.color.b, 0.8f);

            if (pt == PanelType.Left)
                panelOneIsChange = false;
            else if (pt == PanelType.Right)
                panelTwoIsChange = false;
        }
    }
    /// <summary>
    /// 显示屏幕
    /// </summary>
    private void ShowPanel(Image tagetPanel, PanelType pt)
    {
        if (tagetPanel.color.a > 0f)
        {
            //播放隐藏动画效果
            float ap = tagetPanel.color.a - Time.deltaTime * fadeSpeed;
            tagetPanel.color = new Color(tagetPanel.color.r, tagetPanel.color.g, tagetPanel.color.b, ap);
        }
        else
        {
            //完成隐藏动画
            tagetPanel.color = new Color(tagetPanel.color.r, tagetPanel.color.g, tagetPanel.color.b, 0f);

            if (pt == PanelType.Left)
                panelOneIsChange = false;
            else if (pt == PanelType.Right)
                panelTwoIsChange = false;
        }
    }
    /// <summary>
    /// 改变目标屏幕状态
    /// </summary>
    /// <param name="ps"></param>
    public void ChangePanelState(PanelState ps,PanelType pt)
    {
        switch (pt)
        {
            case PanelType.Left:
                panelOne_State = ps;
                panelOneIsChange = true;
                break;
            case PanelType.Right:
                panelTwo_State = ps;
                panelTwoIsChange = true;
                break;
            default:
                break;
        }                
       
    }

    #endregion

    #region 结算面板按钮事件
    /// <summary>
    /// 加载场景的按钮事件
    /// </summary>
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// 加载场景的按钮事件
    /// </summary>
    private void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
    }
    /// <summary>
    /// 加载场景的按钮事件
    /// </summary>
    private void BackMenuScene()
    {
        SceneManager.LoadScene(0);
    }
    private void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// 关闭Next按钮
    /// </summary>
    public void CloseNextButton()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            btn_win_Next.gameObject.SetActive(false);
            btn_win_Menu.transform.position = winPanel_menuAnchors.transform.position;
        }        
    }
    #endregion
}
