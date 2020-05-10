using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ゲーム画面
/// ゲーム中に表示するUIです
/// あくまで例として実装してあります
/// ボタンなどは適宜編集してください
/// </summary>
public class GameCanvasManager : BaseCanvasManager
{
    [SerializeField] Text timerText;
    [SerializeField] Text eatenCountText;
    [SerializeField] RectTransform tutrials;
    [SerializeField] RectTransform fingerPoint;
    public readonly ScreenState thisScreen = ScreenState.Game;
    float timer;
    float angularVelocity = 7;

    public override void OnStart()
    {

        base.SetScreenAction(thisScreen: thisScreen);
        this.ObserveEveryValueChanged(timer => Variables.timer)
            .Subscribe(_ => { SetTimeCountText(); })
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(count => Variables.playerProperties[0].eatenCount)
            .Subscribe(count => eatenCountText.text = "★ " + count)
            .AddTo(this.gameObject);

        gameObject.SetActive(true);
        tutrials.gameObject.SetActive(false);
    }


    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tutrials.gameObject.SetActive(false);
        }

        FingerAnim();
    }

    void FingerAnim()
    {
        timer += Time.deltaTime;
        float y = -330f + 70f * Mathf.Sin(timer * angularVelocity);
        float x = 170 * Mathf.Sin(timer * angularVelocity / 2);
        fingerPoint.anchoredPosition = new Vector3(x, y, 0);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        tutrials.gameObject.SetActive(true);
    }

    protected override void OnClose()
    {
        // gameObject.SetActive(false);
    }

    void SetTimeCountText()
    {
        timerText.text = Variables.timer.ToString("F2");
    }

}
