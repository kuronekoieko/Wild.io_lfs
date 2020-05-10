using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvasManager : BaseCanvasManager
{
    [SerializeField] Button openButton;
    [SerializeField] Button hideButton;
    [SerializeField] Image bannerImage;
    [SerializeField] Image debugPanel;
    [SerializeField] Button applyButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button resultButton;

    public override void OnStart()
    {
        gameObject.SetActive(Debug.isDebugBuild);
        debugPanel.gameObject.SetActive(false);
        openButton.onClick.AddListener(OnClickOpenButton);
        hideButton.onClick.AddListener(OnClickHideButton);
        applyButton.onClick.AddListener(OnClickApplyButton);
        cancelButton.onClick.AddListener(OnClickCancelButton);
        resultButton.onClick.AddListener(() => { Variables.screenState = ScreenState.Result; });
    }

    void OnClickOpenButton()
    {
        debugPanel.gameObject.SetActive(true);
        ShowParam();
    }

    /// <summary>
    /// inputfieldとかtoggleにデータを入れる
    /// </summary>
    void ShowParam()
    {

    }

    void OnClickHideButton()
    {
        bool activeSelf = bannerImage.gameObject.activeSelf;
        bannerImage.gameObject.SetActive(!activeSelf);
        hideButton.GetComponent<CanvasGroup>().alpha = activeSelf ? 0 : 1;
    }

    void OnClickApplyButton()
    {


        //適用時に好きな画面に遷移するように書き換えてOK
        Variables.screenState = ScreenState.Start;
        Close();
    }

    void OnClickCancelButton()
    {
        Close();
    }

    void Close()
    {
        debugPanel.gameObject.SetActive(false);
    }
}