using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Melee
{
    public abstract class MeleeWeaponBase : WeaponBase
    {
        [SerializeField] protected Transform attackPivot;

        protected MeleeWeaponBase(float damage, float attackWidth, float attackLength) : base(damage)
        {
            AttackWidth = attackWidth;
            AttackLength = attackLength;
        }

        public float AttackWidth { get; }
        public float AttackLength { get; }

        protected override IEnumerable<IDamageable> FindDamageables()
        {
            var cols = new Collider2D[]{};
            Physics2D.OverlapBoxNonAlloc(attackPivot.position, new Vector2(AttackWidth, AttackLength), 0f, cols);
            
            foreach (var col in cols)
            {
                var target = col.GetComponent<IDamageable>();

                if (target != null)
                {
                    yield return target;
                }
            }
        }
    }
}