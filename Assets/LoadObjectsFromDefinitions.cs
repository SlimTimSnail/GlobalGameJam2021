using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObjectsFromDefinitions : MonoBehaviour
{
    [SerializeField]
    private LostObjectDefinitions m_definitions;

    [SerializeField]
    private LostObject m_lostObjectPrefab;

    void Start()
    {
        foreach (var def in m_definitions)
        {
            LostObject lostObj = GameObject.Instantiate(m_lostObjectPrefab);
            lostObj.Initialise(def);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
