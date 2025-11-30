using UnityEngine;

namespace TheInvisibleHand.Core
{
    /// <summary>
    /// Initializes the game scene and ensures all managers are set up correctly
    /// Attach this to a GameObject in your main scene
    /// </summary>
    public class SceneInitializer : MonoBehaviour
    {
        [Header("Manager Prefabs/References")]
        public GameObject gameManagerPrefab;
        public GameObject marketEconomyPrefab;
        public GameObject eventManagerPrefab;
        public GameObject progressionManagerPrefab;
        public GameObject playerInventoryPrefab;
        public GameObject playerShopPrefab;
        public GameObject customerSpawnerPrefab;
        public GameObject tutorialManagerPrefab;
        public GameObject uiManagerPrefab;

        private void Awake()
        {
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            Debug.Log("=== Initializing The Invisible Hand ===");

            // Create managers if they don't exist
            EnsureManager<GameManager>(gameManagerPrefab, "GameManager");
            EnsureManager<Economy.MarketEconomy>(marketEconomyPrefab, "MarketEconomy");
            EnsureManager<Events.EventManager>(eventManagerPrefab, "EventManager");
            EnsureManager<ProgressionManager>(progressionManagerPrefab, "ProgressionManager");
            EnsureManager<Player.PlayerInventory>(playerInventoryPrefab, "PlayerInventory");
            EnsureManager<Player.PlayerShop>(playerShopPrefab, "PlayerShop");
            EnsureManager<NPCs.CustomerSpawner>(customerSpawnerPrefab, "CustomerSpawner");
            EnsureManager<Tutorial.TutorialManager>(tutorialManagerPrefab, "TutorialManager");
            EnsureManager<UI.UIManager>(uiManagerPrefab, "UIManager");

            Debug.Log("=== All Managers Initialized ===");
        }

        private void EnsureManager<T>(GameObject prefab, string managerName) where T : Component
        {
            if (FindObjectOfType<T>() == null)
            {
                GameObject managerObj;
                if (prefab != null)
                {
                    managerObj = Instantiate(prefab);
                }
                else
                {
                    managerObj = new GameObject(managerName);
                    managerObj.AddComponent<T>();
                }
                managerObj.name = managerName;
                Debug.Log($"✓ {managerName} created");
            }
            else
            {
                Debug.Log($"✓ {managerName} already exists");
            }
        }

        private void Start()
        {
            // Verify all systems are ready
            VerifySystemsReady();
        }

        private void VerifySystemsReady()
        {
            bool allReady = true;

            allReady &= CheckManager<GameManager>("GameManager");
            allReady &= CheckManager<Economy.MarketEconomy>("MarketEconomy");
            allReady &= CheckManager<Player.PlayerInventory>("PlayerInventory");
            allReady &= CheckManager<Player.PlayerShop>("PlayerShop");

            if (allReady)
            {
                Debug.Log("🎮 Game is ready to play!");
                LogGameState();
            }
            else
            {
                Debug.LogError("⚠️ Some systems failed to initialize!");
            }
        }

        private bool CheckManager<T>(string managerName) where T : Component
        {
            bool exists = FindObjectOfType<T>() != null;
            if (!exists)
            {
                Debug.LogError($"❌ {managerName} not found!");
            }
            return exists;
        }

        private void LogGameState()
        {
            Debug.Log($"Starting Money: ${Player.PlayerInventory.Instance.Money}");
            Debug.Log($"Starting Day: {GameManager.Instance.CurrentDay}");
            Debug.Log($"Market Items: {Economy.MarketEconomy.Instance.GetAllMarketData().Count}");
            Debug.Log($"Reputation: {Player.PlayerShop.Instance.Reputation}");
        }
    }
}
