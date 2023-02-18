namespace Weapon
{
    public interface IPickable
    {
        public bool CanPickUp { get; }
        
        public void PickUp();
    }
}