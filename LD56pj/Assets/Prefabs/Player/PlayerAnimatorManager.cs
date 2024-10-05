using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEditor;
using UnityEngine;

public class PlayerAnimatorManager : MonoSingleton<PlayerAnimatorManager>
{
    public Animator playerAnimator;
    /*public float moveSpeed;
    public float crouchSpeed;//Speed when crouching
    public float runSpeed; //If speed is greater than runSpeed ,switch to tun;
    public float dashTime;*/
    
    public void ChangeCrouchState(bool state)
    {
        playerAnimator.SetBool("Crouch",state);
    }
    public void ChangeLiftState(bool state)
    {
        playerAnimator.SetBool("Lift",state);
    }
    public void ChangeJumpState(bool state)
    {
        playerAnimator.SetBool("Jump",state);
    }
    public void SwitchToDash() //由于是一次性的动作，所以不需要切换状态
    {
        playerAnimator.CrossFade("Dash",0);
    }
}
