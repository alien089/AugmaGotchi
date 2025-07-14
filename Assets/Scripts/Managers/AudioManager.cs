using System.Linq;
using Misc;
using UnityEngine;
using Enums;
using AudioType = Enums.AudioType;

namespace Managers
{
    [System.Serializable()]
    // Serializable dictionary mapping audio types to string keys
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

        // Registers audio-related event listeners on start
        void Start()
        {
            GameManager.Instance.EventManager.Register(FoodEventList.FOOD_GIVEN, PlayBiteSound);
            GameManager.Instance.EventManager.Register(FoodEventList.IS_HUNGER, PlayHungerSound);
            GameManager.Instance.EventManager.Register(CaressEventList.AUDIO_CARESS, PlayPurrSound);
            GameManager.Instance.EventManager.Register(CaressEventList.AUDIO_CARESS, PlayScratchSound);
            GameManager.Instance.EventManager.Register(EntityEventList.SAD_AUDIO, PlaySadSound);
            GameManager.Instance.EventManager.Register(EntityEventList.HAPPY_AUDIO, PlayHappySound);
        }

        // Triggers a random bite sound clip
        private void PlayBiteSound(object[] param)
        {
            int rnd = Random.Range(0, _xBitePackage.AudioClips.Length);

            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.BITE], _xBitePackage.AudioClips[rnd]);
        }

        // Triggers purr sound event with given parameter
        private void PlayPurrSound(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.PURR], (bool)param[0], _xPurrPackage);
        }

        // Triggers scratch sound event with given parameter
        private void PlayScratchSound(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.SCRATCH], (bool)param[0], _xScratchPackage);
        }

        // Triggers hunger sound event with given parameter
        private void PlayHungerSound(object[] param)
        {
            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.HUNGER], (bool)param[0], _xHungerPackage);
        }

        // Triggers a random sad sound clip
        private void PlaySadSound(object[] param)
        {
            int rnd = Random.Range(0, _xSadPackage.AudioClips.Length);

            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.SAD], _xSadPackage.AudioClips[rnd]);
        }

        // Triggers a random happy sound clip
        private void PlayHappySound(object[] param)
        {
            int rnd = Random.Range(0, _xHappyPackage.AudioClips.Length);

            GameManager.Instance.EventManager.TriggerEvent(AudioEvents[AudioType.HAPPY], _xHappyPackage.AudioClips[rnd]);
        }
    }
}
