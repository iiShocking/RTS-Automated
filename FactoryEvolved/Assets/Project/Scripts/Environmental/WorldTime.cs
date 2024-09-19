using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FactoryEvolved
{
    public class WorldTime : MonoBehaviour
    {
        [SerializeField] private int day;
        [SerializeField] private int hour;
        [SerializeField] private int minute;
        [SerializeField] private int second;

        [SerializeField] private TMP_Text timeText;

        private void Start()
        {
            TickManager.Instance.Subscribe(UpdateTime, 1);
        }

        private void UpdateTime()
        {
            second+=60;

            if (second >= 60)
            {
                second = 0;
                minute+=15;
                if (minute >= 60)
                {
                    minute = 0;
                    hour++;
                    if (hour >= 24)
                    {
                        hour = 0;
                        day++;
                    }
                }
            }

            //SS:MM:HH
            string secondText = "00";
            string minuteText = "";
            string hourText = "";
            switch (minute)
            {
                case 0:
                    minuteText = "00:";
                    break;
                default:
                    minuteText = minute + ":";
                    break;
            }

            if (hour < 10)
            {
                hourText = "0" + hour + ":";
            }
            else
            {
                hourText = hour +":";
            }
            timeText.text = hourText + minuteText + secondText;
            
        }
    }
}
