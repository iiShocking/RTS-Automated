using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public interface ISelectable
    {
        void OnSelect();
        void OnDeselect();
        void OnHighlight();
        void OnUnhighlight();
        void DisplayUI();
        void HideUI();
    }
}
