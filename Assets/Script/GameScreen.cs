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

    public LoadObjectsFromDefinitions ObjectLoader = null;

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
        LostObjects = ObjectLoader.LostObjects;

        int rand = Random.Range(0, LostObjects.Count);
        LostObjects[rand].correctObject = true;

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
            LoserVideo.clip = Loser.Outro;
            LoserVideo.Play();
        }
        else
		{
            OnWrongSubmit();
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

    private void OnWrongSubmit()
	{
        if (CurrentSelected.WasSubmitted)
        {
            Debug.Log("I said Nope");
            LoserVideo.clip = Loser.Repeat;
        }
        else
        {
            Debug.Log("Nope");

            Dictionary<string, int> statToAmount = new Dictionary<string, int>()
            {
                { "size", 0 },
                { "colour", 0 },
                { "category", 0 }
            };

            LostObjectDefinitions.LostObjectDefinition currentDef = CurrentSelected.Definition;
            LostObjectDefinitions.LostObjectDefinition correctDef = default;

            for (int i = 0; i < LostObjects.Count; i++)
			{
                if (LostObjects[i].correctObject)
				{
                    correctDef = LostObjects[i].Definition;
				}

                if (LostObjects[i].Definition.Size == currentDef.Size)
				{
                    statToAmount["size"] = statToAmount["size"] + 1;
				}

                if (LostObjects[i].Definition.Colour == currentDef.Colour)
                {
                    statToAmount["colour"] = statToAmount["colour"] + 1;
                }

                if (LostObjects[i].Definition.Category == currentDef.Category)
                {
                    statToAmount["category"] = statToAmount["category"] + 1;
                }
            }

            if (currentDef.Size == correctDef.Size)
			{
                statToAmount.Remove("size");
			}

            if (currentDef.Colour == correctDef.Colour)
            {
                statToAmount.Remove("colour");
            }

            if (currentDef.Category == correctDef.Category)
            {
                statToAmount.Remove("category");
            }

            string smallest = null;

            foreach (KeyValuePair<string, int> kv in statToAmount)
			{
                if (smallest == null
                    || kv.Value <= statToAmount[smallest])
				{
                    smallest = kv.Key;
				}
			}

            switch (smallest)
			{
                case "size":
                    if (currentDef.Size > correctDef.Size)
					{
                        LoserVideo.clip = Loser.TooBig;
					}
                    else
					{
                        LoserVideo.clip = Loser.TooSmall;
					}
                    break;
                case "colour":
                    LoserVideo.clip = Loser.ColourClips[(int)currentDef.Colour];
                    break;
                case "category":
                    LoserVideo.clip = Loser.CategoryClips[(int)currentDef.Category];
                    break;
            }

            LoserVideo.Play();

            CurrentSelected.WasSubmitted = true;
        }
    }
}
