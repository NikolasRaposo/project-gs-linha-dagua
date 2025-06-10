using TMPro;
using UnityEngine;
public class RescueDeliveryZone : MonoBehaviour {
    public int requiredRescuesToWin = 6;
    private int _deliveredCount;
    public RescueSystem rescueSystem;
    public TextMeshProUGUI rescueeIndicator;
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        var passengers = rescueSystem.GetSeatedPassengers();
        if (passengers.Count == 0) return;
        foreach (var npc in passengers) {
            if (npc != null) {
                npc.ResetToSwimming();
                Destroy(npc.gameObject);
                _deliveredCount++;
                rescueeIndicator.text = $"{_deliveredCount}/{requiredRescuesToWin}";
            }
        }
        rescueSystem.ClearSeatedPassengers();
        if (_deliveredCount >= requiredRescuesToWin) {
            rescueSystem.TriggerWinGame();
        }
    }
}