using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] List<AnimationClip> primaryAttackAnimation;
    [SerializeField] int animationLayer;
    [SerializeField] float attackStamina;
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 startRotation;

    private readonly System.Random rnd = new System.Random();

    private int j = 0;

    public AnimationClip GetPrimaryAttackAnimation()
    {
        int r = rnd.Next(primaryAttackAnimation.Count);
        Debug.Log(primaryAttackAnimation[r].name + " " + j);
        j++;
        return primaryAttackAnimation[r];
    }

    public int GetAnimationLayer()
    {
        return animationLayer;
    }

    public float GetPrimaryAttackStamina()
    {
        return attackStamina;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }

    public Vector3 GetStartRotation()
    {
        return startRotation;
    }
}
