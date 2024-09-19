using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    [Serializable]
    public abstract class State
    {
        public abstract void Enter();

        public abstract void Process();

        public abstract void Exit();
    }
}
