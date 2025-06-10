using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RescueSystem : MonoBehaviour {
    [Header("Scene Settings")]
    public float sceneTimer = 60f;
    [Header("Rescue Settings")]
    public float rescueHoldTime = 2f;
    public Transform[] passengerSeats;
    public LayerMask rescueLayer;
    public float rescueRange = 3f;
    [Header("References")]
    public Transform boatTransform;
    public GameObject hudCanvas;
    public GameObject winGamePanel;
    public GameObject endGamePanel;
    public TextMeshProUGUI timerText;

    private float _timeRemaining;
    private bool _timerIsRunning = true;
    private InputSystem_Actions _input;
    private bool _isHoldingInteract;
    private float _holdTimer;
    private bool _gameHasEnded;
    private int _passengersOnBoard;
    private GameObject _currentTarget;

    private void Awake() {
        _input = new InputSystem_Actions();
        _input.Player.Interact.performed += _ => {
            _isHoldingInteract = true;
        };
        _input.Player.Interact.canceled += _ => {
            _isHoldingInteract = false;
            _holdTimer = 0f;
        };
    }
    private void OnEnable() { _input.Player.Enable(); }
    private void OnDisable() { _input.Player.Disable(); }
    private void Start() { _timeRemaining = sceneTimer; }
    private void Update() {
        if (_timerIsRunning) {
            if (_timeRemaining > 0) {
                _timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            } else {
                _timeRemaining = 0;
                _timerIsRunning = false;
                TriggerEndGame();
            }
        }
        DetectRescueTarget();
        if (_isHoldingInteract) {
            if (_currentTarget != null) {
                if (_passengersOnBoard < passengerSeats.Length) {
                    _holdTimer += Time.deltaTime;
                    var progress = Mathf.Clamp01(_holdTimer / rescueHoldTime);
                    _currentTarget.GetComponent<RescueTarget>().SetRescueProgress(progress);
                    if (_holdTimer >= rescueHoldTime) {
                        RescuePerson();
                        _holdTimer = 0f;
                    }
                }
            }
        }
    }

    private void DetectRescueTarget() {
        Collider[] hits = Physics.OverlapSphere(boatTransform.position, rescueRange, rescueLayer);
        if (hits.Length > 0) {
            if (_currentTarget != hits[0].gameObject) {
                _currentTarget = hits[0].gameObject;
                _currentTarget.GetComponent<RescueTarget>().ShowIndicator();
            }
        } else {
            if (_currentTarget != null) {
                _currentTarget.GetComponent<RescueTarget>().HideIndicator();
            }
            _currentTarget = null;
            _holdTimer = 0f;
        }
    }

    private void RescuePerson() {
        if (_currentTarget == null) {
            return;
        }
        RescueTarget targetScript = _currentTarget.GetComponent<RescueTarget>();
        if (targetScript != null) {
            Transform seat = passengerSeats[_passengersOnBoard];
            targetScript.SetRescued(seat);
            _passengersOnBoard++;
        }
    }
    private void OnDrawGizmosSelected() {
        if (boatTransform != null) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(boatTransform.position, rescueRange);
        }
    }
    public List<RescueTarget> GetSeatedPassengers() {
        List<RescueTarget> seatedPassengers = new List<RescueTarget>();
        foreach (Transform seat in passengerSeats) {
            if (seat.childCount > 0) {
                RescueTarget target = seat.GetChild(0).GetComponent<RescueTarget>();
                if (target != null) {
                    seatedPassengers.Add(target);
                }
            }
        }
        return seatedPassengers;
    }
    public void ClearSeatedPassengers() {
        foreach (Transform seat in passengerSeats) {
            if (seat.childCount > 0) {
                Destroy(seat.GetChild(0).gameObject);
            }
        }
        _passengersOnBoard = 0;
    }
    private void UpdateTimerUI() {
        if (timerText == null) return;
        var minutes = Mathf.FloorToInt(_timeRemaining / 60);
        var seconds = Mathf.FloorToInt(_timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    public void TriggerWinGame() {
        if (_gameHasEnded) return;
        _gameHasEnded = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        hudCanvas.SetActive(false);
        winGamePanel.SetActive(true);
    }

    private void TriggerEndGame() {
        if (_gameHasEnded) return;
        _gameHasEnded = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        hudCanvas.SetActive(false);
        endGamePanel.SetActive(true);
    }

}
