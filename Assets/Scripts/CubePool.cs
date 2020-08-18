using System.Collections.Generic;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    public static CubePool shared_instance;
    List<GameObject> pooled_objects;
    public GameObject object_to_pool;
    public int amount_to_pool;

    private void Awake()
    {
        shared_instance = this;
        pooled_objects = new List<GameObject>();
        for(int i = 0; i < amount_to_pool; i++)
        {
            GameObject obj = Instantiate(object_to_pool);
            obj.SetActive(false);
            pooled_objects.Add(obj);
        }
    }

    public GameObject get_pooled_object()
    {
        for(int i = 0; i < pooled_objects.Count; i++)
        {
            if (!pooled_objects[i].activeInHierarchy)
            {
                return pooled_objects[i];
            }
        }
        return null;
    }
}
