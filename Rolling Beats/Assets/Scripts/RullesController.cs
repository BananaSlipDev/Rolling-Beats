using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RullesController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void IdleSprite()
    {
        

    }

    public void DownSprite()
    {
        animator.Play("JumpBottom");
    }

    public void JumpSprite()
    {
        animator.Play("JumpTop");
    }
}
