# The Invisible Hand 🛒

A vibrant marketplace trading RPG that teaches supply and demand economics through engaging gameplay. Players run a market stall, buying low and selling high while navigating dynamic market conditions, customer personalities, and economic events.

**Target Age:** 16-21
**Platform:** iOS (via Unity)
**Genre:** Economics Simulation RPG

## 🎮 Game Concept

You've inherited a small market stall in a bustling city marketplace. Your goal: build a thriving business by mastering the art of trade. Buy goods from the wholesale market, set competitive prices, attract customers, and watch your merchant empire grow.

But it's not that simple. Market prices fluctuate based on supply and demand. Customers have different budgets and price sensitivities. Random events create opportunities and challenges. The "invisible hand" of the market is always at work.

### What Makes It Fun?

- **No Lectures:** Economics emerges naturally from gameplay - you learn by doing
- **Strategic Depth:** Multiple customer types, market conditions, and pricing strategies
- **Dynamic Markets:** Real supply/demand simulation with emergent complexity
- **Personality:** Quirky customers with unique behaviors and reactions
- **Progression:** Unlock new items, upgrades, and abilities as you level up
- **Random Events:** Heat waves, festivals, supply shocks keep things interesting

## 📚 What You'll Learn (Without Realizing It)

### Core Economic Concepts

1. **Supply & Demand**
   - Watch prices rise when supply is low
   - See demand drop when prices are too high
   - Find market equilibrium through experimentation

2. **Price Elasticity**
   - Different customers react differently to prices
   - Luxury goods vs. essentials have different sensitivities
   - Learn optimal pricing strategies

3. **Market Conditions**
   - Shortages drive prices up
   - Surpluses drive prices down
   - Timing matters for profit maximization

4. **Opportunity Cost**
   - Limited money and inventory space
   - Choose what to stock strategically
   - Trade-offs between volume and margins

5. **Market Shocks**
   - External events affect markets
   - Weather, festivals, supply chains
   - Adapt to changing conditions

6. **Consumer Behavior**
   - Budget constraints
   - Perceived value
   - Price awareness and sensitivity

## 🏗️ Technical Architecture

### Unity Systems

```
Assets/
├── Scripts/
│   ├── Core/                 # Core game loop and managers
│   │   ├── GameManager.cs    # Time, day/night cycles, game phases
│   │   ├── SceneInitializer.cs
│   │   └── ProgressionManager.cs
│   ├── Economy/              # Economic simulation engine
│   │   └── MarketEconomy.cs  # Supply/demand calculations
│   ├── Player/               # Player systems
│   │   ├── PlayerInventory.cs
│   │   └── PlayerShop.cs     # Pricing, sales, reputation
│   ├── NPCs/                 # Customer AI
│   │   ├── Customer.cs       # Individual customer behavior
│   │   └── CustomerSpawner.cs
│   ├── Items/                # Item definitions
│   │   └── Item.cs
│   ├── Events/               # Random market events
│   │   ├── MarketEvent.cs
│   │   └── EventManager.cs
│   ├── UI/                   # User interface
│   │   ├── UIManager.cs
│   │   └── MarketDashboard.cs
│   └── Tutorial/             # Onboarding
│       └── TutorialManager.cs
├── Scenes/
│   └── MainScene.txt         # Scene setup guide
├── Prefabs/
├── Materials/
├── Sprites/
└── UI/
```

### Key Game Loop

1. **Time Progression:** Day divided into phases (Opening, Morning Rush, Lunch, Afternoon, Evening Rush, Closing, Night)
2. **Customer Spawning:** Traffic varies by time and reputation
3. **Market Simulation:** Prices update based on player actions and market forces
4. **Events:** Random economic events create dynamic scenarios
5. **Progression:** XP, levels, unlockables reward continued play

### Economic Engine

The `MarketEconomy.cs` system simulates real supply and demand:

```
Price = BasePrice × (Demand/Supply)^elasticity
```

- Sales reduce supply → prices rise
- Restocking increases supply → prices fall
- Events shift demand → market opportunities
- Memory factor → prices don't change instantly

### Customer AI

