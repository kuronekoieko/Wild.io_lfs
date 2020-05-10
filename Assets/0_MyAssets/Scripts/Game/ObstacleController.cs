using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleController : BaseCharactorController
{

    void Start()
    {
        base.OnStart();
    }


    void Update()
    {
        base.OnUpdate();
    }

    public override void Killed()
    {
        base.Killed();
        base.hideObject.transform.DOMoveY(10 * eatenCount * 1.5f, 1).SetRelative();
        base.hideObject.transform.DOLocalRotate(new Vector3(180, 0, 180), 1).SetRelative().SetLoops(-1).SetEase(Ease.Linear);
    }
}
