using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(WeaponHandler))]
    public class Character : MonoBehaviour, IDamageable
    {
        [Header("Movement")]
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float velPower;

        [Header("Damageable")] 
        [SerializeField] private float maximumHealth;

        [Header("Camera following")]
        [SerializeField] private new Camera camera;
        
        private CharacterActions _input;
        private Rigidbody2D _rb;
        private WeaponHandler _weaponHandler;
        private Collider2D _enteredCollider;

        public float Health { get; private set; }
        public bool IsMove { get; private set; }
        public IWeapon TargetWeapon { get; private set; }

        private void Awake()
        {
            _input = new CharacterActions();
            
            _rb = GetComponent<Rigidbody2D>();

            _weaponHandler = GetComponent<WeaponHandler>();
            
            Health = maximumHealth;
        }

        private void OnEnable()
        {
            _input.Gameplay.PickUp.performed += PickUp;
            _input.Gameplay.PutDown.performed += PutDown;
            
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Gameplay.PickUp.performed -= PickUp;
            _input.Gameplay.PutDown.performed -= PutDown;
            
            _input.Disable();
        }

        private void Update()
        {
            FlipCharacter();
        }

        private void FixedUpdate()
        {
            Move(_input.Movement.Move.ReadValue<Vector2>());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            _enteredCollider = col;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_enteredCollider == other)
            {
                _enteredCollider = null;
            }
        }

        private void PutDown(InputAction.CallbackContext ctx)
        {
            _weaponHandler.DropWeapon();
        }
        
        private void PickUp(InputAction.CallbackContext ctx)
        {
            if (_enteredCollider == null)
            {
                return;
            }
            
            var pickable = _enteredCollider.GetComponent<IPickable>();
            
            if (pickable == null)
            {
                return;
            }

            if (!pickable.CanPickUp)
            {
                return;
            }

            switch (pickable)
            {
                case WeaponBase weapon:
                    SetWeapon(_enteredCollider.gameObject);
                    weapon.PickUp();
                    break;
            }
        }

        private void Move(Vector2 input)
        {
            IsMove = input != Vector2.zero;
            
            var targetSpeed = input * speed;

            var speedDif = targetSpeed - _rb.velocity;

            var accelRateX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? acceleration : deceleration;
            var accelRateY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? acceleration : deceleration;

            var movement = new Vector2(
                Mathf.Pow(Mathf.Abs(speedDif.x) * accelRateX, velPower) * Mathf.Sign(speedDif.x),
                Mathf.Pow(Mathf.Abs(speedDif.y) * accelRateY, velPower) * Mathf.Sign(speedDif.y)
            );
            
            _rb.AddForce(movement);
        }

        private void FlipCharacter()
        {
            var pos = camera.ScreenToWorldPoint(_input.Mouse.Move.ReadValue<Vector2>());

            var scale = transform.localScale;
            if (pos.x > transform.position.x)
            {
                scale.x = 1;
            }
            else
            {
                scale.x = -1;
            }

            transform.localScale = scale;
        }

        public void Die()
        {
            // TODO: Implement die
        }
        
        public void SetWeapon(GameObject weapon)
        {
            TargetWeapon = weapon.GetComponent<WeaponBase>();
            _weaponHandler.UpdateWeapon(weapon);
        }

        public void Damage(IWeapon weapon, IDamageable damager)
        {
            Health -= weapon.Damage;

            if (Health < 0)
            {
                Die();
            }
        }

        public void Attack()
        {
            TargetWeapon?.Attack(this);
        }
    }
}
