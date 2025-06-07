using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI_System
{
    public class StatUIComponent : MonoBehaviour
    {
        [SerializeField] private Stats _xStatsType;
        private Image _xProgressBar;

        public Image XProgressBar { get => _xProgressBar; }
        public Stats XStatsType { get => _xStatsType; }

        // Start is called before the first frame update
        void Start()
        {
            _xProgressBar = GetComponentInChildren<Image>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
