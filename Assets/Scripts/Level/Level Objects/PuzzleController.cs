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

    private const float k_ResetDelay = 0.5f;

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
        
        if (CheckSequence(triggerSequence.Count))
        {
            if (triggerSequence.Count == m_Runes.Count && OnPuzzleFinished != null)
            {
                OnPuzzleFinished.Invoke();
            }
        }
        else
        {
            triggerSequence = new List<RunePillar>();
            StartCoroutine(ResetAllPillar());
            if (OnPuzzleFailed != null)
            {
                OnPuzzleFailed.Invoke();
            }
        }
    }

    private bool CheckSequence(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (triggerSequence[i] != m_Runes[i])
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator ResetAllPillar()
    {
        yield return new WaitForSeconds(k_ResetDelay);
        foreach (RunePillar pillar in m_Runes)
        {
            pillar.ResetActive();
        }
    }

}
