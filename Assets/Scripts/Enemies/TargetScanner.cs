using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetScanner
{
    [SerializeField] private float m_HeightOffset = 0.0f;
    [SerializeField] private float m_DetectionRadius = 10;
    [SerializeField] [Range(0.0f, 360.0f)] public float m_DetectionAngle = 270;
    [SerializeField] private float m_MaxHeightDifference = 1.0f;
    [SerializeField] private LayerMask m_ViewBlockerLayerMask;

    public bool Detect(Transform detector, Transform player, bool useHeightDifference = true)
    {
        if (player == null)
            return false;

        Vector3 eyePos = detector.position + Vector3.up * m_HeightOffset;
        Vector3 toPlayer = player.transform.position - eyePos;
        Vector3 toPlayerTop = player.transform.position + Vector3.up * 1.5f - eyePos;

        if (useHeightDifference && Mathf.Abs(toPlayer.y + m_HeightOffset) > m_MaxHeightDifference)
        {
            return false;
        }

        Vector3 toPlayerFlat = toPlayer;
        toPlayerFlat.y = 0;

        if (toPlayerFlat.sqrMagnitude <= m_DetectionRadius * m_DetectionRadius)
        {
            if (Vector3.Dot(toPlayerFlat.normalized, detector.forward) >
                Mathf.Cos(m_DetectionAngle * 0.5f * Mathf.Deg2Rad))
            {

                bool canSee = false;

                Debug.DrawRay(eyePos, toPlayer, Color.blue);
                Debug.DrawRay(eyePos, toPlayerTop, Color.blue);

                canSee |= !Physics.Raycast(eyePos, toPlayer.normalized, m_DetectionRadius,
                    m_ViewBlockerLayerMask, QueryTriggerInteraction.Ignore);

                canSee |= !Physics.Raycast(eyePos, toPlayerTop.normalized, toPlayerTop.magnitude,
                    m_ViewBlockerLayerMask, QueryTriggerInteraction.Ignore);

                if (canSee)
                    return true;
            }
        }

        return false;
    }


#if UNITY_EDITOR

    public void EditorGizmo(Transform transform)
    {
        Color c = new Color(0, 0, 0.7f, 0.4f);

        UnityEditor.Handles.color = c;
        Vector3 rotatedForward = Quaternion.Euler(0, -m_DetectionAngle * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, m_DetectionAngle, m_DetectionRadius);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * m_HeightOffset, 0.2f);
    }

#endif
}