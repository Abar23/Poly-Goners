using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    
    [SerializeField] private int m_Number_of_Coins = 10;

    [SerializeField] private GameObject m_CoinExample;

    [SerializeField] private float m_VerticalForce = 10f;

    [SerializeField] private float m_VerticalVariance = 1f;

    [SerializeField] private float m_HorizontalMin = 4f;

    [SerializeField] private float m_HorizontalMax = 6f;

    [SerializeField] private float m_SpawnInterval = 1f;

    [SerializeField] private float m_SpawnAngle = 60f;

    private const int k_Iterations = 10;

    public void StartSpawnCoin()
    {
        StartCoroutine(SpawnCoin());
    }

    IEnumerator SpawnCoin()
    {
        for (int i = 0; i < m_Number_of_Coins; i++)
        {
            GameObject newCoin = GameObject.Instantiate(m_CoinExample, transform);
            newCoin.SetActive(true);
            Rigidbody rigidbody = newCoin.GetComponent<Rigidbody>();
            StartCoroutine(CoinMovement(rigidbody, RandomForce()));
            yield return new WaitForSeconds(m_SpawnInterval);
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
        float angle = Random.Range(-m_SpawnAngle, m_SpawnAngle) * 2f * Mathf.PI / 360f;
        float radius = Random.Range(m_HorizontalMin, m_HorizontalMax);
        return new Vector3(radius * Mathf.Cos(angle), RandomVertical(), radius * Mathf.Sin(angle));
    }

    float RandomVertical()
    {
        return Random.Range(m_VerticalForce - m_VerticalVariance, m_VerticalForce + m_VerticalVariance);
    }

}
