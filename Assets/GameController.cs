using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Camera gameCamera;
    public Camera uiCamera;

    public static GameController Instance = null;
    private void Awake()
    {
        Instance = this;
    }
    public Transform player;
    public Transform wayParent;
    public int wayID = 0;
    public int waycount = 2;
  //  public GameObject[] createObsorb;
  //  public Vector2 createObsorbIntervalRange = new Vector2(2.5f, 3f);
    //float createObsorbInterval;
   // public float createDistanceCur = 0;
    public MicrophoneVolumeReader microphoneVolumeReader;

    // Start is called before the first frame update
    void Start()
    {
        ResetWayList();
        startPlayerX = player.transform.position.x;
        SetWay();
        //三条路都初始化2个路
        //1.记录位置
        startX = enterX = jiweiqi.transform.position.x;
        float y1 = wayPos_1.position.y;
        float y2 = wayPos_2.position.y;
        float y3 = wayPos_3.position.y;
        
        //创建路1
        {
            for (int i = 0; i < waycount; i++)
            {
                ResetWayList();

                var way1 = Instantiate(GetWayPrefab());
                way1.transform.position = new Vector3(jiweiqi.position.x + wayLength * i, y1);
                way1List.Add(way1);

                var way2 = Instantiate(GetWayPrefab());
                way2.transform.position = new Vector3(jiweiqi.position.x + wayLength * i, y2);
                way2List.Add(way2);

                var way3 = Instantiate(GetWayPrefab());
                way3.transform.position = new Vector3(jiweiqi.position.x + wayLength * i, y3);
                way3List.Add(way3);

            }

        }
        ////创建路2
        //{
        //    for (int i = 0; i < waycount; i++)
        //    {
              
        //    }
        //}
        ////创建路3
        //{
        //    for (int i = 0; i < waycount; i++)
        //    {
            
        //    }
        //}
    }
    void SetWay()
    {
        float y = wayParent.GetChild(wayID).position.y;
        Vector3 pos = player.transform.position;
        pos.y = y;
        player.transform.position = pos;
    }
    public void UpWay()
    {
        wayID++;
        if (wayID > wayParent.childCount - 1)
        {
            wayID = 0;
        }
        SetWay();
    }
    public void DownWay()
    {
        wayID--;
        if (wayID < 0)
        {
            wayID = wayParent.childCount - 1;
        }
        SetWay();
    }
    public Transform jiweiqi;
    public float wayLength = 32.62f;
    float enterX;
    float startX;
    public float createWayDistance = 25f;
    public GameObject[] wayPrefabs;
    List<GameObject> wayPrefabList;
    public void ResetWayList()
    {
        wayPrefabList = new List<GameObject>(wayPrefabs);
        Debug.Log(" " + wayPrefabList.Count);
    }
   
    public GameObject GetWayPrefab()
    {
        int index = Random.Range(0, wayPrefabList.Count);
        Debug.Log(index+" " + wayPrefabList.Count);
        var way= wayPrefabList[index];
        wayPrefabList.RemoveAt(index);
        return way;
    }
    List<GameObject> way1List = new List<GameObject>();
    List<GameObject> way2List = new List<GameObject>();
    List<GameObject> way3List = new List<GameObject>();
    public Transform wayPos_1;
    public Transform wayPos_2;
    public Transform wayPos_3;
    //public Transform cameraMiddle;
    public float trackSpeed = 20;
    public float trackAddSpeed = 10;
    public Transform createjiaZhuPos;
    public Transform destoryJianZhuPos;
    List<GameObject> jianzhuList = new List<GameObject>();
    public float getMoveDisatance()
    {
        return jiweiqi.position.x - startX;
    }
    private void FixedUpdate()
    {
        //先做摄像机跟随玩家
        if (player.transform.position.x > gameCamera.transform.position.x)
        {
            float posX_Start = gameCamera.transform.position.x;
            float posX_End = player.transform.position.x;

            float moveDistance = (trackSpeed + trackAddSpeed * (posX_End - posX_Start)) * Time.deltaTime;
            float distance = posX_End - posX_Start;
            if (moveDistance < distance)
            {
                posX_Start += moveDistance;
            }
            else
            {
                posX_Start = posX_End;
            }
            Vector3 pos = gameCamera.transform.position;
            pos.x = posX_Start;
            gameCamera.transform.position = pos;

        }
        //路的业代
        if (jiweiqi.transform.position.x - enterX >= createWayDistance)
        {
            ResetWayList();
            //A路的迭代
            {
                var way = way1List[0];
                way1List.RemoveAt(0);
                Destroy(way);
            // way1List.Add(way);
            //way.transform.position = way1List[way1List.Count - 2].transform.position + Vector3.right * createWayDistance;
            var way1 = Instantiate(GetWayPrefab());
            way1.transform.position = way1List[way1List.Count - 1].transform.position + Vector3.right * createWayDistance;
            way1List.Add(way1);

            }
            //B路的迭代
            {
                var way = way2List[0];
                way2List.RemoveAt(0);
                Destroy(way);
                //way2List.Add(way);
                // way.transform.position = way2List[way2List.Count - 2].transform.position + Vector3.right * createWayDistance;
                var way1 = Instantiate(GetWayPrefab());
                way1.transform.position = way2List[way2List.Count - 1].transform.position + Vector3.right * createWayDistance;
                way2List.Add(way1);
            }
            //C路的迭代
            {
                var way = way3List[0];
                way3List.RemoveAt(0);
                Destroy(way);
                //  way3List.Add(way);
                //way.transform.position = way3List[way3List.Count - 2].transform.position + Vector3.right * createWayDistance;
                var way1 = Instantiate(GetWayPrefab());
                way1.transform.position = way3List[way3List.Count - 1].transform.position + Vector3.right * createWayDistance;
                way3List.Add(way1);
            }

            enterX += createWayDistance;
        }
        /*
        //删除建筑
        for (int i = 0; i < jianzhuList.Count; i++)
        {
            if (jianzhuList[i].transform.position.x < destoryJianZhuPos.position.x)
            {
                Destroy(jianzhuList[i].gameObject);

                jianzhuList.RemoveAt(i);
                i--;
            }
        }
        //建筑的业代

        while (createDistanceCur < createjiaZhuPos.transform.position.x - startX)
        {
            createObsorbInterval = Random.Range(createObsorbIntervalRange.x, createObsorbIntervalRange.y);
            createDistanceCur += createObsorbInterval;
            int type = Random.Range(0, 3);
            var jianzhu = createObsorb[Random.Range(0, createObsorb.Length)];
            //创建在三条路上-随机
            switch (type)
            {
                case 0:
                    {
                        var jz = Instantiate(jianzhu);
                        Vector3 pos = new Vector3();
                        pos.x = createDistanceCur;
                        pos.y = wayPos_1.position.y;
                        jz.transform.position = pos;
                        jianzhuList.Add(jz);
                    }
                    break;
                case 1:
                    {
                        var jz = Instantiate(jianzhu);
                        Vector3 pos = new Vector3();
                        pos.x = createDistanceCur;
                        pos.y = wayPos_2.position.y;
                        jz.transform.position = pos;
                        jianzhuList.Add(jz);
                    }
                    break;
                case 2:
                    {
                        var jz = Instantiate(jianzhu);
                        Vector3 pos = new Vector3();
                        pos.x = createDistanceCur;
                        pos.y = wayPos_3.position.y;
                        jz.transform.position = pos;
                        jianzhuList.Add(jz);
                    }
                    break;
                default:
                    break;
            }

        }
        */

    }
    private void LateUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public Text hpText;
    public void SetHP(float curHp, float maxHp)
    {
        hpText.text = curHp + "/" + maxHp;
    }
    bool isOver = false;
    public Text scoreText;
    public GameObject overShow;
    float startPlayerX;
    public void GameOver()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;
        int score = (int)(player.transform.position.x - startPlayerX);
        if (score>LocalData.maxScore)
        {
            LocalData.maxScore = score;
        }
        maxScoreText.text="" + LocalData.maxScore;
        scoreText.text = "" + score;
        StartCoroutine(GameOverIE());
    }
    IEnumerator GameOverIE()
    {
        yield return new WaitForSeconds(3.0f);
        overShow.SetActive(true);
    }
    public Text maxScoreText;
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void QuitGame()
    {
        // SceneManager.LoadScene("MenuScene");
        Application.Quit();
    }
}
