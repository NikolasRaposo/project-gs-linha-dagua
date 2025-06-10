using UnityEngine;
public class BoatController : MonoBehaviour {
        [Header("Boat Settings")]
        public float moveSpeed = 10f;
        public float turnSpeed = 50f;
        private Vector2 _moveInput;
        private InputSystem_Actions _input;
        private void Awake() {
            _input = new InputSystem_Actions();
            _input.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _input.Player.Move.canceled += _ => _moveInput = Vector2.zero;
        }
        private void OnEnable() {
            _input.Player.Enable();
        }
        private void OnDisable() {
            _input.Player.Disable();
        }
        private void FixedUpdate() {
            var forwardMovement = transform.forward * (_moveInput.y * moveSpeed * Time.fixedDeltaTime);
            transform.position += forwardMovement;
            var turn = _moveInput.x * turnSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, turn, 0);
        }
}
