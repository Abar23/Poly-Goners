using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "ScriptableObjects/AlignmentConfig", order = 1)]
public class AlignmentConfig : ScriptableObject
{

    public string Name;
    public EntityType Type;
    public int MaxHealth;

}

public enum EntityType
{
    PlayerOne = 0x0,
    PlayerTwo = 0x1,
    Enemy = 0x2,
}
