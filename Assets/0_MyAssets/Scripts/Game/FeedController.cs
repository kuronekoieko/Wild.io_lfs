using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedController : BaseCharactorController
{
    [SerializeField] Animator animator;
    Rigidbody rb;
    float timer;
    Vector3 walkVec;
    float walkSpeed = 10f;
    float timeLimit;
    public override void OnStart()
    {
        base.size = -1;
        base.OnStart();
        rb = GetComponent<Rigidbody>();
        int x = Random.Range(-10, 10);
        int z = Random.Range(-10, 10);
        walkVec = new Vector3(x, 0, z);
        timeLimit = Random.Range(1, 6);
    }


    public override void OnUpdate()
    {
        if (base.charactorState == CharactorState.Dead)
        {
            rb.isKinematic = false;
            base.OnAlive();
        }
        base.OnUpdate();

        timer += Time.deltaTime;
        if (timer > timeLimit)
        {
            timer = 0;
            timeLimit = Random.Range(1, 6);
            int x = Random.Range(-10, 10);
            int z = Random.Range(-10, 10);
            walkVec = new Vector3(x, 0, z);
        }
        float degree = Vector2ToDegree(new Vector2(walkVec.z, walkVec.x));
        transform.eulerAngles = new Vector3(0, degree, 0);
        Vector3 vel = walkVec.normalized * walkSpeed;
        //落下しなくなるため、上に飛ばないようにする
        if (rb.velocity.y < 0) vel.y = rb.velocity.y;
        rb.velocity = vel;
    }

    public override void Killed()
    {
        base.Killed();
        animator.SetTrigger("Dead");
    }

    public static float Vector2ToDegree(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
}
