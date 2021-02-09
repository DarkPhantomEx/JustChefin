using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CollectibleHandler : MonoBehaviour
{
    public GameObject collectPrefab;
    private GameObject collectible;
    public float waitTime;
    public int destroyTime;
    private float currTime = 0;

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        Vector3 collectPrefabPos = new Vector3(Random.Range(-8.0f , 8.0f), 5.0f, Random.Range(-8.0f, 8.0f));
        if (currTime >= waitTime)
        {
            collectible = Instantiate(collectPrefab, collectPrefabPos, transform.rotation) as GameObject;
            currTime = 0;
        }
        Destroy(collectible, destroyTime);
    }
}
