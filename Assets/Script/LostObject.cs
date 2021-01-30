using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostObject : MonoBehaviour
{
    public event Action<LostObject> OnSelected = null;

    public bool correctObject = false;
    public bool alreadySubmitted = false;

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

    public void Select()
    {
        Debug.Log("I'm selected");
    }

    public void Deselect()
    {
        Debug.Log("I'm deselected");
    }
}
