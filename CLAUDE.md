# CLAUDE.md - AI Assistant Guide for The Invisible Hand

## Project Overview

**The Invisible Hand** is a marketplace trading RPG that teaches supply and demand economics through engaging gameplay. Players run a market stall, buying low and selling high while navigating dynamic market conditions, customer personalities, and economic events.

### Key Facts
- **Platform**: iOS (built with Unity)
- **Language**: C#
- **Genre**: Economics Simulation RPG
- **Target Audience**: Ages 16-21
- **Educational Goal**: Teach economics implicitly through gameplay, not lectures
- **Core Loop**: Buy → Price → Sell → Profit → Expand

### Project Status
- **Current State**: Core systems implemented, needs UI/art/polish
- **Unity Version**: 2022.3 LTS or later
- **Main Branch**: `main`
- **Recent Major Commit**: Complete iOS economics RPG game implementation

## Repository Structure

```
the-invisible-hand/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/                   # Core game systems
│   │   │   ├── GameManager.cs      # Time progression, day/night cycles
│   │   │   ├── SceneInitializer.cs # Scene setup and dependency injection
│   │   │   └── ProgressionManager.cs # XP, levels, unlocks
│   │   ├── Economy/                # Economic simulation
│   │   │   └── MarketEconomy.cs    # Supply/demand engine (THE CORE!)
│   │   ├── Player/                 # Player systems
│   │   │   ├── PlayerInventory.cs  # Money and item storage
│   │   │   └── PlayerShop.cs       # Shop management, reputation
│   │   ├── NPCs/                   # Customer AI
│   │   │   ├── Customer.cs         # Individual customer behavior
│   │   │   └── CustomerSpawner.cs  # Customer generation
│   │   ├── Items/                  # Item data
│   │   │   └── Item.cs             # Item definitions
│   │   ├── Events/                 # Market events
│   │   │   ├── MarketEvent.cs      # Event definitions
│   │   │   └── EventManager.cs     # Event triggering
│   │   ├── UI/                     # User interface
│   │   │   ├── UIManager.cs        # UI orchestration
│   │   │   └── MarketDashboard.cs  # Market data display
│   │   └── Tutorial/               # Onboarding
│   │       └── TutorialManager.cs  # Tutorial system
│   ├── Scenes/
│   │   └── MainScene.txt           # Scene setup instructions
│   ├── Prefabs/                    # (To be created)
│   ├── Sprites/                    # (To be created)
│   └── UI/                         # (To be created)
├── Packages/
│   └── manifest.json               # Unity package dependencies
├── ProjectSettings/
│   ├── ProjectSettings.txt         # Project configuration
│   └── ProjectVersion.txt          # Unity version
├── README.md                       # Comprehensive project documentation
├── GAME_DESIGN.md                  # Detailed game design document
└── CLAUDE.md                       # This file (AI assistant guide)
```

## Architecture Overview

### Design Pattern: Singleton Managers + Event-Driven

The codebase uses Unity's component system with singleton managers for global state and event-driven communication.

**Core Managers:**
- `GameManager.Instance` - Time, day/night cycles, game phases
- `MarketEconomy.Instance` - Supply/demand simulation (THE HEART OF THE GAME)
- `PlayerShop.Instance` - Shop operations, reputation, leveling
- `PlayerInventory.Instance` - Money and inventory management
- `EventManager.Instance` - Random market events
- `UIManager.Instance` - UI updates and display
- `TutorialManager.Instance` - Tutorial flow

**Communication Pattern:**
```csharp
// Managers expose events
public event Action<string, float> OnPriceChanged;

// Other systems subscribe
MarketEconomy.Instance.OnPriceChanged += HandlePriceChange;

// Fire events when state changes
OnPriceChanged?.Invoke(itemId, newPrice);
```

### Key Systems Explained

#### 1. GameManager (Core/GameManager.cs:1)
Controls the passage of time and game phases. Time flows continuously (1 real second = 1 game minute).

**Game Phases:**
- Opening (6-9 AM): Planning, wholesale buying, price setting
- Morning Rush (9-12 PM): First customer wave
- Lunch (12-2 PM): Peak demand
- Afternoon (2-5 PM): Slow period, restocking
- Evening Rush (5-8 PM): Second customer wave
- Closing (8-10 PM): Final sales, accounting
- Night (10-6 AM): Shop closed, market simulation runs

**Key Methods:**
- `SetGameSpeed(float)` - Adjust time flow
- `PauseGame()` / `ResumeGame()`

**Events:**
- `OnNewDay` - Triggered when day advances
- `OnPhaseChanged` - Triggered when game phase changes
- `OnTimeProgressed` - Triggered continuously

