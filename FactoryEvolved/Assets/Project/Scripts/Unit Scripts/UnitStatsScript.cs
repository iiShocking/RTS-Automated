using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class UnitStatsScript : MonoBehaviour
    {
        [SerializeField] private float hunger = 100;
        [SerializeField] private float thirst = 100;

        [SerializeField] private float hungerDecayRate;
        [SerializeField] private float thirstDecayRate;

        private UnitUIHandler _unitUIHandler;

        private void Start()
        {
            _unitUIHandler = GetComponent<UnitUIHandler>();
        }

        //USE TICKS WITH THIS
        public void DecreaseStats()
        {
            hunger -= hungerDecayRate;
            thirst -= thirstDecayRate;
            ValidateStats();
            _unitUIHandler.UpdateUnitRadials();
        }

        public void IncreaseHunger(float value)
        {
            hunger += value;
            ValidateStats();
        }

        public void IncreaseThirst(float value)
        {
            thirst += value;
            ValidateStats();
        }

        private void ValidateStats()
        {
            if (hunger > 100) hunger = 100;
            if (thirst > 100) thirst = 100;
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }

        public float GetHunger() => hunger;

        public float GetThirst() => thirst;
    }
}
