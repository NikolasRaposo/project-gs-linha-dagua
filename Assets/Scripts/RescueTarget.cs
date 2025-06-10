using UnityEngine;
using UnityEngine.UI;
public class RescueTarget : MonoBehaviour {
    private static readonly int Swim = Animator.StringToHash("Swim");
    private static readonly int Sit = Animator.StringToHash("Sit");
    [Header("Animation")]
    public Animator animator;
    [Header("Seated Behaviour")] 
    public FitToWaterSurface fitToWaterSurface;
    [Header("UI Indicator")]
    public Canvas targetCanvas;
    public GameObject interactionIndicator;
    public Image interactionOutlier;
    public void ShowIndicator() {
        if (interactionIndicator != null) {
            interactionIndicator.SetActive(true);
        }
    }
    public void HideIndicator() {
        if (interactionIndicator != null) {
            interactionIndicator.SetActive(false);
        }
    }
    public void SetRescueProgress(float progress) {
        if (interactionOutlier != null) {
            interactionOutlier.fillAmount = progress;
        }
    }
    public void SetRescued(Transform seat) {
        if (animator != null) {
            animator.SetTrigger(Sit);
        }
        transform.SetParent(seat);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        fitToWaterSurface.enabled = false;
        targetCanvas.enabled = false;
    }
    public void ResetToSwimming() {
        if (animator != null) {
            animator.SetTrigger(Swim);
        }
        transform.SetParent(null);
    }
}