using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] Transform playerPossitionsParent;
    public PlayerController[] playerControllers { get; set; }
    Transform[] playerPoints;

    void Awake()
    {
        Variables.playerCount = playerPossitionsParent.childCount;
        Variables.playerProperties = new PlayerProperty[Variables.playerCount];
        for (int i = 0; i < Variables.playerProperties.Length; i++)
        {
            Variables.playerProperties[i] = new PlayerProperty();
            Variables.playerProperties[i].name = PlayerSettingSO.i.playerSettings[i].name;
            Variables.playerProperties[i].playerIndex = i;
        }

        playerPoints = new Transform[Variables.playerCount];
        for (int i = 0; i < playerPoints.Length; i++)
        {
            playerPoints[i] = playerPossitionsParent.GetChild(i);
        }
    }

    public void OnStart()
    {
        playerControllers = new PlayerController[Variables.playerCount];
        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i] = Instantiate(
                playerPrefab,
                GetRandomPlayerPos(),
                Quaternion.identity,
                transform);



            playerControllers[i].SetParam(playerIndex: i);
            playerControllers[i].OnStart();
        }
    }

    public void Stop()
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i].Stop();
        }
    }

    public void OnUpdate()
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i].OnUpdate();
        }
    }

    public Vector3 GetRandomPlayerPos()
    {
        int randomInt = Random.Range(0, playerPoints.Length);
        Vector3 pos = playerPoints[randomInt].position;
        playerPoints[randomInt].gameObject.SetActive(false);
        playerPoints = playerPoints.Where(p => p.gameObject.activeSelf).ToArray();

        return pos;
    }
}
