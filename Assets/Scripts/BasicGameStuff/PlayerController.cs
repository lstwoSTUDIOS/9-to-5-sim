using UnityEngine;

namespace BasicGameStuff
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public static bool MouseCaptured
        {
            get => !Cursor.visible;
            set
            {
                Cursor.visible = !value;
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            }
        }

        [Header("Assigns")]
        public GameObject camPivot;
        public GameObject cameraObject;
        public GameObject grabPosObject;

        [Header("Player Settings")]
        public float speed = 5.0f;
        public float jumpVelocity = 4.5f;
        public float grabSpeed = 1f;
        public float sprintMultiplier = 1.25f;
        public float bunnyhopSpeed = 0.25f;
        public LayerMask groundMask;
        public LayerMask pickupMask;

        [Header("Camera Settings")]
        public float xSensitivity = 0.2f;
        public float ySensitivity = 0.2f;

        [Header("Feature Set")]
        public bool allowJumping = true;
        public bool allowLookUp = true;
        public bool enableGrabbing = true;
        public bool enableBunnyhopping = true;

        private Rigidbody grabbedBody;
        private int grabbedBodyLayer;
        private Rigidbody rb;
        private Vector3 velocity;

        private bool IsGrabbing => grabbedBody != null;
        private bool isWalking = false;
        private bool isSprinting = false;
        private bool isCrouching = false;
        private float startingCamPos;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            Instance = this;
            MouseCaptured = true;
            Time.timeScale = 1f;
            camPivot.transform.localRotation = transform.localRotation;
            transform.localRotation = Quaternion.identity;
            startingCamPos = camPivot.transform.localPosition.y;
        }

        private void Update()
        {
            velocity = rb.linearVelocity;
            rb.angularVelocity = Vector3.zero;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                MouseCaptured = !MouseCaptured;
                PauseMenu.Instance.gameObject.SetActive(!MouseCaptured);
                Time.timeScale = MouseCaptured ? 1f : 0f;
            }

            if (MouseCaptured)
            {
                HandleInput();
                RotateCamera();
            }

            HandleGrabbing();
            ApplyVelocity();
            
            HeadBobbing();
        }

        private void Jump(Vector3 direction)
        {
            if (!Input.GetKey(KeyCode.Space) || !IsOnFloor() || !allowJumping)
            {
                return;
            }

            velocity.y = jumpVelocity;

            if(!enableBunnyhopping)
            {
                return;
            }

            var forward = camPivot.transform.forward;
            var right = camPivot.transform.right;

            forward.y = 0;
            right.y = 0;

            var moveDir = (forward * direction.z + right * direction.x).normalized;

            rb.AddForce(moveDir * bunnyhopSpeed, ForceMode.Impulse);
        }

        private void HandleInput()
        {
            Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 direction = (transform.rotation * new Vector3(inputDir.x, 0, inputDir.y)).normalized;

            isWalking = direction.magnitude > 0.1f;
            
            Move(direction);
            Jump(direction);

            var lookingAtItem = Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out var hit, 3f, pickupMask);

            Crosshair.CrosshairColor = lookingAtItem ? Color.red : Color.white;

            if (Input.GetMouseButton(0) && lookingAtItem && !IsGrabbing)
            {
                var rigidbody = hit.rigidbody;

                if (!rigidbody || !rigidbody.TryGetComponent<PickUpItem>(out var pickUpItem))
                {
                    return;
                }

                grabbedBody = rigidbody;
                grabbedBodyLayer = grabbedBody.gameObject.layer;

                pickUpItem.OnPickupStart();
            }

            else if ((!Input.GetMouseButton(0) && IsGrabbing) || (grabbedBody && !grabbedBody.gameObject.activeInHierarchy))
            {
                grabbedBody.GetComponent<PickUpItem>().OnPickupEnd();
                grabbedBody = null;

                return;
            }
        }

        private float speedPercentage = 1;

        private void Move(Vector3 direction)
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift);
            isCrouching = Input.GetKey(KeyCode.LeftControl);
            
            var sprintSpeed = speed * (isSprinting ? sprintMultiplier : 1f);
            var crouchSpeed = sprintSpeed * (isCrouching ? 0.5f : 1f);
            var moveSpeed = crouchSpeed * speedPercentage;

            var forward = camPivot.transform.forward;
            var right = camPivot.transform.right;

            forward.y = 0;
            right.y = 0;

            var moveDir = (forward * direction.z + right * direction.x).normalized;

            velocity.x = Mathf.MoveTowards(velocity.x, moveDir.x * moveSpeed, moveSpeed * 10f * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, moveDir.z * moveSpeed, moveSpeed * 10f * Time.deltaTime);
        }


        private void ApplyVelocity()
        {
            rb.linearVelocity = velocity;
        }

        private void RotateCamera()
        {
            var pivotRot = camPivot.transform.rotation.eulerAngles;
            var camRot = cameraObject.transform.localRotation.eulerAngles;

            var adjustedXSensitivity = xSensitivity * 0.1f;
            var adjustedYSensitivity = ySensitivity * 0.1f;

            pivotRot.y += Input.GetAxisRaw("Mouse X") * adjustedXSensitivity;

            var targetXRotation = camRot.x - Input.GetAxisRaw("Mouse Y") * adjustedYSensitivity;
            camRot.x = allowLookUp ? Mathf.Clamp((targetXRotation > 180 ? targetXRotation - 360 : targetXRotation), -90, 90) : 0;

            camPivot.transform.rotation = Quaternion.Euler(pivotRot);
            cameraObject.transform.localRotation = Quaternion.Euler(camRot);
        }


        private void HandleGrabbing()
        {
            if (!IsGrabbing)
            {
                speedPercentage = 1;
                return;
            }

            var distance = grabPosObject.transform.position - grabbedBody.position;
            var pickUpItem = grabbedBody.GetComponent<PickUpItem>();

            grabbedBody.linearVelocity = distance * grabSpeed * pickUpItem.grabSpeedMultiplier;
            grabbedBody.angularVelocity = Vector3.zero;

            speedPercentage = Mathf.Clamp(speedPercentage, 0, 1);

            if (grabbedBody.GetComponent<PickUpItem>().lookAtPlayer)
            {
                grabbedBody.transform.LookAt(cameraObject.transform.position);
                grabbedBody.transform.rotation = Quaternion.Euler(0, grabbedBody.transform.rotation.eulerAngles.y, 0);
            }
        }

        private float headBobbingTimer = 0f;
        
        private void HeadBobbing()
        {
            var pos = camPivot.transform.localPosition.y;
            
            if (isWalking && IsOnFloor())
            {
                if (isSprinting)
                {
                    pos = startingCamPos + Mathf.Sin(headBobbingTimer * 15f) * 0.1f;
                }
                else if (isCrouching)
                {
                    pos = startingCamPos + Mathf.Sin(headBobbingTimer * 5f) * 0.1f;
                }
                else
                {
                    pos = startingCamPos + Mathf.Sin(headBobbingTimer * 10f) * 0.1f;
                }
                
                headBobbingTimer += Time.deltaTime;
            }
            else
            {
                pos = Mathf.Lerp(camPivot.transform.localPosition.y, startingCamPos, Time.deltaTime * 2f);
                headBobbingTimer = 0f;
            }
            
            camPivot.transform.localPosition = new Vector3(camPivot.transform.localPosition.x, pos, camPivot.transform.localPosition.z);
        }

        private bool IsOnFloor()
        {
            return Physics.Raycast(transform.position, Vector3.down, transform.localScale.y + 0.05f, groundMask);
        }
    }
}