#### 2. MarketEconomy (Economy/MarketEconomy.cs:1)
**THIS IS THE MOST IMPORTANT SYSTEM.** It simulates real supply and demand economics.

**Economic Formula:**
```csharp
CurrentPrice = BasePrice × (Demand / Supply)^elasticity
```

**Key Concepts:**
- **Supply** decreases when items are sold, increases when restocked
- **Demand** fluctuates based on time, events, and player pricing
- **Price Elasticity** (default 0.5) determines how sensitive prices are to supply/demand
- **Market Memory** (default 0.7) prevents sudden price shocks

**Market Conditions:**
- `Shortage` - Supply < 30% of demand, prices 150%+ of base
- `Rising` - Prices trending up (120-150%)
- `Stable` - Balanced (85-115%)
- `Falling` - Prices trending down (70-85%)
- `Surplus` - Supply > 200% of demand, prices 50-70%

**Key Methods:**
- `RegisterItem(itemId, basePrice, baseDemand)` - Add new tradeable item
- `GetCurrentPrice(itemId)` - Get market price
- `GetWholesalePrice(itemId)` - Get wholesale price (65% of retail)
- `RecordSale(itemId, quantity, price)` - Update market after sale
- `RecordRestock(itemId, quantity)` - Update market after restock
- `GetMarketData(itemId)` - Get full market data

**Daily Simulation:**
Each day, the market simulates:
- Demand fluctuation (±20% by default)
- Supply replenishment (other merchants restock)
- Market consumption (demand reduces supply)

#### 3. PlayerShop (Player/PlayerShop.cs:1)
Manages the player's shop operations.

**Key Features:**
- **Reputation** (0-100): Affects customer traffic
  - Increases with fair pricing (+0.1/sale)
  - Decreases with overpricing (-0.5/sale)
  - Decays 5% per day (encourages consistency)
- **Experience & Leveling**: Gain 10% of profit as XP
- **Shop Listings**: Max display slots (upgradable)

**Key Methods:**
- `SetItemPrice(itemId, price)` - List item for sale
- `SellItem(itemId, quantity, out totalPrice)` - Process sale
- `BuyWholesale(itemId, quantity)` - Purchase from market
- `UpgradeDisplaySlots(int)` - Expand shop capacity

**Reputation Formula:**
```csharp
priceRatio = salePrice / marketPrice
if (priceRatio > 1.3f) → -0.5 reputation (overpriced)
if (priceRatio < 0.8f) → +0.3 reputation (great deal!)
else → +0.1 reputation (fair price)
```

#### 4. Customer (NPCs/Customer.cs:1)
Individual customer AI with economic decision-making.

**Customer Archetypes:**
- `BargainHunter` - High price sensitivity, low budget
- `PremiumBuyer` - Low price sensitivity, high budget
- `RegularShopper` - Average behavior
- `ImpulseBuyer` - Low price awareness, emotional purchases
- `SmartShopper` - High price awareness, waits for deals
- `Loyalist` - Returns if treated well

**Economic Parameters:**
- `priceAwareness` (0-1): How well they know market prices
- `priceSensitivity` (0-1): How much price affects decisions
- `budget`: Total money available
- `itemPreferences`: What they like/dislike

**Purchase Decision Algorithm (Customer.cs:59):**
```csharp
1. Check if item is on shopping list OR customer is impulsive
2. Get market price and asking price
3. Calculate priceRatio = askingPrice / marketPrice
4. Check affordability
5. Apply archetype logic:
   - BargainHunter: Won't buy if priceRatio > 1.0
   - PremiumBuyer: Doesn't care about price
   - SmartShopper: Evaluates multiple factors
6. Calculate perceived value (preference + price + urgency + mood)
7. Decide quantity based on budget
8. Purchase or leave
```

**Key Methods:**
- `EvaluatePurchase(itemId, price, out quantity)` - Decide whether to buy
- `CompletePurchase(itemId, quantity, cost)` - Execute purchase
- `LeaveShop(satisfied)` - Exit (affects reputation if unsatisfied)

## Development Conventions

### C# Code Style

1. **Namespaces**
   - Use `TheInvisibleHand.[SystemName]` pattern
   - Examples: `TheInvisibleHand.Core`, `TheInvisibleHand.Economy`

2. **Naming Conventions**
   - `PascalCase` for classes, methods, properties, public fields
   - `camelCase` for private fields, parameters, local variables
   - Prefix private fields with nothing (no underscore)
   - Use descriptive names

