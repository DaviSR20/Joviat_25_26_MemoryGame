using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject prefabToken;
    public Material[] materials;
    [SerializeField] public TextMeshProUGUI AlertaBtnGastat;
    [SerializeField] private TextMeshProUGUI timerText;
    public TextMeshProUGUI intentosText;
    public GameObject[,] tokens;
    public Button ShowAllButton;
    
    [Header("Audio")]
    public AudioClip matchSound;   // Sonido al acertar
    public AudioClip failSound;    // Sonido al fallar
    private AudioSource audioSource;

    private int rows;
    private int cols;
    private float spacing = 2f;

    private int numTokensOpened;
    private string token1Name;
    private string token2Name;
    private bool isClickShowAll = false;

    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    private int intentos = 0;

    void Start()
    {
        // 🔊 Configurar Audio
        audioSource = Camera.main.GetComponent<AudioSource>();

        // ⏱ Inicializar timer
        isTimerRunning = true;
        elapsedTime = 0f;

        // 🔢 Inicializar intentos
        intentos = 0;
        if (intentosText != null)
            intentosText.text = "Intents: 0";

        // ✅ Obtener dificultad del DificultManager de manera segura
        string dificultad = "Fácil"; // valor por defecto
        if (DificultManager.Instance != null)
        {
            string selected = DificultManager.Instance.GetSelectedDifficulty();
            if (!string.IsNullOrEmpty(selected))
                dificultad = selected;
        }

        Debug.Log("🎯 Dificultad recibida: " + dificultad);

        // 🔳 Configurar tamaño del tablero según la dificultad
        switch (dificultad)
        {
            case "Fácil":
                rows = 3;
                cols = 2;
                break;
            case "Media":
                rows = 4;
                cols = 4;
                break;
            case "Difícil":
                rows = 6;
                cols = 4;
                break;
            default:
                rows = 4;
                cols = 4;
                break;
        }

        // 🎲 Generar los tokens
        GenerarTablero();
    }
    void Update()
    {
        // Actualiza cronómetro
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    // Llamar esto cuando acabe el juego
    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Tiempo final: " + elapsedTime);
    }

    private void OnGameFinished()
    {
        StopTimer();
        Debug.Log("✅ Juego completado en " + elapsedTime + " segundos");

        // Guardar puntuación actual
        PlayerPrefs.SetFloat("LastScore", elapsedTime);

        // Comprobar si hay nuevo récord
        float best = PlayerPrefs.GetFloat("BestScore", float.MaxValue);
        if (elapsedTime < best)
        {
            PlayerPrefs.SetFloat("BestScore", elapsedTime);
            PlayerPrefs.Save();
            Debug.Log("🏆 Nuevo récord!");
        }

        // Cargar pantalla final
        SceneManager.LoadScene("ScoreMenu");
    }

    private void CheckGameCompletion()
    {
        foreach (var t in tokens)
        {
            Token token = t.GetComponentInChildren<Token>();
            if (token != null && !token.IsMatched())
                return;
        }

        Debug.Log("🎉 ¡Partida completada!");
        StopTimer();
        Invoke(nameof(OnGameFinished), 2f);
    }

    void GenerarTablero()
    {
        numTokensOpened = 0;
        tokens = new GameObject[rows, cols];

        int totalTokens = rows * cols;
        int numPairs = totalTokens / 2;

        // 🔹 Crear una lista de materiales para el tablero (cada uno aparece 2 veces)
        List<Material> materialesParaUsar = new List<Material>();

        for (int i = 0; i < numPairs; i++)
        {
            Material mat = materials[i % materials.Length]; // por si hay menos materiales disponibles
            materialesParaUsar.Add(mat);
            materialesParaUsar.Add(mat); // duplicar (para formar pareja)
        }

        // 🔹 Mezclar la lista (Fisher–Yates shuffle)
        for (int i = 0; i < materialesParaUsar.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, materialesParaUsar.Count);
            (materialesParaUsar[i], materialesParaUsar[randomIndex]) = (materialesParaUsar[randomIndex], materialesParaUsar[i]);
        }

        // 🔹 Crear los tokens en el tablero
        Vector3 startPos = new Vector3(-((cols - 1) * spacing) / 2, 0, ((rows - 1) * spacing) / 2);
        int indexM = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 pos = startPos + new Vector3(j * spacing * 1.25f, 0, -i * spacing);
                GameObject parentObj = Instantiate(prefabToken, pos, Quaternion.Euler(0, -90, 0));
                parentObj.name = $"Token_{i}_{j}";

                Token token = parentObj.GetComponentInChildren<Token>();
                token.mr.material = materialesParaUsar[indexM];
                indexM++;

                tokens[i, j] = parentObj;
            }
        }

        AlertaBtnGastat?.gameObject.SetActive(false);
    }

    public void TokenPressed(string name)
    {
        if (numTokensOpened < 2)
        {
            if (numTokensOpened == 0)
                token1Name = name;
            else if (numTokensOpened == 1)
            {
                if (token1Name == name)
                    return;
                token2Name = name;
            }

            Token token = GetTokenByName(name);
            token.ShowToken();

            numTokensOpened++;
        }

        if (numTokensOpened == 2)
        {
            // 👇 Aquí aumentamos el contador de intentos
            intentos++;
            if (intentosText != null)
                intentosText.text = "Intentos: " + intentos;

            Invoke(nameof(CheckTokens), 2.0f);
            numTokensOpened = 3;
        }
    }

    private Token GetTokenByName(string name)
    {
        string[] parts = name.Split('_');
        int i = int.Parse(parts[1]);
        int j = int.Parse(parts[2]);

        return tokens[i, j].GetComponentInChildren<Token>();
    }

    public void CheckTokens()
    {
        Token t1 = GetTokenByName(token1Name);
        Token t2 = GetTokenByName(token2Name);

        if (t1.mr.material.name == t2.mr.material.name)
        {
            t1.MatchToken();
            t2.MatchToken();
            PlaySound(matchSound); // ✅ sonido de acierto
            CheckGameCompletion();
        }
        else
        {
            t1.HideToken();
            t2.HideToken();
            PlaySound(failSound); // ❌ sonido de fallo
        }

        numTokensOpened = 0;
    }

    public void ShowAllTokensOnClick()
    {
        if (!isClickShowAll)
        {
            foreach (var t in tokens)
                t.GetComponentInChildren<Token>().ShowToken();

            Invoke(nameof(HideAllTokens), 2f);
            isClickShowAll = true;
            if (ShowAllButton != null)
                ShowAllButton.interactable = false;
        }
        else
        {
            AlertaBtnGastat.gameObject.SetActive(true);
            AlertaBtnGastat.text = "Ja has fet servir el boto";
            Invoke(nameof(HideAlert), 2f);
        }
    }

    private void HideAllTokens()
    {
        foreach (var t in tokens)
            t.GetComponentInChildren<Token>().HideToken();
    }

    private void HideAlert()
    {
        AlertaBtnGastat.gameObject.SetActive(false);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
 
}
