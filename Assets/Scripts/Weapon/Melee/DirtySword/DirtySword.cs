namespace Weapon.Melee.DirtySword
{
    public class DirtySword : MeleeWeaponBase
    {
        public DirtySword() : base(12.5f, 1.5f, 2f)
        {
            
        }

        public override void Attack(IDamageable parent)
        {
            var targets = FindDamageables();

            foreach (var target in targets)
            {
                target.Damage(this, parent);
            }
        }
    }
}