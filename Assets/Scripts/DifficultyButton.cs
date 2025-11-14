using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public string difficulty; // Fácil, Media, Difícil

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
        });
    }
}