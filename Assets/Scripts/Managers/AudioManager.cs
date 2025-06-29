using System.Linq;
using Misc;
using UnityEngine;
using Enums;
using AudioType = Enums.AudioType;

namespace Managers
{
    [System.Serializable()]
    public class AudioDictionary : SerializableDictionaryBase<AudioType, string> { }
    
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSO _xBitePackage;
        [SerializeField] private AudioSO _xHappyPackage;
        [SerializeField] private AudioSO _xHungerPackage;
        [SerializeField] private AudioSO _xPurrPackage;
        [SerializeField] private AudioSO _xSadPackage;
        [SerializeField] private AudioSO _xScratchPackage;
        
        [SerializeField] public AudioDictionary AudioEvents = new AudioDictionary();
    
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_GIVEN, PlayBiteSound);
            GameManager.Instance.EventManager.Register(FoodEventList.IS_HUNGER, PlayHungerSound);
            GameManager.Instance.EventManager.Register(CaressEventList.AUDIO_CARESS, PlayPurrSound);
            GameManager.Instance.EventManager.Register(CaressEventList.AUDIO_CARESS, PlayScratchSound);
            GameManager.Instance.EventManager.Register(AugmaEventList.SAD_AUDIO, PlaySadSound);
            GameManager.Instance.EventManager.Register(AugmaEventList.HAPPY_AUDIO, PlayHappySound);
        }
        
        private void PlayBiteSound(object[] param)
        {
            int rnd = Random.Range(0, _xBitePackage.AudioClips.Length);
            
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.BITE], _xBitePackage.AudioClips[rnd]);
        } 
        
        private void PlayPurrSound(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.PURR], (bool)param[0], _xPurrPackage);
        } 
        
        private void PlayScratchSound(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.SCRATCH], (bool)param[0], _xScratchPackage);
        } 
        
        private void PlayHungerSound(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.HUNGER], (bool)param[0], _xHungerPackage);
        } 
        
        private void PlaySadSound(object[] param)
        {
            int rnd = Random.Range(0, _xSadPackage.AudioClips.Length);

            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.SAD], _xSadPackage.AudioClips[rnd]);
        } 
        
        private void PlayHappySound(object[] param)
        {
            int rnd = Random.Range(0, _xHappyPackage.AudioClips.Length);

            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.HAPPY], _xHappyPackage.AudioClips[rnd]);
        } 
    }
}
