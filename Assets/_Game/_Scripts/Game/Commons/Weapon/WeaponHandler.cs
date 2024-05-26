using UnityEngine;

namespace CombatGame.Commons.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponLogic;

        public void EnableWeaponCollider()
        {
            _weaponLogic.SetActive(true);
        }

        public void DisableWeaponCollider()
        {
            _weaponLogic.SetActive(false);
        }
    }

}