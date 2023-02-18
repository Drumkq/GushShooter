using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class WeaponBase : MonoBehaviour, IWeapon, IPickable
    {
        private BoxCollider2D _collider2D;

        public bool CanPickUp { get; private set; } = true;
        
        protected WeaponBase(float damage)
        {
            Damage = damage;
        }
        
        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _collider2D.isTrigger = true;
        }

        public float Damage { get; }

        protected abstract IEnumerable<IDamageable> FindDamageables();

        public abstract void Attack(IDamageable parent);

        public virtual void PutDown()
        {
            CanPickUp = true;
        }
        
        public virtual void PickUp()
        {
            CanPickUp = false;
        }
    }
}