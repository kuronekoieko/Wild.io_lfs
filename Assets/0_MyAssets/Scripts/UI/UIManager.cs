using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 画面UIの一括管理
/// GameDirectorと各画面を中継する役割
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform canvasesPatent;
    BaseCanvasManager[] canvases;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RuntimeInitializeApplication()
    {
        //ここが呼ばれてからStart()が呼ばれる
        SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        QualitySettings.vSyncCount = 0; // VSyncをOFFにする
        Application.targetFrameRate = 30; // ターゲットフレームレートを60に設定
    }


    void Start()
    {
        canvases = new BaseCanvasManager[canvasesPatent.childCount];
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i] = canvasesPatent.GetChild(i).GetComponent<BaseCanvasManager>();
            if (canvases[i] == null) { continue; }
            canvases[i].OnStart();
        }
        Variables.screenState = ScreenState.Start;
        GoogleAnalyticsManager.i.OnStart();
    }

    void Update()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] == null) { continue; }
            canvases[i].OnUpdate();
        }

    }
}
