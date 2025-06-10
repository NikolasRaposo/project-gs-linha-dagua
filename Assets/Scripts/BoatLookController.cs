using UnityEngine;
public class BoatLookController : MonoBehaviour {
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    [Header("References")]
    public Transform cameraHolder;
    private InputSystem_Actions _controls;
    private Vector2 _lookInput;
    private float _xRotation;
    private float _yRotation;
    private void Awake() {
        _controls = new InputSystem_Actions();
        _controls.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _controls.Player.Look.canceled += _ => _lookInput = Vector2.zero;
    }
    private void OnEnable() {
        _controls.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable() {
        _controls.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update() {
        var mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        var mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;
        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);  // Limite para não girar demais
        cameraHolder.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
}
