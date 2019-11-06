using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Serializable]
    public struct Item
    {
        public GameObject Object;
        public int Count;
        public float SpawnInterval;
        [Range(0, 1)]
        public float SpawnChance;
    }

    [SerializeField] private List<Item> m_Items;

    [SerializeField] private float m_VerticalForce = 10f;

    [SerializeField] private float m_VerticalVariance = 1f;

    [SerializeField] private float m_HorizontalMin = 4f;

    [SerializeField] private float m_HorizontalMax = 6f;

    [SerializeField] private float m_SpawnAngle = 60f;

    private const int k_Iterations = 10;

    void Start()
    {
        
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnController());
    }

    IEnumerator SpawnController()
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            StartCoroutine(SpawnCoin(i));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SpawnCoin(int i)
    {
        for (int j = 0; j < m_Items[i].Count; j++)
        {
            if (UnityEngine.Random.Range(0f, 1f) < m_Items[i].SpawnChance)
            {
                GameObject newObj = GameObject.Instantiate(m_Items[i].Object, transform);
                newObj.SetActive(true);
                Rigidbody rigidbody = newObj.GetComponent<Rigidbody>();
                StartCoroutine(CoinMovement(rigidbody, RandomForce()));
                yield return new WaitForSeconds(m_Items[i].SpawnInterval);
            }
        }
    }

    IEnumerator CoinMovement(Rigidbody rigidbody, Vector3 force, int iterations = k_Iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            if (rigidbody == null)
            {
                break;
            }
            rigidbody.AddRelativeForce(force, ForceMode.Force);
            yield return new WaitForFixedUpdate();
        }
    }

    Vector3 RandomForce()
    {
        float angle = UnityEngine.Random.Range(-m_SpawnAngle, m_SpawnAngle) * 2f * Mathf.PI / 360f;
        float radius = UnityEngine.Random.Range(m_HorizontalMin, m_HorizontalMax);
        return new Vector3(radius * Mathf.Cos(angle), RandomVertical(), radius * Mathf.Sin(angle));
    }

    float RandomVertical()
    {
        return UnityEngine.Random.Range(m_VerticalForce - m_VerticalVariance, m_VerticalForce + m_VerticalVariance);
    }

}
