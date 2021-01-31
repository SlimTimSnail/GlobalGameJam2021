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

    public List<LoserDefinition> LoserDefinitions = null;
    private LoserDefinition CurrentLoser = null;

    [SerializeField]
    private ClockLogic m_clockLogic;

    public static GameScreen Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LostObjects = ObjectLoader.LostObjects;
        ObjectVideo.waitForFirstFrame = true;

        Reset();
    }

	private void Reset()
	{
        foreach(LostObject lo in LostObjects)
		{
            lo.WasSubmitted = false;
		}

        int rand = Random.Range(0, LoserDefinitions.Count);
        CurrentLoser = LoserDefinitions[rand];

        rand = Random.Range(0, LostObjects.Count);
        LostObjects[rand].correctObject = true;

        if(LoserVideo != null)
        {
            LoserVideo.clip = CurrentLoser.Intro;
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
        else if (Input.GetKeyDown(KeyCode.Return))
		{
            LostObject correctObject = null;

            for (int i = 0; i < LostObjects.Count; i++)
			{
                if (LostObjects[i].correctObject)
				{
                    correctObject = LostObjects[i];
                    break;
				}
			}

            OnObjectSelect(correctObject);
            SubmitObject();
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
        m_clockLogic.RunningRealtimeMultipler *= 0.95f;

        if (CurrentSelected.correctObject)
        {
            Debug.Log("Win");
            LoserVideo.clip = CurrentLoser.Outro;
            LoserVideo.Play();

            LostObject correctObj = CurrentSelected;
            ReturnObjectToBox();
            LostObjects.Remove(correctObj);
            Destroy(correctObj.gameObject);

            if (LostObjects.Count == 0)
			{
                StartCoroutine(GoToEndCoroutine());
			}
            else
			{
                StartCoroutine(NextLevelCoroutine());
            }
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
            ObjectVideo.targetTexture.Release();
            ObjectVideo.gameObject.SetActive(false);
        }
    }

    public void PlayIdle(VideoPlayer vp)
    {
        int rand = Random.Range(0, 3);
        switch(rand)
        {
            case 0:
                LoserVideo.clip = CurrentLoser.Idle_01;
                LoserVideo.Play();
                break;
            case 1:
                LoserVideo.clip = CurrentLoser.Idle_02;
                LoserVideo.Play();
                break;
            case 2:
                LoserVideo.clip = CurrentLoser.Idle_03;
                LoserVideo.Play();
                break;
        }
    }

    private void OnWrongSubmit()
	{
        if (CurrentSelected.WasSubmitted)
        {
            Debug.Log("I said Nope");
            LoserVideo.clip = CurrentLoser.Repeat;
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
                        LoserVideo.clip = CurrentLoser.TooBig;
					}
                    else
					{
                        LoserVideo.clip = CurrentLoser.TooSmall;
					}
                    break;
                case "colour":
                    LoserVideo.clip = CurrentLoser.ColourClips[(int)currentDef.Colour];
                    break;
                case "category":
                    LoserVideo.clip = CurrentLoser.CategoryClips[(int)currentDef.Category];
                    break;
            }

            LoserVideo.Play();

            CurrentSelected.WasSubmitted = true;
        }
    }

    private IEnumerator GoToEndCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (LoserVideo.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene("Scene_End");
    }

    private IEnumerator NextLevelCoroutine()
	{
        yield return new WaitForSeconds(0.1f);
        while (LoserVideo.isPlaying)
		{
            yield return null;
		}

        Reset();
	}
}
