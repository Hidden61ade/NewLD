using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEditor;
using UnityEngine;

// SpeedMode: 0:Idle, 1:Walk, 2 Run;

public class PlayerAnimatorManager : MonoSingleton<PlayerAnimatorManager>
{
    [Header("可自动获取")]
    public Animator playerAnimator;
    /*public float moveSpeed;
    public float crouchSpeed;//Speed when crouching
    public float runSpeed; //If speed is greater than runSpeed ,switch to tun;
    public float dashTime;*/

    private void Awake()
    {
        if (playerAnimator==null)
        {
            playerAnimator=gameObject.GetComponent<Animator>();
            if (playerAnimator==null)
            {
                Debug.Log("没有找到animator,请确保挂载了animator");
            }
        }
    }

    public void PlayFootStepSound()
    {
        GameAudio.AudioManager.Instance.PlayFootsteps();
    }

    public void SwitchToIdle()
    {
        playerAnimator.SetInteger("SpeedModeInt",0);
    }
    public void SwitchToWalk()
    {
        playerAnimator.SetInteger("SpeedModeInt",1);
    }
    public void SwitchToRun()
    {
        playerAnimator.SetInteger("SpeedModeInt",2);
    }
    public void SwitchToDash() //由于是一次性的动作，所以不需要切换状态
    {
        playerAnimator.CrossFade("Dash",0);
    }

    public void SwitchToDie()
    {
        playerAnimator.SetTrigger("Die");
    }
    public void ChangeCrouchState(bool state)
    {
        playerAnimator.SetBool("Crouch",state);
    }
    public void ChangeLiftState(bool state)
    {
        playerAnimator.SetBool("Lift",state);
    }
    public void ChangePushState(bool state)
    {
        playerAnimator.SetBool("Push",state);
    }
    public void ChangeJumpState(bool state)
    {
        playerAnimator.SetBool("Jump",state);
    }
    public void ChangePickState(bool state)
    {
        playerAnimator.SetBool("Pick",state);
    }
}