Each customer has:
- **Archetype:** Bargain Hunter, Premium Buyer, Smart Shopper, etc.
- **Budget:** Limited money to spend
- **Price Awareness:** How well they know market prices
- **Price Sensitivity:** How much they care about price
- **Preferences:** Items they want to buy
- **Mood:** Affects purchase decisions

Customers make decisions using economic logic:
```
perceivedValue = baseValue × priceRatio × preference × urgency × mood
```

## 🚀 Getting Started

### Prerequisites

- Unity 2022.3 LTS or later
- iOS device or simulator
- Xcode (for iOS builds)
- macOS (for iOS deployment)

### Opening the Project

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/the-invisible-hand.git
   ```

2. Open Unity Hub

3. Add project: `the-invisible-hand`

4. Open with Unity 2022.3.10f1 (or compatible version)

### Setting Up the Scene

Follow instructions in `Assets/Scenes/MainScene.txt` to set up the main game scene with all required managers and UI.

Quick setup:
1. Create empty GameObjects for each manager
2. Attach corresponding scripts
3. Set up Canvas with UI hierarchy
4. Assign references in SceneInitializer

### Testing in Unity

1. Open the Main scene
2. Press Play
3. Watch console for initialization logs
4. Interact with UI to test systems

Expected console output:
```
=== Initializing The Invisible Hand ===
✓ GameManager created
✓ MarketEconomy created
...
Market items initialized
Customer spawned: Jordan Smith (SmartShopper)
```

## 📱 Building for iOS

### Configure iOS Settings

1. **File → Build Settings**
   - Platform: iOS
   - Click "Switch Platform"

2. **Edit → Project Settings → Player**
   - Company Name: `InvisibleHandGames`
   - Product Name: `The Invisible Hand`
   - Bundle Identifier: `com.invisiblehandgames.theinvisiblehand`
   - Version: `0.1.0`
   - Minimum iOS: `13.0`
   - Target iOS: `17.0`
   - Architecture: `ARM64`
   - Scripting Backend: `IL2CPP`

3. **Graphics Settings**
   - Graphics API: Metal
   - Color Space: Linear

4. **Build**
   - File → Build Settings → Build
   - Choose output folder: `Builds/iOS/`
   - Unity generates Xcode project

### Deploy to Device

1. Open generated Xcode project
2. Connect iOS device
3. Select device in Xcode
4. Set signing team
5. Build and Run

## 🎯 Game Design Philosophy

### Learning Through Play

The game teaches economics **implicitly** through gameplay, not explicitly through tutorials:

- ❌ Don't: "Supply and demand determines price. When supply is low..."
- ✅ Do: "There's a heat wave! Coffee demand is surging!" (players discover prices rise)

### Fail-Safe Experimentation

- No permanent failure states
- Encouragement to experiment with pricing
- Learn from mistakes without harsh penalties
- Tutorial guides without being preachy

### Progressive Complexity

- **Week 1:** Learn basics (buy, price, sell)
- **Week 2:** Understand market conditions
- **Week 3:** Anticipate events and plan ahead
- **Week 4:** Master advanced strategies (speculation, timing)

### Feedback Loops

Players constantly receive feedback:
- Customer reactions (happy/angry)
- Reputation changes
- Profit/loss calculations
- Market condition indicators
- Achievement unlocks

## 🛠️ Customization & Extension

### Adding New Items

Create new `Item` ScriptableAssets:
```csharp
// In Unity: Create → The Invisible Hand → Item
itemId: "pizza"
displayName: "Pizza"
basePrice: 8.0
baseDemand: 70
category: Food
```

Register in `MarketEconomy.InitializeMarket()`:
```csharp
RegisterItem("Pizza", basePrice: 8.0f, baseDemand: 70f);
```

### Creating Market Events

Create `MarketEvent` ScriptableAssets:
```csharp
eventTitle: "Pizza Festival"
impacts: [
  { affectedItem: "Pizza", impactType: DemandIncrease, magnitude: 1.5 }
]
```

### Adding Customer Archetypes

Extend `CustomerArchetype` enum and configure in `CustomerSpawner`:
```csharp
case CustomerArchetype.Tourist:
    customer.priceSensitivity = 0.2f; // Don't care about price
    customer.dailyBudget = Random.Range(100f, 300f);
    break;
