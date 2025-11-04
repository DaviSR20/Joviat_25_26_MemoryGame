using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    private GameManager gameManager;
    private Animator animator;
    private bool isMatched = false;

    public MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        GameObject o = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = o.GetComponent<GameManager>();
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        gameManager.TokenPressed(transform.parent.name);
    }

    public void ShowToken()
    {
        animator.SetBool("Show", true);
        animator.SetBool("Hide", false);
    }

    public void HideToken()
    {
        animator.SetBool("Hide", true);
        animator.SetBool("Show", false);
    }
    
    public void MatchToken()
    {
        isMatched = true;
        animator.SetBool("Win", true);
    }
    public bool IsMatched()
    {
        return isMatched;
    }

}
