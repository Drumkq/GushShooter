namespace Weapon
{
    public interface IWeapon
    {
        public float Damage { get; }
        public void Attack(IDamageable parent);
    }
}