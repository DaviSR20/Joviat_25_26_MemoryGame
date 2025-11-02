using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject prefabToken;

    public int rows = 4;
    public int cols = 4;
    public float spacing = 2f; // Espacio entre tokens

    public GameObject[,] tokens; // Array 2D para guardar los tokens

    public Material[] materials;
    [SerializeField] public TextMeshProUGUI AlertaBtnGastat;

    private int numTokensOpened;
    private string token1Name;
    private string token2Name;
    private bool isClickShowAll = false;
    void Start()
    {
        numTokensOpened = 0;
        tokens = new GameObject[rows, cols];

        // Coordenada inicial (puedes ajustarla según tu escena)
        Vector3 startPos = new Vector3(-((cols - 1) * spacing) / 2, 0, ((rows - 1) * spacing) / 2);
        int indexM = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 pos = startPos + new Vector3(j * spacing * 1.25f, 0, -i * spacing);
                GameObject o = Instantiate(prefabToken, pos, Quaternion.identity);
                o.name = $"Token_{i}_{j}";
                o.GetComponent<Token>().mr.material = materials[indexM];
                indexM = (indexM + 1) % materials.Length;
                tokens[i, j] = o;
            }
        }
        AlertaBtnGastat.gameObject.SetActive(false);
    }

    public void TokenPressed(string name)
    {

        if (numTokensOpened < 2)
        {

            if (numTokensOpened == 0)
            {
                token1Name = name;
                //dir-li al token que es mostri
            }
            else if (numTokensOpened == 1)
            {
                if (token1Name == name)
                {
                    return;
                }
                token2Name = name;

            }

            Token token = GetTokenByName(name);
            token.ShowToken();

            numTokensOpened++;
            Debug.Log("Tokens opened: " + numTokensOpened);


        }

        if (numTokensOpened == 2)
        {
            //si hem obert dos tokens aleshores posar timer en marxa:
            Invoke("CheckTokens", 2.0f);
            numTokensOpened = 3;
        }



    }

    private Token GetTokenByName(string name)
    {
        int i, j; ////a partir del nom obtenim els valors de i,j
        string[] parts = name.Split('_');
        i = int.Parse(parts[1]);
        j = int.Parse(parts[2]);
        return tokens[i, j].GetComponent<Token>();
    }

    public void CheckTokens()
    {
        Token t1 = GetTokenByName(token1Name);
        Token t2 = GetTokenByName(token2Name);

        Debug.Log(t1.mr.material.name);
        Debug.Log(t2.mr.material.name);
        if (t1.mr.material.name == t2.mr.material.name)
        {
            t1.MatchToken();
            t2.MatchToken();
        }
        else
        {
            t1.HideToken();
            t2.HideToken();
        }




        numTokensOpened = 0;

    }

    public void Update()
    {
        if (tokens == null)
        {
            SceneManager.LoadScene("ScoreMenu");
        }
    }
    private void ShowAllTokens()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Token token = tokens[i, j].GetComponent<Token>();
                token.ShowToken();
            }
        }
    }
    private void HideAllTokens()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Token token = tokens[i, j].GetComponent<Token>();
                token.HideToken();
            }
        }
    }

    public void ShowAllTokensOnClick()
    {
        if (!isClickShowAll)
        {
            ShowAllTokens();
            Invoke("HideAllTokens", 2f); // opcional, los oculta después de 2 segundos
            isClickShowAll = true;
        }
        else
        {
            AlertaBtnGastat.gameObject.SetActive(true);
            AlertaBtnGastat.text = "Ja has fet servir el boto";

            // Oculta el mensaje después de 5 segundos
            Invoke(nameof(HideAlert), 2f);
        }
    }

    private void HideAlert()
    {
        AlertaBtnGastat.gameObject.SetActive(false);
    }
}