3. **Unity Patterns**
   - Singleton pattern:
     ```csharp
     public static ClassName Instance { get; private set; }

     private void Awake() {
         if (Instance != null && Instance != this) {
             Destroy(gameObject);
             return;
         }
         Instance = this;
         DontDestroyOnLoad(gameObject); // If needed across scenes
     }
     ```
   - Use `[SerializeField]` for inspector-editable private fields
   - Use `[Header("Section")]` to organize inspector properties

4. **Event-Driven Communication**
   - Prefer events over direct coupling
   - Pattern: `public event Action<T> OnSomethingHappened;`
   - Invoke with null check: `OnSomethingHappened?.Invoke(data);`
   - Subscribe in `Start()`, unsubscribe in `OnDestroy()`

5. **Documentation**
   - Use `///` XML comments for public APIs
   - Document complex algorithms inline
   - Mark important systems with comments like `// THE CORE!`

### Economics Design Principles

**CRITICAL**: Every feature must serve the educational goal.

1. **Show, Don't Tell**
   - ❌ Bad: "Supply and demand determines price"
   - ✅ Good: "Heat wave! Coffee demand is surging!" (players observe price rise)

2. **Economic Realism**
   - All prices follow `P = BasePrice × (D/S)^elasticity`
   - Customer decisions are economically rational (within their archetype)
   - Market conditions reflect real supply/demand dynamics

3. **Balanced Game Design**
   - No "exploit" strategies that break the economy
   - Fair pricing should be profitable
   - Experimentation should be encouraged, not punished
   - Failure is a learning opportunity

4. **Clear Feedback**
   - Players must understand WHY prices changed
   - Customer reactions should be visible
   - Reputation changes should be explained

### Unity-Specific Guidelines

1. **Scene Setup**
   - Follow instructions in `Assets/Scenes/MainScene.txt`
   - All managers must be initialized via `SceneInitializer`
   - Use proper GameObject hierarchy

2. **Performance**
   - Target: 60 FPS on iOS devices
   - Avoid allocation in `Update()` loops
   - Cache component references
   - Use object pooling for customers (future optimization)

3. **Mobile Optimization**
   - Keep UI elements large (min 44×44 points for touch)
   - Avoid complex shaders
   - Compress textures for mobile
   - Test on actual iOS devices, not just simulator

4. **Serialization**
   - Use `[Serializable]` for data classes
   - Don't serialize Unity objects in save files
   - Use ScriptableObjects for item definitions (future)

## Testing Strategy

### What to Test

1. **Economic Simulation**
   - Verify price formula correctness
   - Test edge cases (zero supply, infinite demand)
   - Ensure no NaN or infinity values
   - Validate market conditions update correctly

2. **Customer AI**
   - Test each archetype behaves correctly
   - Verify purchase decisions are economically sound
   - Check budget constraints are respected
   - Ensure no customers buy unaffordable items

3. **Reputation System**
   - Verify reputation changes match pricing
   - Test decay over time
   - Check customer traffic scales with reputation

4. **Progression**
   - Test XP gain and leveling
   - Verify unlocks trigger at correct levels
   - Check no progression exploits

### Testing Approach

```csharp
// Example test pattern (Unity Test Framework)
[Test]
public void MarketPrice_IncreasesWhenSupplyDecreases()
{
    // Arrange
    MarketEconomy.Instance.RegisterItem("TestItem", 10f, 100f);
    float initialPrice = MarketEconomy.Instance.GetCurrentPrice("TestItem");

    // Act
    MarketEconomy.Instance.RecordSale("TestItem", 50, 10f);
    float newPrice = MarketEconomy.Instance.GetCurrentPrice("TestItem");

    // Assert
    Assert.Greater(newPrice, initialPrice, "Price should increase when supply decreases");
}
```

### Manual Testing Checklist

- [ ] Customer spawning works in all game phases
- [ ] Prices update visibly in UI
- [ ] Reputation changes are reflected in customer traffic
- [ ] Tutorial flows smoothly for new players
- [ ] Market events trigger and affect prices correctly
- [ ] Game can run for 30+ in-game days without bugs
- [ ] Save/load works (when implemented)

## Common Tasks for AI Assistants

### Adding a New Item

1. **Register in MarketEconomy** (Economy/MarketEconomy.cs:42):
   ```csharp
   RegisterItem("Pizza", basePrice: 8.0f, baseDemand: 70f);
   ```

2. **Create Item ScriptableObject** (future):
   ```csharp
   // Create → The Invisible Hand → Item
   itemId: "Pizza"
   displayName: "Pizza"
   basePrice: 8.0f
   category: Food
   ```

3. **Add to CustomerSpawner** shopping list pool

4. **Create sprite/icon** in Assets/Sprites/Items/

