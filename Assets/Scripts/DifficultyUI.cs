using UnityEngine;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button startButton; // ← botón para iniciar el juego

    private void Start()
    {
        if (DificultManager.Instance == null) return;

        // Botones de dificultad
        easyButton?.onClick.AddListener(() => DificultManager.Instance.SetDifficulty("Fácil"));
        mediumButton?.onClick.AddListener(() => DificultManager.Instance.SetDifficulty("Media"));
        hardButton?.onClick.AddListener(() => DificultManager.Instance.SetDifficulty("Difícil"));

        // Botón de start / jugar
        startButton?.onClick.AddListener(() =>
        {
            if (DificultManager.Instance != null)
            {
                DificultManager.Instance.GoToGameScene();
            }
            else
            {
                Debug.LogWarning("⚠️ No hay instancia de DificultManager activa.");
            }
        });
    }
}