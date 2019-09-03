using CSE5912;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealth : MonoBehaviour
{
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int health = (int) GameConstants.Instance.PlayerHealth;
        healthText.text = "Health = " + health.ToString();
    }
}
