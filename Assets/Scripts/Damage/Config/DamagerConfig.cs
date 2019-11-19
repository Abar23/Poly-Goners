using UnityEngine;

public class DamagerConfig : ScriptableObject
{

    public enum DamageType
    {
        Physical = 0x0,
        MagicFire = 0x1,
        MagicPoison = 0x2,
        MagicLightning = 0x3,
    }

    public DamageType Type;

    public int Damage;

    public string Tier;

}
