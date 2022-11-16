using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
   [SerializeField] private GameObject _weaponLogic;

   void EnableWeapon()
   {
        _weaponLogic.SetActive(true);
   }

   void DisableWeapon()
   {
        _weaponLogic.SetActive(false);
   }
}
