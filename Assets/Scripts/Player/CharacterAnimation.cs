using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator), typeof(Character))]
    public class CharacterAnimation : MonoBehaviour
    {
        private Character _character;
        private Animator _animator;
        
        private static readonly int Move = Animator.StringToHash("Move");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            _animator.SetBool(Move, _character.IsMove);
        }
    }
}