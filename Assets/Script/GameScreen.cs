using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameScreen : MonoBehaviour
{
	public VideoPlayer ObjectVideo = null;
    public VideoPlayer LoserVideo = null;

	public LostObject CurrentSelected = null;
    public List<LostObject> LostObjects = null;
    public Button SubmitButton = null;
    public Button ReturnButton = null;

    public LoserDefinition Loser = null;

    public static GameScreen Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(LoserVideo != null)
        {
            LoserVideo.clip = Loser.Intro;
            LoserVideo.gameObject.SetActive(true);
            LoserVideo.Play();
            LoserVideo.loopPointReached += PlayIdle;
        }
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

        if (ObjectVideo != null)
		{
            ObjectVideo.clip = CurrentSelected.Definition.VideoClip;
            ObjectVideo.gameObject.SetActive(true);
            ObjectVideo.Play();
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

        if (ObjectVideo != null)
        {
            ObjectVideo.Stop();
            ObjectVideo.gameObject.SetActive(false);
        }
    }

    public void PlayIdle(VideoPlayer vp)
    {
        int rand = Random.Range(0, 3);
        switch(rand)
        {
            case 0:
                LoserVideo.clip = Loser.Idle_01;
                LoserVideo.Play();
                break;
            case 1:
                LoserVideo.clip = Loser.Idle_02;
                LoserVideo.Play();
                break;
            case 2:
                LoserVideo.clip = Loser.Idle_03;
                LoserVideo.Play();
                break;
        }
    }
}
