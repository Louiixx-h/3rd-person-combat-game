using UnityEngine;

namespace CombatGame.Commons.Weapons
{
    public class WeaponSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponInUse;
        [SerializeField] private GameObject _savedWeapon;

        public void Use()
        {
            if (_savedWeapon == null || _weaponInUse == null) return;

            _weaponInUse.SetActive(true);
            _savedWeapon.SetActive(false);
        }

        public void Save()
        {
            if (_savedWeapon == null || _weaponInUse == null) return;

            _weaponInUse.SetActive(false);
            _savedWeapon.SetActive(true);
        }
    }
}