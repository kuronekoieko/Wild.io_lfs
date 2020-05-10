using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

/// <summary>
/// Unityで解像度に合わせて画面のサイズを自動調整する
/// http://www.project-unknown.jp/entry/2017/01/05/212837
/// </summary>
public class CameraController : MonoBehaviour
{
    Vector3 vecFromPlayerToCamera = new Vector3(0, 40, -40);
    float nFocalLength;
    int maxSize;

    public void OnStart(PlayerController player)
    {
        this.ObserveEveryValueChanged(size => player.size)
            .Subscribe(size => CheckSizeUp(size))
            .AddTo(this.gameObject);

        maxSize = Variables.playerSizes.Last().size;
        float aperture = Variables.playerSizes.First().cameraAperture;
        nFocalLength = focalLength(Camera.main.fieldOfView, aperture);

        transform.position = player.transform.position + vecFromPlayerToCamera.normalized * nFocalLength;
        SCCameraCoverTransparent s = GetComponent<SCCameraCoverTransparent>();
        s.subject = player.transform;
    }

    void CheckSizeUp(int size)
    {
        if (size > maxSize) { return; }
        float aperture = Variables.playerSizes[size].cameraAperture;
        float f = focalLength(Camera.main.fieldOfView, aperture);
        DOTween.To(() => nFocalLength, (x) => nFocalLength = x, f, 0.5f);
    }


    public void FollowTarget(Vector3 playerPos)
    {
        transform.position = playerPos + vecFromPlayerToCamera.normalized * nFocalLength;
    }


    /*! 
     @brief 焦点距離(FocalLength)を求める
     @param[in]		fov			視野角(FieldOfView)
     @param[in]		aperture	画面幅いっぱいに表示したいオブジェクトの幅
     @return        焦点距離(FocalLength)
    */
    float focalLength(float fov, float aperture)
    {
        // FieldOfViewを2で割り、三角関数用にラジアンに変換しておく
        float nHalfTheFOV = fov / 2.0f * Mathf.Deg2Rad;

        // FocalLengthを求める
        float nFocalLength = (0.5f / (Mathf.Tan(nHalfTheFOV) / aperture));

        // Unityちゃんは画面高さ(Vertical)なFOVなので画面アスペクト比(縦/横)を掛けとく
        nFocalLength *= ((float)Screen.height / (float)Screen.width);

        return nFocalLength;
    }
}
