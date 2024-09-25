using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Movement
    public CharacterController controller;
    public Transform cameraTransform;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    Vector3 velocity;
    bool isGrounded;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform groundCheck;

    private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    #endregion
    
    void Start()
    {
        // Menyembunyikan kursor dan menguncinya di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Cek apakah player berada di tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input gerakan
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Gerakkan player
        controller.Move(move * speed * Time.deltaTime);

        // Lompatan
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravitasi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Rotasi kamera dan player mengikuti input mouse
        RotatePlayerAndCamera();
    }

    void RotatePlayerAndCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotasi horizontal untuk player (kiri-kanan)
        transform.Rotate(Vector3.up * mouseX);

        // Rotasi vertikal untuk kamera (atas-bawah)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Batasi rotasi vertikal agar tidak lebih dari 90 derajat

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Atur rotasi kamera
    }
}
