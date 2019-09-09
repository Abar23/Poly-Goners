using UnityEngine;

[CreateAssetMenu(fileName = "Damagable Data", menuName = "ScriptableObjects/DamagableConfig", order = 1)]
public class DamagableConfig : ScriptableObject
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
