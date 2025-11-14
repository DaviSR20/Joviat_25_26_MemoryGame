using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DificultManager : MonoBehaviour
{
    public static DificultManager Instance;

    private string selectedDifficulty = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Suscribirse al evento de carga de escena para reasignar botones
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 🔹 Se ejecuta cada vez que se carga una escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DificultadScene") // nombre de tu escena de dificultad
        {
            // Buscar los botones en la escena recién cargada
            Button easyButton = GameObject.Find("EasyButton")?.GetComponent<Button>();
            Button mediumButton = GameObject.Find("MediumButton")?.GetComponent<Button>();
            Button hardButton = GameObject.Find("HardButton")?.GetComponent<Button>();

            // Asignar eventos de click
            if (easyButton != null)
                easyButton.onClick.AddListener(() => OnButtonClicked("Fácil"));

            if (mediumButton != null)
                mediumButton.onClick.AddListener(() => OnButtonClicked("Media"));

            if (hardButton != null)
                hardButton.onClick.AddListener(() => OnButtonClicked("Difícil"));
        }
    }

    private void OnButtonClicked(string dificultad)
    {
        selectedDifficulty = dificultad;
        Debug.Log("🔘 Dificultad elegida: " + dificultad);
    }
    public void SetDifficulty(string dificultad)
    {
        selectedDifficulty = dificultad;
        Debug.Log("🔘 Dificultad elegida: " + dificultad);
    }

    public string GetSelectedDifficulty()
    {
        return selectedDifficulty;
    }

    public void GoToGameScene()
    {
        if (string.IsNullOrEmpty(selectedDifficulty))
        {
            Debug.LogWarning("⚠️ No se ha elegido dificultad.");
            return;
        }

        SceneManager.LoadScene("GameScene");
    }
}