```

## 📊 Game Balance

Current balance parameters (tunable):

- **Starting Money:** $100
- **Starting Inventory:** 100 slots
- **Base Spawn Rate:** 30 seconds between customers
- **Price Elasticity:** 0.5 (medium sensitivity)
- **Reputation Decay:** 5% per day
- **Wholesale Margin:** 65% of retail

These can be adjusted in the Inspector on manager GameObjects.

## 🐛 Debugging Tips

### Enable Debug Logs

Most systems log to console. Look for:
- `✓` Initialization success
- `EVENT:` Market events
- `Customer spawned:` NPC creation
- `Sold X items for $Y` Transactions

### Common Issues

**No customers spawning:**
- Check CustomerSpawner is active
- Verify spawn rate and max customers settings
- Check current game phase (customers spawn during open hours)

**Prices not changing:**
- Ensure MarketEconomy.Instance exists
- Check RecordSale/RecordRestock are being called
- Verify supply/demand values in market data

**UI not updating:**
- Check UIManager subscriptions in Start()
- Verify Text components are assigned
- Look for null reference errors in console

## 🎨 Art & Polish (Future)

Current implementation is code-focused. To make it production-ready:

### Visual Assets Needed
- Character sprites (customers, merchant)
- Item icons (food, goods)
- Market stall backgrounds
- UI elements (buttons, panels, icons)
- Particle effects (sales sparkles, complaints)

### Audio
- Background music (upbeat marketplace theme)
- SFX (coins, customer chatter, bell dings)
- Voice barks (customer reactions)

### Animations
- Customer walk cycles
- Item purchase animations
- Price change indicators
- Notification pop-ups

### Juice & Feel
- Screen shake on big sales
- Particle effects for achievements
- Smooth UI transitions
- Haptic feedback (iOS)

## 🚧 Roadmap

### Version 0.2 - Polish
- [ ] Complete UI implementation
- [ ] Add visual assets
- [ ] Sound effects and music
- [ ] Save/load system
- [ ] Balance pass based on playtesting

### Version 0.3 - Content
- [ ] More items (20+ total)
- [ ] More events (15+ scenarios)
- [ ] More customer types
- [ ] Seasonal mechanics
- [ ] Competitor shops (AI merchants)

### Version 0.4 - Advanced Economics
- [ ] Futures/speculation mechanics
- [ ] Price discrimination strategies
- [ ] Market manipulation detection
- [ ] Economic crises (inflation, recession)
- [ ] Advanced charts and analytics

### Version 1.0 - Release
- [ ] Full tutorial
- [ ] 50+ items
- [ ] Story mode with progression
- [ ] Endless mode
- [ ] Leaderboards
- [ ] Achievements integration (Game Center)

## 📖 Educational Use

### For Teachers

This game can supplement economics curriculum:

- **Lesson 1:** Basic supply/demand (play first week)
- **Lesson 2:** Market equilibrium (observe price convergence)
- **Lesson 3:** Elasticity (compare luxury vs. essential goods)
- **Lesson 4:** External shocks (analyze event impacts)

Debrief questions:
- "Why did prices rise during the festival?"
- "Which customer type is most price sensitive?"
- "How did the supply chain disruption affect your strategy?"

### For Self-Learners

Track your progress:
- Day 1-7: Basic profitability
- Day 8-14: Consistent profit margins
- Day 15-30: Anticipating market changes
- Day 30+: Mastering all market conditions

## 🤝 Contributing

Contributions welcome! Areas that need help:

- Art assets (sprites, UI)
- Sound design
- Additional market events
- Economic scenarios
- Playtesting and feedback
- Documentation improvements

## 📄 License

MIT License - See LICENSE file for details

## 🙏 Credits

Inspired by real-world economics education games and market simulation board games.

Built with Unity and lots of coffee ☕

---

**Ready to become a merchant tycoon? Let the invisible hand guide you to profit!** 📈💰
