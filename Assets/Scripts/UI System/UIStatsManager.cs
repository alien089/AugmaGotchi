using System;
using System.Collections.Generic;
using Character_System;
using Enums;
using Managers;
using UnityEngine;

namespace UI_System
{
    public class UIStatsManager : MonoBehaviour
    {
        private AugmaManager _xAugma;
        private Dictionary<Stats, StatUIComponent> _xStatComponent = new Dictionary<Stats, StatUIComponent>();
    
        void Start()
        {
            GameManager.Instance.EventManager.Register(AugmaEventList.GIVE_AUGMA_TO_UI, GetAugma);
            StatUIComponent[] tmp = transform.GetComponentsInChildren<StatUIComponent>();
            
            foreach (StatUIComponent t in tmp)
            {
                _xStatComponent.Add(t.XStatsType, t);
            }
            
            GetComponent<Canvas>().enabled = false;
        }

        void Update()
        {
            if (!_xAugma) return;

            foreach (KeyValuePair<Stats, StatUIComponent> x in _xStatComponent)
            {
                x.Value.XProgressBar.fillAmount = _xAugma.FCurrentValuesStats[x.Key];
            }
        }

        private void GetAugma(object[] param)
        {
            _xAugma = (AugmaManager)param[0];
            GetComponent<Canvas>().enabled = true;
        }
    }
}
