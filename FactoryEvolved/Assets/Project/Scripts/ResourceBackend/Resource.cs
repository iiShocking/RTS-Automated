using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    [Serializable]
    public class Resource
    {
        [SerializeField] private string _name;
        [SerializeField] private int _amount;

        public Resource(string name)
        {
            _name = name;
            _amount = 0;
        }
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }
    }
}
