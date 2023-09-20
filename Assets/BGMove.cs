using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    public List<Transform> bgChildList=new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bgChildList.Add( transform.GetChild(i));
        }
    }
    float curDistance = 0;
    public float imageDistance = 0;
    // Update is called once per frame
    void Update()
    {
        while (GameController.Instance.getMoveDisatance()> curDistance+ imageDistance)
        {
            curDistance += imageDistance;
            var child=bgChildList[0];
            child.transform.position= bgChildList[bgChildList.Count - 1].position + Vector3.right * imageDistance;
            bgChildList.RemoveAt(0);
            bgChildList.Add(child);
        }
    }
}
