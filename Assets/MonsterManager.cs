using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject prefabsMoster;

    float nowTime;
    float minTime = 1f;
    float maxTime = 5f;
    public float createTime = 1f;

    public float minX = -1f;  // 추가
    public float maxX = 1f;   // 추가

    // Start is called before the first frame update
    void Start()
    {
      createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        nowTime = nowTime + Time.deltaTime;
        if (nowTime > createTime)
        {
            GameObject monster = Instantiate(prefabsMoster);

            float randomX = Random.Range(minX, maxX);

            // X만 랜덤, Y/Z는 현재 위치 유지
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, transform.position.z);
            monster.transform.position = spawnPos;

            createTime = Random.Range(minTime, maxTime);
            nowTime = 0;
        }
    } 
}
