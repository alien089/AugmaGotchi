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
        private Dictionary<Stats, StatComponent> _xStatComponent = new Dictionary<Stats, StatComponent>();
    
        void Start()
        {
            GameManager.Instance.EventManager.Register(AugmaEventList.GIVE_AUGMA_TO_UI, GetAugma);
            StatComponent[] tmp = transform.GetComponentsInChildren<StatComponent>();
            
            foreach (StatComponent t in tmp)
            {
                _xStatComponent.Add(t.XStatsType, t);
            }
            
            GetComponent<Canvas>().enabled = false;
        }

        void Update()
        {
            if (!_xAugma) return;

            foreach (KeyValuePair<Stats, StatComponent> x in _xStatComponent)
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