5. **Test economic behavior** with various market conditions

### Adding a Market Event

1. **Create MarketEvent class** (Events/MarketEvent.cs):
   ```csharp
   public class PizzaFestival : MarketEvent
   {
       public override void Apply()
       {
           MarketEconomy.Instance.GetMarketData("Pizza").CurrentDemand *= 2.0f;
       }
   }
   ```

2. **Register in EventManager** with trigger conditions

3. **Add event notification** to UI

4. **Playtest** to ensure it creates interesting decisions

### Adding a Customer Archetype

1. **Add to enum** (NPCs/Customer.cs:239):
   ```csharp
   public enum CustomerArchetype
   {
       // ... existing
       Tourist  // New archetype
   }
   ```

2. **Configure in CustomerSpawner**:
   ```csharp
   case CustomerArchetype.Tourist:
       customer.priceSensitivity = 0.2f;  // Don't care about price
       customer.priceAwareness = 0.3f;     // Don't know market prices
       customer.dailyBudget = Random.Range(100f, 300f);
       customer.isImpulsive = true;
       break;
   ```

3. **Test purchase behavior** across price ranges

### Debugging Economic Issues

1. **Enable verbose logging** in MarketEconomy:
   ```csharp
   Debug.Log($"{itemId}: Price=${price:F2}, D={demand:F0}, S={supply:F0}, Ratio={demand/supply:F2}");
   ```

2. **Check market data** in inspector (add to UIManager for debugging)

3. **Verify formula** matches expected behavior:
   - Low supply → High price
   - High demand → High price
   - Oversupply → Low price

4. **Test edge cases**:
   - What if supply is zero?
   - What if demand is zero?
   - What if prices are clamped at limits?

### Implementing UI

1. **Subscribe to manager events**:
   ```csharp
   void Start()
   {
       PlayerInventory.Instance.OnMoneyChanged += UpdateMoneyDisplay;
       PlayerShop.Instance.OnReputationChanged += UpdateReputationBar;
       MarketEconomy.Instance.OnPriceChanged += UpdatePriceDisplay;
   }
   ```

2. **Use TextMeshPro** for all text (already in dependencies)

3. **Follow mobile UI guidelines**:
   - Large touch targets (44×44 pt minimum)
   - High contrast text
   - Clear visual hierarchy

4. **Test on device** (not just editor)

## Key Files to Understand First

When starting work on this project, read these files in order:

1. **README.md** - Comprehensive project overview
2. **GAME_DESIGN.md** - Detailed game design philosophy
3. **Economy/MarketEconomy.cs** - The heart of the simulation
4. **Core/GameManager.cs** - Game flow and time
5. **NPCs/Customer.cs** - AI decision-making
6. **Player/PlayerShop.cs** - Player-facing systems

## Important Economic Formulas

These are CORE to the game. Don't modify without understanding implications.

### Price Calculation (MarketEconomy.cs:140)
```csharp
supplyDemandRatio = demand / supply
priceMultiplier = Pow(supplyDemandRatio, elasticity)  // elasticity = 0.5
currentPrice = basePrice * priceMultiplier
// Clamped to 50%-300% of base price
```

### Reputation Change (PlayerShop.cs:154)
```csharp
priceRatio = salePrice / marketPrice
if (priceRatio > 1.3)  → -0.5 reputation
if (priceRatio > 1.1)  → -0.1 reputation
if (priceRatio < 0.8)  → +0.3 reputation
else                    → +0.1 reputation
```

### Customer Perceived Value (Customer.cs:113)
```csharp
value = 1.0
value += personalPreference * 0.3
value *= (2.0 - priceRatio)  // Lower price = higher value
if (onShoppingList) value += 0.4
value *= moodMultiplier
return Clamp01(value)
```

### Experience & Leveling (PlayerShop.cs:179)
```csharp
xpGain = profit * 0.1
xpNeeded = currentLevel * 100
if (experience >= xpNeeded) → Level Up
```

## Git Workflow

### Branch Strategy
- **Main branch**: `main` (protected)
- **Feature branches**: `claude/[description]-[session-id]`
- Always develop on feature branches
- Push with: `git push -u origin <branch-name>`

### Commit Guidelines
```bash
# Good commit messages
git commit -m "Add Tourist customer archetype with high budget, low price sensitivity"
git commit -m "Fix market price calculation when supply is zero"
git commit -m "Implement reputation decay system"

# Bad commit messages
git commit -m "Fix bug"
git commit -m "Update code"
git commit -m "WIP"
```

### Before Committing
1. **Test in Unity** - Press Play, verify no errors
2. **Check economics** - Ensure formulas still work correctly
3. **Review changes** - Read your diff
4. **Run builds** - Ensure no compilation errors

