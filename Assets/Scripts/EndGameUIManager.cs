using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGameUIManager : MonoBehaviour {
    public void RestartGame() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void BeginGame() {
        SceneManager.LoadScene(1);
    }

	public void ExitGame() {
		Application.Quit();
	}
}
