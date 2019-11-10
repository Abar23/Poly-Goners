using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{

    [SerializeField] private List<RunePillar> m_Runes;
    [SerializeField] private UnityEvent OnPuzzleFinished;
    [SerializeField] private UnityEvent OnPuzzleFailed;
    private List<RunePillar> triggerSequence;

    void Start()
    {
        triggerSequence = new List<RunePillar>();
    }

    public void TriggerPillar(RunePillar pillar)
    {
        triggerSequence.Add(pillar);
        CheckPuzzle();
    }

    private void CheckPuzzle()
    {
        if (triggerSequence.Count == m_Runes.Count)
        {
            if (CheckSequence())
            {
                if (OnPuzzleFinished != null)
                {
                    OnPuzzleFinished.Invoke();
                }
            }
            else
            {
                foreach (RunePillar pillar in triggerSequence)
                {
                    pillar.ResetActive();
                }
                triggerSequence = new List<RunePillar>();
                if (OnPuzzleFailed != null)
                {
                    OnPuzzleFailed.Invoke();
                }
            }
        }
    }

    private bool CheckSequence()
    {
        for (int i = 0; i < triggerSequence.Count; i++)
        {
            if (triggerSequence[i] != m_Runes[i])
            {
                return false;
            }
        }
        return true;
    }

}
