using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedManager : MonoBehaviour
{
    [SerializeField] Transform fieldCornerUR;
    [SerializeField] Transform fieldCornerLL;
    [SerializeField] FeedController feedAnimalPrefab;
    FeedController[] feedControllers;

    public void OnStart()
    {
        FeedGenerator();
    }

    public void OnUpdate()
    {
        for (int i = 0; i < feedControllers.Length; i++)
        {
            feedControllers[i].OnUpdate();
        }
    }

    void FeedGenerator()
    {
        feedControllers = new FeedController[100];
        for (int i = 0; i < feedControllers.Length; i++)
        {
            Vector3 pos = GetRandomPos();
            feedControllers[i] = Instantiate(feedAnimalPrefab, pos, Quaternion.identity, transform);
            feedControllers[i].OnStart();
        }
    }

    public Vector3 GetRandomPos()
    {
        //座標をランダムに取得
        float x = Random.Range(fieldCornerLL.position.x, fieldCornerUR.position.x);
        float z = Random.Range(fieldCornerLL.position.z, fieldCornerUR.position.z);
        return new Vector3(x, 0, z);
    }
}
