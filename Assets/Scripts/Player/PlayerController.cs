using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mirror
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        public enum GroundState : byte { Jumping, Falling, Grounded }

        [Header("Avatar Object")]
        public GameObject Body;

        [Header("Avatar Components")]
        public CharacterController characterController;
        public PlayerAnimatorController playerAnimatorController;
        public PlayerAttackController playerAttackController;
        public PlayerStatController playerStatController;
        public PlayerNetwork playerNetwork;
        public PlayerCamera playerCamera;

        [Header("Avatar Sound Components")]
        public AudioSource TryToShootSound;
        public AudioSource ShootSound;
        public AudioSource FootStep;

        [Header("Movement")]
        [Range(1, 20)]
        public float moveSpeedMultiplier = 8f;

        [Header("Turning")]
        [Range(1f, 200f)]
        public float maxTurnSpeed = 100f;
        [Range(.5f, 5f)]
        public float turnDelta = 3f;

        [Header("Jumping")]
        [Range(0.1f, 1f)]
        public float initialJumpSpeed = 0.2f;
        [Range(1f, 10f)]
        public float maxJumpSpeed = 5f;
        [Range(0.1f, 1f)]
        public float jumpDelta = 0.2f;

        [Header("Diagnostics - Do Not Modify")]
        public GroundState groundState = GroundState.Grounded;

        [Range(-1f, 1f)]
        public float horizontal;
        [Range(-1f, 1f)]
        public float vertical;

        [Range(-200f, 200f)]
        public float turnSpeed;

        [Range(-10f, 10f)]
        public float jumpSpeed;

        [Range(-1.5f, 1.5f)]
        public float animVelocity;

        [Range(-1.5f, 1.5f)]
        public float animRotation;

        public Vector3Int velocity;
        public Vector3 direction;

        public Vector2 turn;
        public float sensitivity = .5f;
        public Vector3 deltaMove;
        public float speed = 1;

        bool isMovable = true;
        bool isDying = false;
        bool isHit = false;
        bool isDashing = false;
        Vector3 DashDirection = Vector3.zero;
        [HideInInspector] public bool isDead = false;

        CanvasManager canvasManager = null;
        bool playerInitialized = false;

        void OnValidate()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            // Override CharacterController default values
            characterController.enabled = false;
            characterController.skinWidth = 0.02f;
            characterController.minMoveDistance = 0f;

            GetComponent<Rigidbody>().isKinematic = true;

            this.enabled = false;
        }

        public override void OnStartAuthority()
        {
            characterController.enabled = true;
            this.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            canvasManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<CanvasManager>();
        }

        public override void OnStopAuthority()
        {
            this.enabled = false;
            characterController.enabled = false;
        }

        void Start()
        {
        }
        void Update()
        {
            if (!characterController.enabled)
                return;
            if (playerInitialized == false)
            {
                playerNetwork.CmdSetPlayerSlot();
                //playerNetwork.CmdSetName(PlayerPrefs.GetString("PlayerName"));
                playerInitialized = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                canvasManager.Paused();
            }
            if (isDead == true || canvasManager.isPaused)
                return;
            if (playerStatController.IsDying() == true)
                Die();
            if (isHit && playerAnimatorController.isHit() == false)
                isHit = false;
            else if (isHit)
                return;
            if (isMovable == true)
            {
                HandleJumping();
                HandleMove();
            }
            HandleDash();
            HandleAttack();
            // Reset ground state
            if (characterController.isGrounded)
                groundState = GroundState.Grounded;
            else if (groundState != GroundState.Jumping)
                groundState = GroundState.Falling;

            // Diagnostic velocity...FloorToInt for display purposes
            velocity = Vector3Int.FloorToInt(characterController.velocity);
        }

        public void TakeDamage()
        {
            playerAnimatorController.GetHit();
            isHit = true;
        }
        
        private void Die()
        {
            if (isDying == false)
            {
                playerAnimatorController.Die();
                isDying = true;
                isMovable = false;
            }
            else if (playerAnimatorController.isDead() == true)
            {
                playerNetwork.CmdHideHealthBar(true, GetComponent<NetworkIdentity>());
                playerNetwork.CmdHideBody(true, GetComponent<NetworkIdentity>());
                Body.SetActive(false);
                isDead = true;
                canvasManager.SetObserversHUD();
                canvasManager.SetDeathView();
            }
        }

        public void Revive()
        {
            playerStatController.dying = false;
            playerNetwork.CmdHideHealthBar(false, GetComponent<NetworkIdentity>());
            playerNetwork.CmdHideBody(false, GetComponent<NetworkIdentity>());
            playerStatController.CmdHeal(100, GetComponent<NetworkIdentity>());

            playerAnimatorController.dying = false;
            Body.SetActive(true);
            isDead = false;
            isDying = false;
            isMovable = true;
            isHit = false;
            canvasManager.SetPlayerHUD();
        }
        void HandleTurning()
        {

            /*
            turn.x += Input.GetAxis("Mouse X") * sensitivity;
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;
            transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);*/

            // Q and E cancel each other out, reducing the turn to zero.
            /*
            if (Input.GetAxis("Mouse X")  != 0)
                turnSpeed = Mathf.MoveTowards(turnSpeed, -maxTurnSpeed, turnDelta);
            if (Input.GetKey(KeyCode.E))
                turnSpeed = Mathf.MoveTowards(turnSpeed, maxTurnSpeed, turnDelta);

            // If both pressed, reduce turning speed toward zero.
            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                turnSpeed = Mathf.MoveTowards(turnSpeed, 0, turnDelta);

            // If neither pressed, reduce turning speed toward zero.
            if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                turnSpeed = Mathf.MoveTowards(turnSpeed, 0, turnDelta);

            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);*/
        }
        void HandleDash()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && isDashing == false)
            {
                HandleMove();
                isDashing = true;
                isMovable = false;
                playerAnimatorController.SetDash();
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                DashDirection = new Vector3(horizontal, 0f, vertical);
            }
            else if (isDashing == true &&  playerAnimatorController.isDashing() == false)
            {
                isDashing = false;
                isMovable = true;
            }
            if (isDashing == true)
                characterController.Move(direction * 2 * Time.deltaTime);
        }
        void HandleAttack()
        {
            if (isDashing)
                return;
            if (Input.GetKey(KeyCode.Mouse0) && playerAnimatorController.isAttacking() == false)
            {
                isMovable = false;
                playerAnimatorController.SetAttackAnim();
                TryToShootSound.Play();
            }
            if (isMovable == false && playerAnimatorController.isAttacking() == false)
                isMovable = true;
            if (playerAnimatorController.isAttacking() && playerAnimatorController.isThrowArrow() == true)
            {
                TryToShootSound.Stop();
                ShootSound.Play();
                playerAttackController.Shoot();
            }
        }
        void HandleJumping()
        {
            // Handle variable force jumping.
            // Jump starts with initial power on takeoff, and jumps higher / longer
            // as player holds spacebar. Jump power is increased by a diminishing amout
            // every frame until it reaches maxJumpSpeed, or player releases the spacebar,
            // and then changes to the falling state until it gets grounded.
            if (groundState != GroundState.Falling && Input.GetKey(KeyCode.Space))
            {
                if (groundState != GroundState.Jumping)
                {
                    // Start jump at initial power.
                    groundState = GroundState.Jumping;
                    jumpSpeed = initialJumpSpeed;
                    playerAnimatorController.Jump();
                }
                else
                    // Jumping has already started...increase power toward maxJumpSpeed over time.
                    jumpSpeed = Mathf.MoveTowards(jumpSpeed, maxJumpSpeed, jumpDelta);

                // If power has reached maxJumpSpeed, change to falling until grounded.
                // This prevents over-applying jump power while already in the air.
                if (jumpSpeed == maxJumpSpeed)
                    groundState = GroundState.Falling;
            }
            else if (groundState != GroundState.Grounded)
            {
                // handles running off a cliff and/or player released Spacebar.
                groundState = GroundState.Falling;
                jumpSpeed = Mathf.Min(jumpSpeed, maxJumpSpeed);
                jumpSpeed += Physics.gravity.y * Time.deltaTime;
            }
            else
                jumpSpeed = Physics.gravity.y * Time.deltaTime;
        }

        // TODO: Directional input works while airborne...feature?
        void HandleMove()
        {
            if (isDashing)
                return;
            // Capture inputs
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            // Create initial direction vector without jumpSpeed (y-axis).
            direction = new Vector3(horizontal, 0f, vertical);
            playerAnimatorController.SetDirection(direction);

            if (characterController.isGrounded && direction != Vector3.zero)
            {
                playerAnimatorController.SetRunningAnim(true);
                if (FootStep.isPlaying == false)
                    FootStep.Play();
            }
            else
            {
                playerAnimatorController.SetRunningAnim(false);
                FootStep.Stop();
            }
            // Clamp so diagonal strafing isn't a speed advantage.
            direction = Vector3.ClampMagnitude(direction, 1f);

            // Transforms direction from local space to world space.
            direction = transform.TransformDirection(direction);

            // Multiply for desired ground speed.
            direction *= moveSpeedMultiplier;

            // Add jumpSpeed to direction as last step.
            direction.y = jumpSpeed;

            // Finally move the character.
            characterController.Move(direction * Time.deltaTime);
        }
    }
}
