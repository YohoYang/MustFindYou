using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using EnumSpace;

public class P2MoveManager : MonoBehaviour
{
    //玩家本回合位置
    private Vector2 PlayerPosition;
    //玩家行动的最大步数
    public int MaxStep;
    public float MoveSpeed;

    public static int RemainStep;
    SwitchManager p1Playing;
    float S = 0f;
    private bool isMoving;
    private bool canMove = true;
    private Vector3 newPosition = new Vector3(0, 0);
    private RaycastHit2D hit;
    private Ray2D ray;
    private Vector3 hitPoint;
    private RaycastHit2D hit2;
    private PanelManager panelManager;
    private Animator animaor;
    private float PlayerzValue;
    private bool LockMove;
    private MoveDir moveDirEnum;
    private CameraManager cameraManager;
    public GameObject get;
    public AudioSource audio;
    public AudioClip clip;
    public AudioClip clip2;

    void Start()
    {        
        RemainStep = MaxStep;
        p1Playing = GameObject.Find("SwitchManager").GetComponent<SwitchManager>();
        PlayerzValue = 1f;
       panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
        animaor = GetComponent<Animator>();
        LockMove = false;
        cameraManager = GameObject.Find("CameraController").GetComponent<CameraManager>();
    }

    void Update()
    {
        if (RemainStep <= 0)
        {
            gameObject.GetComponentInChildren<Light2D>().intensity = 0.5f;
        }
        if (!SwitchManager.gameStop)
        {
            if (p1Playing.player1play == false)
            {
                panelManager.ChangePanelState(PanelState.Show, PanelType.Right);
                //gameObject.GetComponentInChildren<Light2D>().enabled = true;
                if (RemainStep > 0)
                {
                    if (Input.GetKey(KeyCode.A) && !LockMove)
                    {
                        PlayerPosition = gameObject.GetComponent<Transform>().position;
                        hit2 = Physics2D.Raycast(transform.position + Vector3.left / 2, Vector2.left, 0.7f);
                        if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
                        {
                            MoveControlLogic(MoveDir.A);
                            cameraManager.PlayerTwoMoveSound();
                        }
                    }
                    if (Input.GetKey(KeyCode.D) && !LockMove)
                    {
                        PlayerPosition = gameObject.GetComponent<Transform>().position;
                        hit2 = Physics2D.Raycast(transform.position + Vector3.right / 2, Vector2.right, 0.7f);
                        if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
                        {
                            MoveControlLogic(MoveDir.D);
                            cameraManager.PlayerTwoMoveSound();
                        }
                            
                    }
                    if (Input.GetKey(KeyCode.W) && !LockMove)
                    {
                        PlayerPosition = gameObject.GetComponent<Transform>().position;
                        hit2 = Physics2D.Raycast(transform.position + Vector3.up / 2, Vector2.up, 0.7f);
                        if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
                        {
                            MoveControlLogic(MoveDir.W);
                            cameraManager.PlayerTwoMoveSound();
                        }
                    }
                    if (Input.GetKey(KeyCode.S) && !LockMove)
                    {
                        PlayerPosition = gameObject.GetComponent<Transform>().position;
                        hit2 = Physics2D.Raycast(transform.position + Vector3.down / 2, Vector2.down, 0.7f);
                        if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
                        {
                            MoveControlLogic(MoveDir.S);
                            cameraManager.PlayerTwoMoveSound();
                        }
                    }
                }
                if (LockMove)
                {
                    switch (moveDirEnum)
                    {
                        case MoveDir.W:
                            MoveW();
                            break;
                        case MoveDir.A:
                            MoveA();
                            break;
                        case MoveDir.S:
                            MoveS();
                            break;
                        case MoveDir.D:
                            MoveD();
                            break;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q)&&!isMoving)
                {
                    p1Playing.SwitchPlayer();
                    //gameObject.GetComponentInChildren<Light2D>().enabled = false;
                    panelManager.ChangePanelState(PanelState.Fade, PanelType.Right);
                }


            }
        }

    }

    private void MoveControlLogic(MoveDir mdType)
    {
        LockMove = true;
        moveDirEnum = mdType;
        animaor.SetBool("Running", true);
    }

