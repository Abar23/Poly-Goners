using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBox : MonoBehaviour
{
    public List<GameObject> Characters;
    public List<Transform> Hands;
    public List<bool> IsSmall;

    private List<List<GameObject>> subLists;
    private int charIndex = 0;
    private int subListIndex = 0;

    private WeaponManager weapons;
    private readonly System.Random rnd = new System.Random();

    private void Start()
    {
        subLists = new List<List<GameObject>>();
        weapons = GetComponentInChildren<WeaponManager>();

        foreach (GameObject character in Characters)
        {
            List<GameObject> subList = new List<GameObject>();

            int numChildren = character.transform.childCount;
            for (int i = 1; i < numChildren; i++)
            {
                subList.Add(character.transform.GetChild(i).gameObject);
            }

            subLists.Add(subList);
        }

        (subLists[charIndex])[subListIndex].SetActive(false);
        Characters[charIndex].SetActive(false);

        charIndex = rnd.Next(Characters.Count);
        Characters[charIndex].SetActive(true);
        if (IsSmall[charIndex] == true)
        {
            weapons.gameObject.transform.localScale = new Vector3(100, 100, 100);
        }
        else
        {
            weapons.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        subListIndex = rnd.Next((subLists[charIndex]).Count);
        (subLists[charIndex])[subListIndex].SetActive(true);
        weapons.transform.SetParent(Hands[charIndex], false);

    }

    public void NextCharacter()
    {
        (subLists[charIndex])[subListIndex].SetActive(false);
        subListIndex++;

        if (subListIndex >= (subLists[charIndex]).Count)
        {
            subListIndex = 0;
            Characters[charIndex].SetActive(false);
            charIndex++;
        }

        if (charIndex >= Characters.Count)
        {
            charIndex = 0;
        }

        if (Characters[charIndex].activeSelf == false)
        {
            Characters[charIndex].SetActive(true);
            if (IsSmall[charIndex] == true)
            {
                weapons.gameObject.transform.localScale = new Vector3(100, 100, 100);
            }
            else
            {
                weapons.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
        (subLists[charIndex])[subListIndex].SetActive(true);
        weapons.transform.SetParent(Hands[charIndex], false);
    }

}
