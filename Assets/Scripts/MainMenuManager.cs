using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClickButton()
    {
        Debug.Log("Hola món");
        //SceneManager.LoadScene("GameScene");
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(audioClip);
    }

    public void onValueChange(string value)
    {
        Debug.Log("el contingut és:" + value);
    }

    public void onValueChange2()
    {
        Debug.Log("el contingut és:");
    }

    public void OnToogleClicked(bool flag)
    {
        if (flag)
        {
            Debug.Log("click ");
        }
    }
    public void ChangeSceneGame()
    {
        SceneManager.LoadScene("DificultMenu");
    }

}