    /// <summary>
    /// 移动角色的方法
    /// </summary>
    void Move()
    {
        PlayerPosition = gameObject.GetComponent<Transform>().position;
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                hit2 = Physics2D.Raycast(transform.position + Vector3.left / 2, Vector2.left, 0.7f);
                if (hit2.collider == null)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    PlayerPosition = PlayerPosition + new Vector2(-1, 0);
                    newPosition = PlayerPosition;
                    isMoving = true;
                    animaor.SetBool("Running", true);
                }



            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                hit2 = Physics2D.Raycast(transform.position + Vector3.right / 2, Vector2.right, 0.7f);
                if (hit2.collider == null)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    PlayerPosition = PlayerPosition + new Vector2(1, 0);
                    newPosition = PlayerPosition;
                    isMoving = true;
                    animaor.SetBool("Running", true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                hit2 = Physics2D.Raycast(transform.position + Vector3.up / 2, Vector2.up, 0.7f);
                if (hit2.collider == null)
                {
                    PlayerPosition = PlayerPosition + new Vector2(0, 1);
                    newPosition = PlayerPosition;
                    isMoving = true;
                    animaor.SetBool("Running", true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                hit2 = Physics2D.Raycast(transform.position + Vector3.down / 2, Vector2.down, 0.7f);
                if (hit2.collider == null)
                {
                    PlayerPosition = PlayerPosition + new Vector2(0, -1);
                    newPosition = PlayerPosition;
                    isMoving = true;
                    animaor.SetBool("Running", true);
                }
            }
        }

        if (isMoving)
        {
            canMove = false;
            S += Time.deltaTime / 10f;
            newPosition = new Vector3(newPosition.x, newPosition.y, PlayerzValue);//修正Z
            gameObject.GetComponent<Transform>().position = Vector3.Lerp(gameObject.GetComponent<Transform>().position, newPosition, S);
            if ((gameObject.GetComponent<Transform>().position - newPosition).magnitude <= 0.01)
            {
                gameObject.GetComponent<Transform>().position = newPosition;
                S = 0;
                isMoving = false;
                canMove = true;
                RemainStep--;
                animaor.SetBool("Running", false);

            }
        }


    }
    void MoveA()
    {
        if (canMove)
        {
            PlayerPosition = gameObject.GetComponent<Transform>().position;
            hit2 = Physics2D.Raycast(transform.position + Vector3.left / 2, Vector2.left, 0.7f);
            if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
            {
                isMoving = true;
                transform.localScale = new Vector3(1, 1, 1);
                PlayerPosition = PlayerPosition + new Vector2(-1, 0);
                newPosition = PlayerPosition;
            }
        }

        if (isMoving)
        {
            canMove = false;
            S += Time.deltaTime / 10f;
            newPosition = new Vector3(newPosition.x, newPosition.y, PlayerzValue);//修正Z
            gameObject.GetComponent<Transform>().position = Vector3.Lerp(gameObject.GetComponent<Transform>().position, newPosition, S);
            if ((gameObject.GetComponent<Transform>().position - newPosition).magnitude <= 0.01)
            {
                gameObject.GetComponent<Transform>().position = newPosition;
                S = 0;
                animaor.SetBool("Running", false);
                canMove = true;
                isMoving = false;
                LockMove = false;
                RemainStep--;

            }
        }

    }
    void MoveD()
    {
        if (canMove)
        {
            PlayerPosition = gameObject.GetComponent<Transform>().position;
            hit2 = Physics2D.Raycast(transform.position + Vector3.right / 2, Vector2.right, 0.7f);
            if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
            {
                isMoving = true;
                transform.localScale = new Vector3(-1, 1, 1);
                PlayerPosition = PlayerPosition + new Vector2(1, 0);
                newPosition = PlayerPosition;
            }
        }

        if (isMoving)
        {
            canMove = false;
            S += Time.deltaTime / 10f;
            newPosition = new Vector3(newPosition.x, newPosition.y, PlayerzValue);//修正Z
            gameObject.GetComponent<Transform>().position = Vector3.Lerp(gameObject.GetComponent<Transform>().position, newPosition, S);
            if ((gameObject.GetComponent<Transform>().position - newPosition).magnitude <= 0.01)
            {
                gameObject.GetComponent<Transform>().position = newPosition;
                S = 0;
                animaor.SetBool("Running", false);
                canMove = true;
                isMoving = false;
                LockMove = false;
                RemainStep--;

            }
        }

    }
    void MoveW()
    {
        if (canMove)
        {
            PlayerPosition = gameObject.GetComponent<Transform>().position;
            hit2 = Physics2D.Raycast(transform.position + Vector3.up / 2, Vector2.up, 0.7f);
            if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
            {
                isMoving = true;
                PlayerPosition = PlayerPosition + new Vector2(0, 1);
                newPosition = PlayerPosition;
            }
        }

        if (isMoving)
        {
            canMove = false;
            S += Time.deltaTime / MoveSpeed;
            newPosition = new Vector3(newPosition.x, newPosition.y, PlayerzValue);//修正Z
            gameObject.GetComponent<Transform>().position = Vector3.Lerp(gameObject.GetComponent<Transform>().position, newPosition, S);
            if ((gameObject.GetComponent<Transform>().position - newPosition).magnitude <= 0.01)
            {
                gameObject.GetComponent<Transform>().position = newPosition;
                S = 0;
                animaor.SetBool("Running", false);
                canMove = true;
                isMoving = false;
                LockMove = false;
                RemainStep--;

            }
        }

    }
    void MoveS()
    {
        if (canMove)
        {
            PlayerPosition = gameObject.GetComponent<Transform>().position;
            hit2 = Physics2D.Raycast(transform.position + Vector3.down / 2, Vector2.down, 0.7f);
            if (hit2.collider == null || hit2.collider.gameObject.layer == 8)
            {
                isMoving = true;
                PlayerPosition = PlayerPosition + new Vector2(0, -1);
                newPosition = PlayerPosition;
            }
        }

        if (isMoving)
        {
            canMove = false;
            S += Time.deltaTime / 10f;
            newPosition = new Vector3(newPosition.x, newPosition.y, PlayerzValue);//修正Z
            gameObject.GetComponent<Transform>().position = Vector3.Lerp(gameObject.GetComponent<Transform>().position, newPosition, S);
            if ((gameObject.GetComponent<Transform>().position - newPosition).magnitude <= 0.01)
            {
                gameObject.GetComponent<Transform>().position = newPosition;
                S = 0;
                animaor.SetBool("Running", false);
                canMove = true;
                isMoving = false;
                LockMove = false;
                RemainStep--;

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "fire")
        {
            RemainStep += 5;
            /*if (RemainStep > MaxStep)
            {
                RemainStep = MaxStep;          
            }            */
            Destroy(collision.gameObject);
            Instantiate(get, transform.position, Quaternion.identity);
            audio.PlayOneShot(clip);
        }
        if (collision.gameObject.tag == "hammer")
        {
            audio.PlayOneShot(clip2);
        }


    }
}