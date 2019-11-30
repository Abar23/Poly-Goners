using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDisplayEffects : MonoBehaviour
{

    [Serializable]
    public struct MagicDisplayPair
    {
        public string Name;
        public GameObject Object;
    }

    [SerializeField] private List<MagicDisplayPair> m_DisplayMatch;

    private Dictionary<string, GameObject> displayMatch;

    void Awake()
    {
        displayMatch = new Dictionary<string, GameObject>();
        foreach (MagicDisplayPair pair in m_DisplayMatch)
        {
            displayMatch.Add(pair.Name, pair.Object);
        }
    }

    public void DisplayEffect(string name)
    {
        if (name == null)
            return;
        if (displayMatch.ContainsKey(name))
        {
            displayMatch[name].SetActive(true);
            displayMatch[name].GetComponent<ParticleSystem>().Play();
        }
    }

    public void ResetEffect(string name)
    {
        if (name == null)
            return;
        if (displayMatch.ContainsKey(name))
        {
            displayMatch[name].GetComponent<ParticleSystem>().Stop();
        }
    }
}
