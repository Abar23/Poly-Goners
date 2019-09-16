using UnityEngine;

[CreateAssetMenu(fileName = "Damageable Data", menuName = "ScriptableObjects/DamageableConfig", order = 1)]
public class DamageableConfig : ScriptableObject
{

    public int MaxHealth;
    public int StartingHealth;
    public Alignment Alignment;

}

public enum Alignment
{
    PlayerOne = 0x0,
    PlayerTwo = 0x1,
    Neutral = 0x2,
    Enemy = 0x3,
}
