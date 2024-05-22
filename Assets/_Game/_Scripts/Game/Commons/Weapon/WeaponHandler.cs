using UnityEngine;

namespace CombatGame.Commons.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponLogic;

        public void EnableWeapon()
        {
            _weaponLogic.SetActive(true);
        }

        public void DisableWeapon()
        {
            _weaponLogic.SetActive(false);
        }
    }

}