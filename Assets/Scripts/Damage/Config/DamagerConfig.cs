using UnityEngine;

[CreateAssetMenu(fileName = "Damager Data", menuName = "ScriptableObjects/DamagerConfig", order = 1)]
public class DamagerConfig : ScriptableObject
{

    public int Damage;
    public Alignment Alignment;

}
