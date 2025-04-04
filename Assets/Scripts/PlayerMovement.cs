using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed = 2.5f;
	public float sprintSpeed = 3.0f;
	public float crouchSpeed = 1.5f;
	public float jumpHeight = 2.0f;
	public float mouseSensitivity = 2.0f;
	public Transform playerCamera;

	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	private float gravity = -9.8f;

	public float groundDistance = 0.4f;
	private float originalHeight;
	private float crouchHeight = 1.0f;
	private float crouchTransitionSpeed = 6f;

	private bool isCrouched;
	private float targetHeight;
	private Vector3 targetCamPos;

	private Vector3 move;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		originalHeight = controller.height;
		targetHeight = controller.height;
		targetCamPos = playerCamera.localPosition;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		HandleMovement();
		HandleCrouch();
		HandleJump();
		ApplyGravity();
		SmoothCrouchTransition();
	}

	void HandleMovement()
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");
		move = transform.right * moveX + transform.forward * moveZ;

		float currentSpeed = movementSpeed;

		if (Input.GetKey(KeyCode.LeftShift))
		{
			currentSpeed = sprintSpeed;
		}
		else if (Input.GetKey(KeyCode.LeftControl))
		{
			currentSpeed = crouchSpeed;
		}

		controller.Move(move * currentSpeed * Time.deltaTime);
	}

	void HandleCrouch()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			isCrouched = true;
			targetHeight = crouchHeight;
			targetCamPos = new Vector3(playerCamera.localPosition.x, 0.5f, playerCamera.localPosition.z);
		}
		else if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			isCrouched = false;
			targetHeight = originalHeight;
			targetCamPos = new Vector3(playerCamera.localPosition.x, 0.9f, playerCamera.localPosition.z);
		}
	}

	void HandleJump()
	{
		groundedPlayer = Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + 0.1f);

		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

		if (Input.GetButtonDown("Jump") && groundedPlayer)
		{
			playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
	}

	void ApplyGravity()
	{
		playerVelocity.y += gravity * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}

	void SmoothCrouchTransition()
	{
		controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
		playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetCamPos, Time.deltaTime * crouchTransitionSpeed);
	}

	void LateUpdate()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

		transform.Rotate(Vector3.up * mouseX);

		Vector3 cameraRotation = playerCamera.localEulerAngles;
		cameraRotation.x -= mouseY;

		if (cameraRotation.x > 180) cameraRotation.x -= 360;
		cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);

		playerCamera.localEulerAngles = new Vector3(cameraRotation.x, 0f, 0f);
	}
}
