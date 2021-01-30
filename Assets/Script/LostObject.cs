using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostObject : MonoBehaviour
{
    private LostObjectDefinitions.LostObjectDefinition m_definition;

    public event Action<LostObject> OnSelected = null;

    public bool correctObject = false;
    public bool alreadySubmitted = false;



    [SerializeField]
    private Image m_image;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        if (OnSelected != null)
        {
            OnSelected.Invoke(this);
        }
    }

    internal void Initialise(LostObjectDefinitions.LostObjectDefinition def)
    {
        m_definition = def;
        m_image.sprite = def.Sprite;
    }

    public void Select()
    {
        Debug.Log("I'm selected");
    }

    public void Deselect()
    {
        Debug.Log("I'm deselected");
    }
}
