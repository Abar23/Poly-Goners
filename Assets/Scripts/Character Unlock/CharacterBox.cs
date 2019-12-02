using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBox : MonoBehaviour
{
    public IconManager CharacterIcon;
    public List<GameObject> Characters;
    public List<Transform> RightHands;
    public List<Transform> LeftHands;
    public List<bool> IsSmall;

    private List<List<GameObject>> subLists;
    private int charIndex = 0;
    private int subListIndex = 0;

    private WeaponManager weapons;
    private MagicDisplayEffects displayEffects;
    private readonly System.Random rnd = new System.Random();

    private void Awake()
    {
        subLists = new List<List<GameObject>>();
        weapons = GetComponentInChildren<WeaponManager>();
        displayEffects = GetComponentInChildren<MagicDisplayEffects>();

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
            weapons.gameObject.transform.localScale = new Vector3(100, 100, 100);
        else
            weapons.gameObject.transform.localScale = new Vector3(1, 1, 1);

        subListIndex = rnd.Next((subLists[charIndex]).Count);
        (subLists[charIndex])[subListIndex].SetActive(true);
        weapons.transform.SetParent(RightHands[charIndex], false);
        displayEffects.transform.SetParent(LeftHands[charIndex], false);

        SetIcon((subLists[charIndex])[subListIndex]);
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
        weapons.transform.SetParent(RightHands[charIndex], false);
        displayEffects.transform.SetParent(LeftHands[charIndex], false);
    }

    public void SetCharacter(int cIndex, int sIndex)
    {
        (subLists[charIndex])[subListIndex].SetActive(false);
        Characters[charIndex].SetActive(false);

        charIndex = cIndex;
        subListIndex = sIndex;

        Characters[charIndex].SetActive(true);
        if (IsSmall[charIndex] == true)
            weapons.gameObject.transform.localScale = new Vector3(100, 100, 100);
        else
            weapons.gameObject.transform.localScale = new Vector3(1, 1, 1);

        (subLists[charIndex])[subListIndex].SetActive(true);
        weapons.transform.SetParent(RightHands[charIndex], false);
        displayEffects.transform.SetParent(LeftHands[charIndex], false);
    }

    public Renderer GetActiveCharacterRenderer()
    {
        return (subLists[charIndex])[subListIndex].GetComponent<Renderer>();
    }

    public void SetIcon(GameObject obj)
    {
        CharacterIcon.EnableIcon(obj);
        if (GetComponent<Player>().PlayerNumber != 1)
            CharacterIcon.FlipIcon();
    }
}
