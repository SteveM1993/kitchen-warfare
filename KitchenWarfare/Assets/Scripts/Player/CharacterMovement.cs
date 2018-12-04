using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    CharacterController charController;

    [Serializable]
    public class AnimationSettings
    {
        public string verticalVelocityFloat = "Forward";
        public string horizontalVelocityFloat = "Strafe";
        public string groundedBool = "isGrounded";
        public string jumpBool = "isJumping";
    }
    [SerializeField]
    public AnimationSettings animations;

    [Serializable]
    public class PhysicsSettings
    {
        public float gravityModifier = 9.81f;
        public float baseGravity = 50.0f;
        public float resetGravityValue = 1.2f;
        public LayerMask groundLayers;
    }
    [SerializeField]
    public PhysicsSettings physics;

    [Serializable]
    public class MovementSettings
    {
        public float jumpSpeed = 6;
        public float jumpTime = 0.25f;
    }
    [SerializeField]
    public MovementSettings movement;

    private bool isJumping;    
    private bool resetGravity;
    private float gravity;

    private bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 start = transform.position + transform.up;
        Vector3 dir = Vector3.down;
        float radius = charController.radius;
        if (Physics.SphereCast(start, radius, dir, out hit, charController.height/2, physics.groundLayers))
        {
            return true;
        }

        return false;
    }

    //Pre-load init
    private void Awake()
    {
        animator = GetComponent<Animator>();
        SetupAnimator();
    }

    //Use this for initilization
    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    //Update is called once per frame
    private void Update()
    {
        ApplyGravity();
        //isGrounded = charController.isGrounded;
    }

    //Animate character
    public void Animate(float forward, float strafe)
    {
        animator.SetFloat(animations.verticalVelocityFloat, forward);
        animator.SetFloat(animations.horizontalVelocityFloat, strafe);
        animator.SetBool(animations.groundedBool, IsGrounded());
        animator.SetBool(animations.jumpBool, isJumping);
    }

    //Jumping controller
    public void Jump()
    {
        if (isJumping)
        {
            return;
        }

        if (IsGrounded())
        {
            isJumping = true;
            StartCoroutine(StopJump());
        }
    }

    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(movement.jumpTime);
        isJumping = false;
    }

    private void ApplyGravity()
    {
        if (!charController.isGrounded)
        {
            if (!resetGravity)
            {
                gravity = physics.resetGravityValue;
                resetGravity = true;
            }

            gravity += Time.deltaTime * physics.gravityModifier;
        }
        else
        {
            gravity = physics.baseGravity;
            resetGravity = false;
        }

        Vector3 gravVector = new Vector3();
        
        if (!isJumping)
        {
            gravVector.y -= gravity;
        }
        else
        {
            gravVector.y = movement.jumpSpeed;
        }

        charController.Move(gravVector * Time.deltaTime);
    }

    //Sets up the animator with the child avatar
    private void SetupAnimator()
    {
        Animator wantedAnim = GetComponentsInChildren<Animator>()[1];
        Avatar wantedAvatar = wantedAnim.avatar;

        animator.avatar = wantedAvatar;
        Destroy(wantedAnim);
    }
}
