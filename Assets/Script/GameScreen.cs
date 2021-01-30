using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameScreen : MonoBehaviour
{
	[SerializeField]
	private VideoPlayer m_videoPlayer = null;

	public LostObject CurrentSelected = null;
    public List<LostObject> LostObjects = null;
    public Button SubmitButton = null;
    public Button ReturnButton = null;

    public static GameScreen Instance;

    private void Awake()
    {
        Instance = this;
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

        if (m_videoPlayer != null)
		{
            m_videoPlayer.clip = CurrentSelected.Definition.VideoClip;
            m_videoPlayer.gameObject.SetActive(true);
            m_videoPlayer.Play();
        }
    }

    public void SubmitObject()
    {
        if(CurrentSelected.correctObject == true)
        {
            Debug.Log("Win");
        }
        else if(CurrentSelected.Submitted == true)
        {
            Debug.Log("I said Nope");
        }
        else
        {
            Debug.Log("Nope");
            CurrentSelected.Submitted = true;
        }
    }

    public void ReturnObjectToBox()
    {
        CurrentSelected.Deselect();
        CurrentSelected = null;
        SubmitButton.interactable = false;
        ReturnButton.interactable = false;

        if (m_videoPlayer != null)
        {
            m_videoPlayer.Stop();
            m_videoPlayer.gameObject.SetActive(false);
        }
    }
}
