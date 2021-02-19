using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapon;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public WeaponManger weaponManager;
    public PlayerUI playerUI;
    public Camera camera;
    public float walkingSpeed = 8f;
    public float runningSpeed = 15f;
    public float crouchSpeed = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float crouchHeight = 2f;

    private float speed = 0;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isWalking = true;
    private bool isCrouching = false;

    private float extraJump = 1.0f;
    private float forward = 0f;
    private float side = 0f;

    public delegate void SetBool(bool active);
    public delegate void WeaponPosition(string pos, bool state);
    public event SetBool Headbobing;
    public event WeaponPosition WeaponPositionEvent;


    // Start is called before the first frame update
    void Start() {
        speed = walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded();
        if (isGrounded)
        {
            //WeaponPositionEvent(GunAnimation.Hide, false);
            Headbobing(true);
        }
        else
        {
            //WeaponPositionEvent(GunAnimation.Hide, true);
            Headbobing(false);
        }

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        HandleCrouch();

        if (isGrounded && !isCrouching)
            controller.height = 3.8f;

        HandleJump();

        side = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");

        // avoids moving to the sides while jumping
        if (!isGrounded && !isCrouching)
            side = 0;

        // if an object blocks the weapon
        bool isWeaponBlocked = Physics.Raycast(transform.position, Vector3.forward, 1f);
        
        // vector of the move direction
        Vector3 moveDirection = Vector3.Normalize(transform.right * side + transform.forward * forward);

        if (true)//!isWeaponBlocked)
        {
            HandleRun();
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        // sets gravity on y axis
        velocity.y += gravity * Time.deltaTime;
        // applies garvity
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))//&& isGrounded)
        {
            if (isGrounded || (isGrounded && extraJump >= 3f))
                extraJump = 1.0f;
            else
                extraJump++;

            if (extraJump < 3f)
            {
                if (extraJump > 1f)
                    playerUI.EffectEnergy(-10);
                // required velocity for jump is root(h*-2*g)
                velocity.y = Mathf.Sqrt(jumpHeight * extraJump * -2f * gravity);
                //controller.height = 2.5f;
            }
        }
    }

    private void HandleRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !weaponManager.Weapon.Reloading)
        {
            if (forward > 0)
            {
                if (isGrounded)
                    WeaponPositionEvent(WeaponAnimations.Run, true);
                else
                    WeaponPositionEvent(WeaponAnimations.Run, false);
                speed = runningSpeed;
                isWalking = false;
            }
        }
        else
        {
            WeaponPositionEvent(WeaponAnimations.Run, false);
            isWalking = true;
            if (!isCrouching)
                speed = walkingSpeed;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            // checks if player is blocked by objects from above
            bool blockedFromTop = Physics.Raycast(transform.position, Vector3.up, 2f);
            // exits the method if player is blocked and already crouching, won't be able to stand up back anyway
            if (blockedFromTop && isCrouching)
                return;

            isCrouching = !isCrouching;
            // scale of the y axis while crouching/standing
            float crouchScale = 0;
            float verticalPosFix = 0;
            
            if (isCrouching)
            {
                controller.height = crouchHeight;
                // decreases player's size
                crouchScale = transform.localScale.y / 1.5f;
                verticalPosFix = crouchHeight;
                speed = crouchSpeed;
                
            }
            else
            {
                controller.height = 3.8f;
                verticalPosFix = -crouchHeight;
                // increases back player's size
                crouchScale = transform.localScale.y * 1.5f;
            }

            // set size of player
            //transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            // fixing position
            transform.position = new Vector3(transform.position.x, transform.position.y + crouchHeight, transform.position.z);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, controller.height/2 + 0.4f);
    }

    public bool OnGround
    {
        get { return isGrounded; }
    }

    public bool CanFire
    {
        get { return isWalking || isCrouching; }
    }

    public bool IsWalking
    {
        get { return isWalking; }
    }
}
