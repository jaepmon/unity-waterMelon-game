using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] GameObject goPrefab = null;

    [SerializeField] Transform pool = null;

    [SerializeField] int poolSize;

    public Queue<GameObject> mint = new Queue<GameObject>();
    void Awake()
    {
        instance = this;
        
        mint = InsertQueue();
    }
    Queue<GameObject> InsertQueue()
    {
        Queue<GameObject> tempQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject clone = Instantiate(goPrefab, transform.position, Quaternion.identity);

            clone.SetActive(false);
            
            if(pool != null)
            {
                clone.transform.SetParent(pool);
            }
            else
            {
                clone.transform.SetParent(this.transform);
            }

            tempQueue.Enqueue(clone);
        }
        return tempQueue;
    }
}
