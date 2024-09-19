using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class HealthScript : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] public bool isDead;
        [SerializeField] private StateController _stateController;

        public void TakeDamage(float dmg)
        {
            print("Taking damage");
            health -= dmg;
            _stateController.SwitchToIdle();
            ValidateStats();
            StartCoroutine(IdleCooldown());
        }

        private void ValidateStats()
        {
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            isDead = true;
            yield return new WaitForSeconds(.2f);
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        private IEnumerator IdleCooldown()
        {
            yield return new WaitForSeconds(2);
            _stateController.SwitchToWander();
        }
    }
}