## Common Pitfalls to Avoid

### 1. Breaking the Economy
❌ **Don't**: Modify price formulas without understanding cascading effects
✅ **Do**: Test economic changes with extreme values (zero supply, infinite demand)

### 2. Singleton Null References
❌ **Don't**: Access `Instance` in `Awake()` before it's set
✅ **Do**: Subscribe to events in `Start()`, access instances after initialization

### 3. Unsubscribed Events
❌ **Don't**: Subscribe to events without unsubscribing
✅ **Do**: Always unsubscribe in `OnDestroy()`:
```csharp
void OnDestroy()
{
    if (MarketEconomy.Instance != null)
        MarketEconomy.Instance.OnPriceChanged -= HandlePriceChange;
}
```

### 4. Mobile Performance
❌ **Don't**: Use `Find()` or `GetComponent()` in `Update()`
✅ **Do**: Cache references in `Start()` or `Awake()`

### 5. Economic Unrealism
❌ **Don't**: Create systems where player can exploit infinite money
✅ **Do**: Ensure all profit comes from actual economic activity (buy low, sell high)

### 6. Over-Engineering
❌ **Don't**: Add complex systems "for future flexibility"
✅ **Do**: Implement what's needed now, refactor when actually needed

## Educational Value Checklist

When adding features, ensure they support learning:

- [ ] Feature demonstrates an economic concept
- [ ] Cause and effect are clear to players
- [ ] Feedback is immediate and understandable
- [ ] Players learn by doing, not reading
- [ ] Mistakes are instructive, not punishing
- [ ] Economic principles are mechanically enforced

## Project Vision & Goals

### Core Philosophy
> "Players can't help but learn economics because it's built into the core gameplay loop. The invisible hand guides them naturally."

### Success Metrics
1. **Engagement**: Players want to keep playing
2. **Learning**: Players demonstrate economic understanding through optimal strategies
3. **Fun**: High app store ratings mentioning "fun" and "learned"

### What This Game Is
- A fun marketplace trading game
- An implicit economics teacher
- A sandbox for economic experimentation
- A progression-based RPG

### What This Game Is NOT
- A textbook or lecture
- A dry simulation
- Punishingly difficult
- Pay-to-win

## Resources

### Unity Documentation
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/index.html)
- [iOS Build Settings](https://docs.unity3d.com/Manual/class-PlayerSettingsiOS.html)

### Economics Resources
- Supply and demand curves
- Price elasticity of demand
- Market equilibrium concepts
- Consumer behavior theory

### Game Design References
- *Game Programming Patterns* - Singleton, Observer patterns
- *The Art of Game Design* - Feedback loops, player psychology
- Apple Human Interface Guidelines - iOS design standards

## Quick Reference Commands

### Unity Editor
- **Play Mode**: Ctrl/Cmd + P
- **Pause**: Ctrl/Cmd + Shift + P
- **Frame Advance**: Ctrl/Cmd + Alt + P

### Building for iOS
```bash
# Switch to iOS platform
File → Build Settings → iOS → Switch Platform

# Configure player settings
Edit → Project Settings → Player → iOS settings

# Build
File → Build Settings → Build
```

### Testing in Editor
```csharp
// Force spawn customer
CustomerSpawner.Instance.SpawnCustomer();

// Trigger market event
EventManager.Instance.TriggerEvent(eventName);

// Fast forward time
GameManager.Instance.SetGameSpeed(5f);

// Check market data
var data = MarketEconomy.Instance.GetMarketData("Coffee");
Debug.Log($"Price: ${data.CurrentPrice}, Supply: {data.Supply}, Demand: {data.CurrentDemand}");
```

## Getting Help

If you encounter issues:

1. **Check Console**: Most errors are logged with context
2. **Read README.md**: Comprehensive troubleshooting section
3. **Check GAME_DESIGN.md**: Understand intended behavior
4. **Review this file**: Common pitfalls and solutions
5. **Test economic formulas**: Verify math is correct
6. **Create GitHub issue**: If bug persists

## Version History

- **v0.1** (Current): Core systems implemented, needs UI/art/polish
- **Future v0.2**: Complete UI, visual assets, sound
- **Future v0.3**: More content (items, events, customer types)
- **Future v1.0**: Full release with tutorial, 50+ items, story mode

---

**Last Updated**: 2025-11-30
**Document Version**: 2.0
**Maintainer**: grahamom
**Unity Version**: 2022.3 LTS

**Remember**: The invisible hand works best when the economics are mechanically sound and the gameplay is genuinely fun. Teach through play, not preaching. 🎮📈
