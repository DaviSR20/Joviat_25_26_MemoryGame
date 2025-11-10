using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreMenuManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI messageText;
    public Button mainMenuButton;
    public Button retryButton;

    [Header("Efectos")]
    public ParticleSystem confettiEffect; // 🎉 Arrastra el Particle System aquí desde el inspector

    public ParticleSystem confettiEffect2;
    private void Start()
    {
        // Recuperar los valores guardados
        float currentTime = PlayerPrefs.GetFloat("LastScore", -1f);
        float bestTime = PlayerPrefs.GetFloat("BestScore", -1f);

        // Mostrar el tiempo actual
        if (currentTime >= 0)
            currentTimeText.text = $"Temps total: {currentTime:F2} s";
        else
            currentTimeText.text = "Temps total: —";

        // Mostrar el Best Score si existe
        if (bestTime >= 0)
            bestScoreText.text = $"Millor temps: {bestTime:F2} s";
        else
            bestScoreText.text = "Millor temps: —";

        // Mensaje dinámico
        if (bestTime > 0 && currentTime > 0)
        {
            if (currentTime < bestTime)
            {
                messageText.text = "Nou rècord! Bona feina!";
                PlayConfetti();
            }
            else{
                messageText.text = "Bona partida! Torna-ho a intentar!";
                PlayConfetti();
            }
        }
        else
        {
            messageText.text = "Has completat la partida!";
            PlayConfetti();
        }

        // Asignar botones
        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        retryButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
    }
    private void PlayConfetti()
    {
        if (confettiEffect != null)
        {
            confettiEffect.Play();
            confettiEffect2.Play();
        }
        else
        {
            Debug.LogWarning("⚠️ No se ha asignado el efecto de confeti al ScoreMenuManager.");
        }
    }

}