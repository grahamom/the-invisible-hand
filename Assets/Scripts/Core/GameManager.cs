using UnityEngine;
using System;

namespace TheInvisibleHand.Core
{
    /// <summary>
    /// Main game manager - controls game flow, time progression, and global state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [SerializeField] private float gameSpeedMultiplier = 1f;
        [SerializeField] private int startingDay = 1;

        [Header("Game State")]
        public int CurrentDay { get; private set; }
        public float CurrentTime { get; private set; } // 0-24 hours
        public GamePhase CurrentPhase { get; private set; }

        public event Action<int> OnNewDay;
        public event Action<GamePhase> OnPhaseChanged;
        public event Action<float> OnTimeProgressed;

        private bool isPaused = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentDay = startingDay;
            CurrentTime = 6f; // Start at 6 AM
            CurrentPhase = GamePhase.Opening;
        }

        private void Update()
        {
            if (isPaused) return;

            // Progress time (1 real second = 1 game minute)
            float timeIncrement = Time.deltaTime * gameSpeedMultiplier * 60f;
            CurrentTime += timeIncrement / 3600f; // Convert to hours

            OnTimeProgressed?.Invoke(CurrentTime);

            // Check for phase transitions
            UpdateGamePhase();

            // Check for new day
            if (CurrentTime >= 24f)
            {
                AdvanceDay();
            }
        }

        private void UpdateGamePhase()
        {
            GamePhase newPhase = CurrentPhase;

            if (CurrentTime >= 6f && CurrentTime < 9f)
                newPhase = GamePhase.Opening;
            else if (CurrentTime >= 9f && CurrentTime < 12f)
                newPhase = GamePhase.MorningRush;
            else if (CurrentTime >= 12f && CurrentTime < 14f)
                newPhase = GamePhase.Lunch;
            else if (CurrentTime >= 14f && CurrentTime < 17f)
                newPhase = GamePhase.Afternoon;
            else if (CurrentTime >= 17f && CurrentTime < 20f)
                newPhase = GamePhase.EveningRush;
            else if (CurrentTime >= 20f && CurrentTime < 22f)
                newPhase = GamePhase.Closing;
            else
                newPhase = GamePhase.Night;

            if (newPhase != CurrentPhase)
            {
                CurrentPhase = newPhase;
                OnPhaseChanged?.Invoke(CurrentPhase);
                Debug.Log($"Phase changed to: {CurrentPhase}");
            }
        }

        private void AdvanceDay()
        {
            CurrentDay++;
            CurrentTime = 6f; // Reset to 6 AM
            OnNewDay?.Invoke(CurrentDay);
            Debug.Log($"New day: {CurrentDay}");
        }

        public void SetGameSpeed(float multiplier)
        {
            gameSpeedMultiplier = Mathf.Clamp(multiplier, 0.1f, 5f);
        }

        public void PauseGame() => isPaused = true;
        public void ResumeGame() => isPaused = false;
        public void TogglePause() => isPaused = !isPaused;
    }

    public enum GamePhase
    {
        Opening,        // 6-9 AM: Prepare for the day, buy stock
        MorningRush,    // 9-12 PM: First customer wave
        Lunch,          // 12-2 PM: Peak demand period
        Afternoon,      // 2-5 PM: Slow period, good for restocking
        EveningRush,    // 5-8 PM: Second customer wave
        Closing,        // 8-10 PM: Final sales, end-of-day accounting
        Night           // 10-6 AM: Shop closed, market simulation runs
    }
}
