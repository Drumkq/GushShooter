namespace Weapon
{
    public interface IDamageable
    {
        public float Health { get; }
        
        public void Damage(IWeapon weapon, IDamageable damager);
        
        public void Attack();

        public void Die();
    }
}