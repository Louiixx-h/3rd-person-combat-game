using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScritptable : ScriptableObject
{
    public enum WeaponCategoryEnum
    {
        NONE,
        HEAVY_WEAPON,
        LIGHT_WEAPON, 
        BOW,
        MAGIC,
        BOMB
    }

    public enum WeaponRarityEnum
    {
        NONE,
        COMMON,
        UNCOMMON,
        RARE,
        EPIC
    }

    public GameObject WeaponObject;
    public float BaseDamage;
    public float Level;
    public WeaponCategoryEnum Category;
    public WeaponRarityEnum Rarity;
}
