using UnityEngine;

namespace CombatGame.Commons.Weapons
{
    public class WeaponSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponInUse;
        [SerializeField] private GameObject _holsteredWeapon;

        public void GetWeapon()
        {
            if (_holsteredWeapon == null || _weaponInUse == null) return;

            _weaponInUse.SetActive(true);
            _holsteredWeapon.SetActive(false);
        }

        public void SaveWeapon()
        {
            if (_holsteredWeapon == null || _weaponInUse == null) return;

            _weaponInUse.SetActive(false);
            _holsteredWeapon.SetActive(true);
        }
    }
}