using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostObject : MonoBehaviour
{
    public LostObjectDefinitions.LostObjectDefinition m_definition;
    public LostObjectDefinitions.LostObjectDefinition Definition => m_definition;

    public event Action<LostObject> OnSelected = null;

    public bool correctObject = false;

    [SerializeField]
    private bool m_submitted = false;
    public bool WasSubmitted { get => m_submitted; set => SetSubmittedState(value); }

    private void SetSubmittedState(bool value)
    {
        m_submitted = value;
    }

    [SerializeField]
    private Button m_button;

    [SerializeField]
    private Image m_image;



    // Start is called before the first frame update
    void Start()
    {
        m_image.alphaHitTestMinimumThreshold = 0.001f;
    }

    private void OnEnable()
    {
        OnSelected += GameScreen.Instance.OnObjectSelect;
    }

    private void OnDisable()
    {
        OnSelected -= GameScreen.Instance.OnObjectSelect;
    }

    public void OnClick()
    {
        OnSelected?.Invoke(this);
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
