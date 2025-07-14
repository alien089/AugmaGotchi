using System;
using System.Collections.Generic;
using Character_System;
using Enums;
using Managers;
using UnityEngine;

namespace UI_System
{
    // Manages UI stat components and updates progress bars based on entity stats.
    public class UIStatsManager : MonoBehaviour
    {
        private EntityManager _xEntity;
        private Dictionary<Stats, StatUIComponent> _xStatComponent = new Dictionary<Stats, StatUIComponent>();

        // Registers event, collects stat UI components and disables canvas initially.
        void Start()
        {
            GameManager.Instance.EventManager.Register(EntityEventList.GIVE_Entity_TO_UI, GetEntity);
            
            // Collect all StatUIComponents from children and map them by their Stats type
            StatUIComponent[] tmp = transform.GetComponentsInChildren<StatUIComponent>();
            foreach (StatUIComponent t in tmp)
            {
                _xStatComponent.Add(t.XStatsType, t);
            }
            
            // Disable canvas until an entity is assigned
            GetComponent<Canvas>().enabled = false;
        }

        // Updates each stat UI component's progress bar based on the entity's current stats.
        void Update()
        {
            if (!_xEntity) return;

            // Update all stat progress bars with current stat values from entity
            foreach (KeyValuePair<Stats, StatUIComponent> x in _xStatComponent)
            {
                x.Value.XProgressBar.fillAmount = _xEntity.FCurrentValuesStats[x.Key];
            }
        }

        // Assigns the entity to the UI manager and enables the canvas.
        private void GetEntity(object[] param)
        {
            _xEntity = (EntityManager)param[0];
            GetComponent<Canvas>().enabled = true;
        }

        // Unregisters the event listener on application quit.
        private void OnApplicationQuit()
        {
            GameManager.Instance.EventManager.Unregister(EntityEventList.GIVE_Entity_TO_UI, GetEntity);
        }
    }
}
