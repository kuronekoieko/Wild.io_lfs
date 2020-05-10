using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;
using UnityEngine.AI;

public class PlayerController : BaseCharactorController
{
    public enum PlayerType
    {
        Player,
        Enemy,
    }
    [SerializeField] TextMesh infoText;
    [SerializeField] ParticleSystem sizeUpPS;
    [SerializeField] TextMesh sizeUpText;
    [SerializeField] SpriteRenderer infoBGSprite;
    [SerializeField] Animator animator;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    int playerIndex;
    Rigidbody rb;
    float walkSpeed = 30f;
    Vector3 walkVec;
    int maxSize;
    PlayerType type;
    Vector3 mouseDownPos;
    NavMeshAgent agent;
    BaseCharactorController closestCharactor;

    public override void OnStart()
    {
        base.size = 0;
        this.ObserveEveryValueChanged(count => Variables.playerProperties[playerIndex].eatenCount)
            .Subscribe(count => CheckSizeUp(count))
            .AddTo(this.gameObject);
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        transform.localScale = Vector3.one;
        maxSize = Variables.playerSizes.Last().size;
        sizeUpText.gameObject.SetActive(playerIndex == 0);
        skinnedMeshRenderer.material.color = PlayerSettingSO.i.playerSettings[playerIndex].color;
        type = (playerIndex == 0) ? PlayerType.Player : PlayerType.Enemy;
        switch (type)
        {
            case PlayerType.Player:
                break;
            case PlayerType.Enemy:
                walkVec = Vector3.forward;
                animator.SetTrigger("Run");
                break;
        }
        infoText.transform.LookAt(Camera.main.transform.position);
        infoBGSprite.transform.LookAt(Camera.main.transform.position);
        sizeUpText.gameObject.SetActive(false);
        base.OnStart();

        agent.stoppingDistance = 0;
        agent.angularSpeed = 1000;
        agent.acceleration = 100;
        agent.speed = 50;
        agent.enabled = (playerIndex != 0);
    }

    public void SetParam(int playerIndex)
    {
        this.playerIndex = playerIndex;
        name = Variables.playerProperties[playerIndex].name;
        infoText.text = name;
    }

    public override void OnUpdate()
    {
        if (playerIndex == 0 && base.charactorState == CharactorState.Dead)
        {
            Variables.screenState = ScreenState.Result;
            Variables.isKilled = true;
        }
    }

    void FixedUpdate()
    {

        if (Variables.screenState == ScreenState.Game)
        {
            switch (type)
            {
                case PlayerType.Player:
                    Controller();
                    SetVelocityFromWalkVec();
                    break;
                case PlayerType.Enemy:

                    if (closestCharactor)
                    {
                        agent.SetDestination(closestCharactor.transform.position);
                    }
                    else
                    {
                        //SetVelocityFromWalkVec();
                        closestCharactor = GameManager.i.GetClosestTarget(transform.position, base.size);
                    }
                    break;
            }


        }

        infoText.transform.LookAt(Camera.main.transform.position);
        sizeUpText.transform.LookAt(Camera.main.transform.position);
        infoBGSprite.transform.LookAt(Camera.main.transform.position);
        base.OnUpdate();
    }




    void OnCollisionEnter(Collision col)
    {

        switch (type)
        {
            case PlayerType.Player:

                break;
            case PlayerType.Enemy:
                if (closestCharactor)
                {
                    if (col.gameObject == closestCharactor.gameObject)
                    {
                        closestCharactor = null;
                        // Debug.Log("aaaaaaaaaaa");
                    }
                }

                OnCollisionWall(col);
                break;
        }

        OnCollisionCharactor(col);
    }


    public override void Killed()
    {
        base.Killed();
        animator.SetTrigger("Dead");

    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    void SetVelocityFromWalkVec()
    {
        float degree = Vector2ToDegree(new Vector2(walkVec.z, walkVec.x));
        transform.eulerAngles = new Vector3(0, degree, 0);
        Vector3 vel = walkVec.normalized * walkSpeed;
        //落下しなくなるため、上に飛ばないようにする
        if (rb.velocity.y < 0) vel.y = rb.velocity.y;
        rb.velocity = vel;
    }


    void OnCollisionWall(Collision col)
    {
        if (col.transform.CompareTag("Ground")) { return; }

        Vector3 normal = col.contacts[0].normal;
        normal.y = 0;
        Vector3 reflectVec = Vector3.Reflect(walkVec, normal);
        walkVec = reflectVec;


    }

    void OnCollisionCharactor(Collision col)
    {
        var colCharactor = col.gameObject.GetComponent<BaseCharactorController>();
        if (colCharactor == null) { return; }
        //おなじだと両方消えるので
        if (colCharactor.size >= base.size) { return; }
        //死亡アニメーション中に処理させない
        if (base.charactorState != CharactorState.Alive) { return; }


        int rate = (playerIndex == 0) ? 1 : Random.Range(1, 3);

        Variables.playerProperties[playerIndex].eatenCount += colCharactor.eatenCount * rate;
        colCharactor.Killed();
        animator.SetTrigger("Attack");
        var charactorRB = colCharactor.GetComponent<Rigidbody>();
        if (charactorRB == null) { return; }
        charactorRB.isKinematic = true;
    }

    void Controller()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            animator.SetTrigger("Run");
        }

        if (Input.GetMouseButton(0))
        {
            SetWalkVec();
        }
    }

    void SetWalkVec()
    {

        Vector2 mouseVec = Input.mousePosition - mouseDownPos;

        //タップで止まる対策
        if (mouseVec.sqrMagnitude < 1.0f) { return; }
        walkVec.x = mouseVec.x;
        walkVec.z = mouseVec.y;

    }

    void CheckSizeUp(int eatenCount)
    {
        if (base.size > maxSize) { return; }
        int eatenCountToNextSize = Variables.playerSizes[base.size].eatenCountToNextSize;
        if (eatenCount < eatenCountToNextSize) { return; }

        size++;
        transform.localScale += Vector3.one;
        walkSpeed += 10;
        sizeUpPS.Play();
        sizeUpTextAnim();
        //        Debug.Log(name + " " + size);
    }

    void sizeUpTextAnim()
    {
        sizeUpText.gameObject.SetActive(playerIndex == 0);
        sizeUpText.transform.localScale = Vector3.zero;
        Color c = sizeUpText.color;
        Sequence sequence = DOTween.Sequence()
        .Append(sizeUpText.transform.DOScale(new Vector3(-1, 1, 1), 1).SetEase(Ease.OutElastic))
        .Append(DOTween.ToAlpha(() => sizeUpText.color, color => sizeUpText.color = color, 0f, 1f))
        .OnComplete(() =>
        {
            sizeUpText.gameObject.SetActive(false);
            sizeUpText.color = c;
        });
    }

    public static float Vector2ToDegree(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
}
