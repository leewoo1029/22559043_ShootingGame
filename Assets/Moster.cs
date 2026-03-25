using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moster : MonoBehaviour
{
    public float spd = 1.0f;
    public GameObject target;
    
    Vector3 direct = Vector3.down;


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);

        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        int rndNum = Random.Range(0, 10);
        if (rndNum % 3 == 0) 
        {
            direct = target.transform.position - transform.position;
            direct.Normalize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }
}
