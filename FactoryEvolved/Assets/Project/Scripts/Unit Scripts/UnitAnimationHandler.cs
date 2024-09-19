using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitAnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        private UnitMovement _unitMovement;
        [SerializeField] public string currentAnimation;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _unitMovement = GetComponent<UnitMovement>();
            
            PlayAnimation("Idle");
        }

        public void PlayAnimation(string animationName)
        {
            foreach (var param in _animator.parameters)
            {
                _animator.SetBool(param.name, false);
            }
            _animator.SetBool(animationName, true);
            currentAnimation = animationName;
        }

        public void TriggerAnimation(string animationName)
        {
            _animator.SetTrigger(animationName);
            currentAnimation = animationName;
        }
    }
}
