using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class LoadObjectsFromDefinitions : MonoBehaviour
{
    [SerializeField]
    private List<LostObject> m_lostObjs;
    public List<LostObject> LostObjects => m_lostObjs;

    [SerializeField]
    private LostObjectDefinitions m_definitions;

    [SerializeField]
    private LostObject m_lostObjectPrefab;

    public void SpawnObjects()
    {
        foreach(var obj in m_lostObjs.ToList())
        {
            DestroyImmediate(obj.gameObject);
            m_lostObjs.Remove(obj);
        }
        foreach (var def in m_definitions)
        {
            LostObject lostObj = GameObject.Instantiate(m_lostObjectPrefab, transform);
            lostObj.Initialise(def);
            m_lostObjs.Add(lostObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
