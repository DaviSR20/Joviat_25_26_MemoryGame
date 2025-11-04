using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 👈 necesario para cargar escenas

public class DificultManager : MonoBehaviour
{
    public static DificultManager Instance;

    [Header("Referencias UI")]
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private string selectedDifficulty = null; // 👈 guardará la dificultad elegida

    private void Awake()
    {
        // Evita duplicados y que se destruya al cargar nueva escena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Asignar eventos de click
        if (easyButton != null)
            easyButton.onClick.AddListener(() => OnButtonClicked("Fácil"));

        if (mediumButton != null)
            mediumButton.onClick.AddListener(() => OnButtonClicked("Media"));

        if (hardButton != null)
            hardButton.onClick.AddListener(() => OnButtonClicked("Difícil"));
    }

    private void OnButtonClicked(string dificultad)
    {
        selectedDifficulty = dificultad; // ✅ guardar dificultad elegida
        Debug.Log($"🔘 Botón de dificultad pulsado: {dificultad}");

        // Puedes dar feedback visual aquí si quieres
        // (por ejemplo, cambiar el color del botón o mostrar un texto)
    }

    // 👇 Esta función la puedes llamar desde un botón "Jugar"
    public void GoToGameScene()
    {
        if (string.IsNullOrEmpty(selectedDifficulty))
        {
            Debug.LogWarning("⚠️ No se ha elegido una dificultad antes de continuar.");
            return;
        }

        Debug.Log($"🚀 Cargando GameScene con dificultad: {selectedDifficulty}");
        SceneManager.LoadScene("GameScene");
    }

    // 👇 Función para que otros scripts (como GameManager) puedan saber la dificultad
    public string GetSelectedDifficulty()
    {
        return selectedDifficulty;
    }
}
