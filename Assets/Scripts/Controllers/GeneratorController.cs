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
        private int lastEnergyRate = 0;
        [SerializeField] private int energyConsumptionRate;
        private float lastWaterRate = 0;
        [SerializeField] private int waterConsumptionRate;
        private float lastOrganicRate = 0;
        [SerializeField] private int organicConsumptionRate;

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

            energyConsumptionRate = (int)(2.0f * minConsumptionRate);
            lastEnergyRate = energyConsumptionRate;

            waterConsumptionRate = (int)(1.5f * minConsumptionRate);
            lastWaterRate = waterConsumptionRate;

            organicConsumptionRate = (int)minConsumptionRate;
            lastOrganicRate = organicConsumptionRate;

            // Set initial resource states for Water, Energy, Organic
            resourceStates = new bool[3] { true, true, true };

            generatorHUD.SetResourceState(ComponentType.Tree, waterConsumptionRate / maxConsumptionRate);
            generatorHUD.SetResourceState(ComponentType.Sunflower, energyConsumptionRate / maxConsumptionRate);
            generatorHUD.SetResourceState(ComponentType.Decomposer, organicConsumptionRate / maxConsumptionRate);
        }

        private void Update() {
            UpdateFailingCount();

            timer -= Time.deltaTime;

            if (timer <= 0.0f) {
                if (resourceStates[0]) {
                    RandomizeConsumptionRate(ComponentType.Tree);
                } else {
                    generatorHUD.SetResourceState(ComponentType.Tree, 0);
                }

                if (resourceStates[1]) {
                    RandomizeConsumptionRate(ComponentType.Sunflower);
                } else {
                    generatorHUD.SetResourceState(ComponentType.Sunflower, 0);
                }

                if (resourceStates[2]) {
                    RandomizeConsumptionRate(ComponentType.Decomposer);
                } else {
                    generatorHUD.SetResourceState(ComponentType.Decomposer, 0);
                }

                timer = generatorTimerReset;
            }
        }

        private void RandomizeConsumptionRate(ComponentType type) {

            float floor = minConsumptionRate;
            // Randomly decide if we push high if last value was low
            // 2 out of 3 chance of adjusting higher
            bool adjustHigher = false;

            switch (type) {
                case ComponentType.Tree:
                    if (lastWaterRate < maxConsumptionRate * 0.3) {
                        adjustHigher = Random.Range(0, 3) > 1;
                    }

                    if (adjustHigher) {
                        floor *= 2;
                    }

                    waterConsumptionRate = (int)Random.Range(floor, maxConsumptionRate);
                    lastEnergyRate = waterConsumptionRate;

                    // Set Generator HUD to reflect intensity of consumption as a percentage of max rate
                    generatorHUD.SetResourceState(ComponentType.Tree, waterConsumptionRate / maxConsumptionRate);
                    break;
                case ComponentType.Sunflower:
                    if (lastEnergyRate < maxConsumptionRate * 0.3) {
                        adjustHigher = Random.Range(0, 3) > 1;
                    }

                    if (adjustHigher) {
                        floor *= 2;
                    }

                    energyConsumptionRate = (int)Random.Range(floor, maxConsumptionRate);
                    lastEnergyRate = energyConsumptionRate;

                    // Set Generator HUD to reflect intensity of consumption as a percentage of max rate
                    generatorHUD.SetResourceState(ComponentType.Sunflower, energyConsumptionRate / maxConsumptionRate);
                    break;
                case ComponentType.Decomposer:
                    if (lastOrganicRate < maxConsumptionRate * 0.3) {
                        adjustHigher = Random.Range(0, 3) > 1;
                    }

                    if (adjustHigher) {
                        floor *= 2;
                    }

                    organicConsumptionRate = (int)Random.Range(floor, maxConsumptionRate);
                    lastOrganicRate = organicConsumptionRate;

                    // Set Generator HUD to reflect intensity of consumption as a percentage of max rate
                    generatorHUD.SetResourceState(ComponentType.Decomposer, organicConsumptionRate / maxConsumptionRate);
                    break;
            }
        }

        #region Resource State Management
        public void UpdateResourceState(ComponentType type, bool activeState) {
            switch (type) {
                case ComponentType.Tree:
                    resourceStates[0] = activeState;
                    if (activeState) {
                        RandomizeConsumptionRate(ComponentType.Tree);
                    } else {
                        generatorHUD.SetResourceState(ComponentType.Tree, 0);
                    }
                    break;
                case ComponentType.Sunflower:
                    resourceStates[1] = activeState;
                    if (activeState) {
                        RandomizeConsumptionRate(ComponentType.Sunflower);
                    } else {
                        generatorHUD.SetResourceState(ComponentType.Sunflower, 0);
                    }
                    break;
                case ComponentType.Decomposer:
                    resourceStates[2] = activeState;
                    if (activeState) {
                        RandomizeConsumptionRate(ComponentType.Decomposer);
                    } else {
                        generatorHUD.SetResourceState(ComponentType.Decomposer, 0);
                    }
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
        public int EnergyConsumptionRate {
            get { return energyConsumptionRate; }
            set { energyConsumptionRate = value; }
        }

        public int WaterConsumptionRate {
            get { return waterConsumptionRate; }
            set { waterConsumptionRate = value; }
        }

        public int OrganicConsumptionRate {
            get { return organicConsumptionRate; }
            set { organicConsumptionRate = value; }
        }

        public int FailingCount {
            get { return failingCount; }
        }

        #endregion
    }
}