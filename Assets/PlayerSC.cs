using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSC : MonoBehaviour
{
    public int curHP = 5;
    public int maxHP=5;

    bool isDie = false;
    public AudioSource wow;
    public AudioSource hurt;
  //  public GameObject biaoxue;
    public void TakeDamage()
    {
        if (isDie)
        {
            return;
        }
        //   curHP--;
        if (lastBloodCor!=null)
        {
            StopCoroutine(lastBloodCor);
        }
        lastBloodCor = StartCoroutine(createBloodIE());
        hurt.Play();
        if (curHP<=0)
        {
            //biaoxue.SetActive(true);
            curHP = 0;
            isDie = true;
            GameController.Instance.GameOver();
        }
        GameController.Instance.SetHP(curHP, maxHP);
    }
    Coroutine lastBloodCor;
    
    Rigidbody2D rig;
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.SetHP(curHP, maxHP);
    }
    bool isStart = false;
    public float minStartVolume = 0.5f;
    public float downEndVolume = 0.2f;
    public float upVolume = 8f;
    public float moveForce = 4;
    public float moveSpeed = 10;
    public Animator anim;
    public GameObject upShow;
    public GameObject downShow;
    public Transform bloodPos;
    public GameObject biaoxuePrefab;
    public float createBiaoXueInterval = 0.05f;
    float timer = 3;
    public float createBloodTime = 5;
    IEnumerator createBloodIE()
    {
        createBloodTime = timer;
        while (createBloodTime > 0)
        {
            createBloodTime -= createBiaoXueInterval;
            yield return new WaitForSeconds(createBiaoXueInterval);
            var biaoxue=Instantiate(biaoxuePrefab);
            biaoxue .transform.position= bloodPos.transform.position;
        }
       

    }
    int colCount = 0;//玩家的碰撞数量
    bool isStartPengZhuang = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colCount++;
        isStartPengZhuang = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        colCount--;
        if (colCount==0)
        {
            if (isStartPengZhuang)
            {
                wow.Play();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        float value = GameController.Instance.microphoneVolumeReader.GetMicrophoneVolume();
        if (isDie&&isStart)
        {/*
            rig.velocity = Vector2.right * moveSpeed * value;
            if (Input.GetKeyDown(KeyCode.W))
            {
                isStartPengZhuang = false;
                GameController.Instance.DownWay();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                isStartPengZhuang = false;
                GameController.Instance.UpWay();
            }
            */
        }
        if (isDie)
        {
            return;
        }
        //检测音量输入
        if (value > minStartVolume)
        {
            isStart = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            isStartPengZhuang = false;
            GameController.Instance.DownWay();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            isStartPengZhuang = false;
            GameController.Instance.UpWay();
        }
        if (isStart)
        {
            
            anim.SetBool("isMove",value>upVolume);
            downShow.gameObject.SetActive(false);
            upShow.gameObject.SetActive(false);
            if (value > upVolume)
            {
                downShow.gameObject.SetActive(true);
            }
            else
            {
                upShow.gameObject.SetActive(true);
            }
            if (value > downEndVolume)
            {

                rig.velocity = Vector2.right * moveSpeed* value;
            }
            else
            {
                rig.velocity = Vector3.one;
                isDie = true;
                GameController.Instance.GameOver();
            }
        }
    }
}
