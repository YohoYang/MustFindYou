using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using EnumSpace;

public class CameraManager : MonoBehaviour
{
    private TimelineAsset timeline_Start;
    private TimelineAsset timeline_Win;
    private TimelineAsset timeline_Lost;
    private AudioClip audio_NormalClip;
    private AudioClip audio_WinClip;
    private AudioClip audio_OneMove;
    private AudioClip audio_TwoMove;

    private Camera camera_One;
    private Camera camera_Two;
    public Transform followTarget_One;
    public Transform followTarget_Two;
    private PanelManager panel_Manager;    
    private PlayableDirector director;
    private AudioSource audio_Source;
    private AudioSource audio_OneSource;
    private AudioSource audio_TwoSource;
    private bool isPlayTimeline;

    public float followSpeed;

    /// <summary>
    /// 是否正在演出
    /// </summary>
    public bool IsPlayTimeline
    {
        get { return isPlayTimeline; }
    }
    private void Awake()
    {
        camera_One = GameObject.Find("PlayerOneCamera").GetComponent<Camera>();
        camera_Two = GameObject.Find("PlayerTwoCamera").GetComponent<Camera>();
        director = GetComponent<PlayableDirector>();
        audio_Source = GetComponent<AudioSource>();
        audio_OneSource = GameObject.Find("Player1").GetComponent<AudioSource>();
        audio_TwoSource = GameObject.Find("Player2").GetComponent<AudioSource>();
        panel_Manager = GameObject.Find("PanelManager").GetComponent<PanelManager>();

        LoadAllTimelines();        
        
    }
    private void Start()
    {
        director.playableAsset = timeline_Start;
        director.Play();
        isPlayTimeline = true;
    }

    
    private void LoadAllTimelines()
    {
        timeline_Start=Resources.Load<TimelineAsset>("Timelines/GameStartTimeline");
        timeline_Win= Resources.Load<TimelineAsset>("Timelines/WinTimeline");
        timeline_Lost = Resources.Load<TimelineAsset>("Timelines/LostTimeLine");
        audio_WinClip = Resources.Load<AudioClip>("Audios/Winter");
        audio_NormalClip = Resources.Load<AudioClip>("Audios/Luz__piano_");
        audio_OneMove= Resources.Load<AudioClip>("Audios/effecSound/boySound");
        audio_TwoMove= Resources.Load<AudioClip>("Audios/effecSound/girlSound");
    }
    private void LateUpdate()
    {
        CameraOneFollow();
        CameraTwoFollow();
    }
    private void CameraOneFollow()
    {
        if(followTarget_One!=null)
        {
            Vector3 newpos = new Vector3(followTarget_One.position.x, followTarget_One.position.y, camera_One.transform.position.z);            
            camera_One.transform.position = newpos;
        }
    }
    private void CameraTwoFollow()
    {
        if (followTarget_Two != null)
        {
            Vector3 newpos = new Vector3(followTarget_Two.position.x, followTarget_Two.position.y, camera_Two.transform.position.z);
            camera_Two.transform.position = newpos;
        }
    }

    /// <summary>
    /// 胜利演出
    /// </summary>
    public void WinEnd()
    {
        //显示所有屏幕
        panel_Manager.ChangePanelState(PanelState.Show, PanelType.Left);
        panel_Manager.ChangePanelState(PanelState.Show, PanelType.Right);

        director.playableAsset = timeline_Win;
        director.Play();
        audio_Source.clip = audio_WinClip;
        audio_Source.Play();
        isPlayTimeline = true;
        panel_Manager.IsOverGame = true;
    }
    /// <summary>
    /// 失败演出
    /// </summary>
    public void LostEnd()
    {
        //关闭所有屏幕
        panel_Manager.ChangePanelState(PanelState.Fade, PanelType.Left);
        panel_Manager.ChangePanelState(PanelState.Fade, PanelType.Right);

        director.playableAsset = timeline_Lost;
        director.Play();
        isPlayTimeline = true;
        panel_Manager.IsOverGame = true;
    }

    public void PlayerOneMoveSound()
    {
        audio_OneSource.PlayOneShot(audio_OneMove);
    }
    public void PlayerTwoMoveSound()
    {
        audio_TwoSource.PlayOneShot(audio_TwoMove);
    }

    #region timelineuse

    /// <summary>
    /// Timeline开始演出时需要使用，请勿调用
    /// </summary>
    public void TimelineStartGameUse()
    {
        panel_Manager.ChangePanelState(PanelState.Fade, PanelType.Right);
    }

    /// <summary>
    /// 用于没有下一关时
    /// </summary>
    public void TimelineWinGameUse()
    {
        panel_Manager.CloseNextButton();
    }
    /// <summary>
    /// 完成播放
    /// </summary>
    public void TimelineComplete()
    {
        isPlayTimeline = false;
    }
    #endregion
}
