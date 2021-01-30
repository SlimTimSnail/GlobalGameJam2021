using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    public LostObject CurrentSelected = null;
    public List<LostObject> LostObjects = null;
    public Button SubmitButton = null;
    public Button ReturnButton = null;

    public static GameScreen Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("Scene_End");
        }
    }

    public void OnObjectSelect(LostObject lostObject)
    {
        if(CurrentSelected != null)
        {
            CurrentSelected.Deselect();
        }
        CurrentSelected = lostObject;
        CurrentSelected.Select();
        SubmitButton.interactable = true;
        ReturnButton.interactable = true;
    }

    public void SubmitObject()
    {
        if(CurrentSelected.correctObject == true)
        {
            Debug.Log("Win");
        }
        else if(CurrentSelected.alreadySubmitted == true)
        {
            Debug.Log("I said Nope");
        }
        else
        {
            Debug.Log("Nope");
            CurrentSelected.alreadySubmitted = true;
        }
    }

    public void ReturnObjectToBox()
    {
        CurrentSelected.Deselect();
        CurrentSelected = null;
        SubmitButton.interactable = false;
        ReturnButton.interactable = false;
    }
}
