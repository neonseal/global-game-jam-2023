using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;
using Audio;

namespace Generator {
    public enum SoundEffects {
        DecreaseState,
        IncreaseState,
        GeneratorIdle
    }

    public class GeneratorController : MonoBehaviour {
        private static GeneratorHUD generatorHUD;

        [Header("Audio Controller")]
        private static AudioController audioController;
        private static AudioSource[] soundEffects;

        [Header("Consumption Costs")]
        [SerializeField] private float minConsumptionRate = 100.0f;
        [SerializeField] private float maxConsumptionRate = 1000.0f;
        private float lastEnergyRate = 0.0f;
        [SerializeField] private float energyConsumptionRate;
        private float lastWaterRate = 0.0f;
        [SerializeField] private float waterConsumptionRate;
        private float lastOrganicRate = 0.0f;
        [SerializeField] private float organicConsumptionRate;

        [Header("Resource States")]
        // Dictionary <ResourceName, Active>
        private static bool[] resourceStates;
        private int failingCount;

        [Header("Generator Consumption Randomizer")]
        [SerializeField] private float generatorTimerReset = 15.0f;
        private float timer;

        private void Awake() {
            generatorHUD = gameObject.GetComponent<GeneratorHUD>();
            soundEffects = gameObject.GetComponents<AudioSource>();
            audioController = gameObject.GetComponentInChildren<AudioController>();

            timer = generatorTimerReset;

            energyConsumptionRate = 2.0f * minConsumptionRate;
            lastEnergyRate = energyConsumptionRate;

            waterConsumptionRate =  1.5f * minConsumptionRate;
            lastWaterRate = waterConsumptionRate;

            organicConsumptionRate = minConsumptionRate;
            lastOrganicRate = organicConsumptionRate;

            // Set initial resource states for Water, Energy, Organic
            resourceStates = new bool[3] { true, true, true };

            Debug.Log("SETTING ENERGY: " + energyConsumptionRate / maxConsumptionRate);
            generatorHUD.SetResourceState(ComponentType.Tree, waterConsumptionRate / maxConsumptionRate);
            generatorHUD.SetResourceState(ComponentType.Sunflower, energyConsumptionRate / maxConsumptionRate);
            generatorHUD.SetResourceState(ComponentType.Decomposer, organicConsumptionRate / maxConsumptionRate);
        }

        private void Update() {
            UpdateFailingCount();

            timer -= Time.deltaTime;

            if (timer <= 0.0f) {
                energyConsumptionRate = RandomizeConsumptionRate(ComponentType.Sunflower);
                waterConsumptionRate = RandomizeConsumptionRate(ComponentType.Tree);
                organicConsumptionRate = RandomizeConsumptionRate(ComponentType.Decomposer);

                timer = generatorTimerReset;
            }
        }

        private float RandomizeConsumptionRate(ComponentType type) {
            Debug.Log("RANDOMIZING RATE: " + type); ;
            float newRate = 0.0f;
            float floor = minConsumptionRate;
            // Randomly decide if we push high if last value was low
            // 2 out of 3 chance of adjusting higher
            bool adjustHigher = false;

            switch (type) {
                case ComponentType.Sunflower:
                    if (lastEnergyRate < maxConsumptionRate * 0.3) {
                        adjustHigher = Random.Range(0, 3) > 1;
                    }

                    if (adjustHigher) {
                        floor *= 2;
                    }

                    energyConsumptionRate = Random.Range(floor, maxConsumptionRate);
                    lastEnergyRate = energyConsumptionRate;

                    // Set Generator HUD to reflect intensity of consumption as a percentage of max rate
                    generatorHUD.SetResourceState(ComponentType.Sunflower, energyConsumptionRate / maxConsumptionRate);
                    break;
                case ComponentType.Tree:
                    if (lastWaterRate < maxConsumptionRate * 0.3) {
                        adjustHigher = Random.Range(0, 3) > 1;
                    }

                    if (adjustHigher) {
                        floor *= 2;
                    }

                    waterConsumptionRate = Random.Range(floor, maxConsumptionRate);
                    lastEnergyRate = waterConsumptionRate;

                    // Set Generator HUD to reflect intensity of consumption as a percentage of max rate
                    generatorHUD.SetResourceState(ComponentType.Tree, waterConsumptionRate / maxConsumptionRate);
                    break;
                case ComponentType.Decomposer:
                    if (lastOrganicRate < maxConsumptionRate * 0.3) {
                        adjustHigher = Random.Range(0, 3) > 1;
                    }

                    if (adjustHigher) {
                        floor *= 2;
                    }

                    organicConsumptionRate = Random.Range(floor, maxConsumptionRate);
                    lastOrganicRate = organicConsumptionRate;

                    // Set Generator HUD to reflect intensity of consumption as a percentage of max rate
                    generatorHUD.SetResourceState(ComponentType.Decomposer, organicConsumptionRate / maxConsumptionRate);
                    break;
            }

            return newRate;
        }

        #region Resource State Management
        public void UpdateResourceState(ComponentType type, bool activeState) {
            switch (type) {
                case ComponentType.Tree:
                    resourceStates[0] = activeState;
                    break;
                case ComponentType.Sunflower:
                    resourceStates[1] = activeState;
                    break;
                case ComponentType.Decomposer:
                    resourceStates[2] = activeState;
                    break;
            }
            PlayStateChangeSoundEffect(activeState);
            UpdateFailingCount();

            if (failingCount == 3) {
                soundEffects[(int)SoundEffects.GeneratorIdle].Play();
                audioController.SwapMusicTracks(MusicTracks.GameOver);
                // Trigger game over
            } else {
                // Update Music
                audioController.SwapMusicTracks((MusicTracks)failingCount);
            }
        }

        private void PlayStateChangeSoundEffect(bool activating) {
            AudioSource effect;
            int effectIndex;

            if (activating) {
                effectIndex = (int)SoundEffects.IncreaseState;
                effect = soundEffects[effectIndex] as AudioSource;
                effect.Play();
            } else {
                effectIndex = (int)SoundEffects.DecreaseState;
                effect = soundEffects[effectIndex] as AudioSource;
                effect.Play();
            }

        }

        private void UpdateFailingCount() {
            failingCount = resourceStates.Count(state => state == false);
        }
        #endregion

        #region Getters and Setters
        public float EnergyConsumptionRate {
            get { return energyConsumptionRate; }
            set { energyConsumptionRate = value; }
        }

        public float WaterConsumptionRate {
            get { return waterConsumptionRate; }
            set { waterConsumptionRate = value; }
        }

        public float OrganicConsumptionRate {
            get { return organicConsumptionRate; }
            set { organicConsumptionRate = value; }
        }

        public int FailingCount {
            get { return failingCount; }
        }

        #endregion
    }
}