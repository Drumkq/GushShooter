using JetBrains.Annotations;
using UnityEngine;

namespace Weapon
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private Transform spawnPivot;

        [CanBeNull] private GameObject _physicalWeapon;
        
        public void UpdateWeapon(GameObject weapon)
        {
            if (_physicalWeapon != null)
            {
                DropWeapon();
            }
            
            _physicalWeapon = weapon;
            
            weapon.transform.parent = spawnPivot;
            
            weapon.transform.localPosition = Vector3.zero;
        }

        public void DropWeapon()
        {
            if (_physicalWeapon != null)
            {
                _physicalWeapon.transform.SetParent(null);
                _physicalWeapon.transform.position = transform.position;
                
                _physicalWeapon.GetComponent<WeaponBase>()?.PutDown();
            }
        }
    }
}