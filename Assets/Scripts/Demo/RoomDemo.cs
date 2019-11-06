using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDemo : MonoBehaviour
{
    [SerializeField] List<GameObject> m_Rooms;
    [SerializeField] float m_RotationSpeed;
    private int index;
    private float k_PresentTime = 12f;
    private float k_BlankTime = 2f;

    void Start()
    {
        index = 0;
        StartCoroutine(PresentRoom());
    }

    IEnumerator PresentRoom()
    {
        m_Rooms[index].SetActive(true);
        float elapse = 0f;
        while (true)
        {
            float deltaTime = Time.deltaTime;
            m_Rooms[index].transform.Rotate(Vector3.up * m_RotationSpeed * deltaTime);
            elapse += deltaTime;
            if (elapse >= k_PresentTime)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        m_Rooms[index].SetActive(false);
        index++;
        if (index < m_Rooms.Count)
        {
            yield return new WaitForSeconds(k_BlankTime);
            StartCoroutine(PresentRoom());
        }
    }
}
