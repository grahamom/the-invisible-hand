# The Invisible Hand - Game Design Document

## Executive Summary

**Title:** The Invisible Hand
**Platform:** iOS (Unity)
**Genre:** Economics Simulation / Tycoon / Casual Strategy
**Target Audience:** 16-21 years old
**Core Loop:** Buy → Price → Sell → Profit → Expand
**Educational Goal:** Teach supply/demand economics through natural gameplay

## Design Pillars

### 1. Learning By Doing
No textbook explanations. Players discover economic principles through experimentation and observation.

### 2. Strategic Depth Without Complexity
Easy to learn, hard to master. Intuitive controls, emergent complexity.

### 3. Personality & Charm
Quirky characters, humorous events, satisfying feedback. Not dry or academic.

### 4. Risk-Free Experimentation
Failure is learning. No punishing mechanics that discourage experimentation.

## Core Gameplay Loop

```
1. Check Market Dashboard
   ↓
2. Buy Wholesale (low prices/surplus opportunities)
   ↓
3. Set Retail Prices (competitive but profitable)
   ↓
4. Serve Customers (watch reactions, adjust)
   ↓
5. Observe Results (profit/loss, reputation, market changes)
   ↓
6. Adapt Strategy
   ↓
[Loop back to 1]
```

## Game Phases (Daily Cycle)

### Opening (6-9 AM)
- **Player Activity:** Check market, buy wholesale, set prices
- **Customer Traffic:** None
- **Strategic Purpose:** Planning and preparation

### Morning Rush (9-12 PM)
- **Player Activity:** Serve customers, monitor sales
- **Customer Traffic:** Medium (office workers, early shoppers)
- **Strategic Purpose:** First wave of revenue

### Lunch (12-2 PM)
- **Player Activity:** High activity period
- **Customer Traffic:** HIGH (peak demand)
- **Strategic Purpose:** Major profit opportunity

### Afternoon (2-5 PM)
- **Player Activity:** Restock, adjust prices
- **Customer Traffic:** Low (perfect for restocking)
- **Strategic Purpose:** Strategic repositioning

### Evening Rush (5-8 PM)
- **Player Activity:** Final sales push
- **Customer Traffic:** High (commuters, dinner shoppers)
- **Strategic Purpose:** Second revenue wave

### Closing (8-10 PM)
- **Player Activity:** Review day, plan tomorrow
- **Customer Traffic:** Low and declining
- **Strategic Purpose:** Reflection and accounting

### Night (10-6 AM)
- **Player Activity:** Shop closed (can skip/fast-forward)
- **Customer Traffic:** None
- **Strategic Purpose:** Market simulation runs, events may trigger

## Economic Systems

### Supply & Demand Simulation

**Price Formula:**
```
CurrentPrice = BasePrice × (Demand / Supply)^elasticity
```

**Supply Changes:**
- Player restocks → Supply increases → Prices fall
- Player sells → Supply decreases → Prices rise
- Market consumption → Supply gradually decreases
- Market restocks → Supply gradually increases
- Events → Supply shocks (sudden changes)

**Demand Changes:**
- Time of day → Demand varies by item
- Events → Demand surges/drops
- Trends → Gradual shifts over time
- Seasonality → (Future feature)

### Market Conditions

**Shortage** (Supply < 30% of demand)
- Prices 150%+ of base
- High profit potential
- Risk: Can't stock enough inventory

**Rising** (Prices trending up)
- Prices 120-150% of base
- Good sell opportunity
- Risk: Demand may drop soon

**Stable** (Balanced market)
- Prices 85-115% of base
- Predictable margins
- Risk: Lower profit potential

**Falling** (Prices trending down)
- Prices 70-85% of base
- Good buy opportunity
- Risk: May fall further

**Surplus** (Supply > 200% of demand)
- Prices 50-70% of base
- Wholesale bargains
- Risk: May spoil before selling

### Customer AI Behavior

Each customer has:

**Economic Parameters:**
- `budget` - Total money available
- `priceAwareness` - Knowledge of market prices (0-1)
- `priceSensitivity` - How much price affects decisions (0-1)

**Decision Algorithm:**
```
1. Check if item is on shopping list OR customer is impulsive
2. Get market price and seller's asking price
3. Calculate price ratio: askingPrice / marketPrice
4. Evaluate affordability: budget >= askingPrice
5. Apply archetype modifiers:
   - BargainHunter: Won't buy if ratio > 1.0
   - PremiumBuyer: Doesn't care about ratio
   - SmartShopper: Compares multiple factors
6. Calculate perceived value with mood modifier
7. Decide quantity based on budget and preference
8. Complete purchase or leave
```

**Customer Archetypes:**

| Archetype | Price Sensitivity | Price Awareness | Budget | Behavior |
|-----------|------------------|-----------------|--------|----------|
| Bargain Hunter | High (0.7-1.0) | High (0.7-1.0) | Low ($10-30) | Only buys deals |
| Premium Buyer | Low (0.1-0.4) | Low (0.3-0.6) | High ($50-150) | Quality over price |
| Regular Shopper | Medium (0.4-0.7) | Medium (0.4-0.7) | Medium ($20-60) | Average behavior |
| Impulse Buyer | Medium (0.3-0.6) | Low (0.2-0.5) | Medium ($30-80) | Buys unplanned items |
| Smart Shopper | High (0.5-0.8) | Very High (0.8-1.0) | Medium ($40-100) | Strategic, waits for deals |
| Loyalist | Medium (0.3-0.6) | Medium (0.4-0.7) | Medium ($30-70) | Returns if treated well |

### Reputation System

**Reputation Sources:**
- Fair pricing (+0.1 per sale)
- Bargain deals (+0.3 per sale)
- Overpricing (-0.5 per sale)
- Poor service (-1.0 when customer leaves unhappy)

**Reputation Effects:**
- 0-20: Very low traffic, angry customers
- 21-40: Low traffic, skeptical customers
- 41-60: Normal traffic
- 61-80: Increased traffic, some loyal customers
- 81-100: High traffic, many loyal returning customers

**Decay:** 5% per day (encourages consistent good service)

### Progression System

**Experience & Leveling:**
- Gain XP from sales (10% of profit)
- Level up every 100 XP × current level
- Levels unlock new features

**Unlockables:**

| Level | Unlock | Benefit |
|-------|--------|---------|
| 1 | Basic Items | Bread, Milk, Coffee, Apples |
| 3 | Premium Goods | Cheese, Wine, Flowers |
| 5 | Stall Upgrade | +5 display slots |
| 7 | Price Insights | See price predictions |
| 10 | Bulk Pricing | Volume discounts |
| 12 | Market Intel | See competitor prices |
| 15 | Seasonal Items | Special event-only goods |

**Achievements:**

Hidden until unlocked, encourage exploration:
- First Customer - Make first sale
- Perfect Price - Price within 5% of optimal
- Bargain Hunter - Buy 10 items during surplus
- Market Master - Profit from a shortage
- Customer Favorite - Reach 90+ reputation
- Merchant Tycoon - Earn $1000 profit
- Economist - Witness all market conditions
- Crisis Manager - Survive 3 market events profitably

## Random Events

Events teach economic concepts through storytelling:

### Supply Shock Events
**"Supply Chain Disruption"**
- Effect: -60% supply of Bread, Milk
- Duration: 1 day
- Learning: Scarcity drives prices up

**"Harvest Festival"**
- Effect: +150% supply of Apples, produce
- Duration: 3 days
- Learning: Surplus drives prices down

### Demand Shock Events
**"Heat Wave"**
- Effect: +80% demand for beverages
- Duration: 2 days
- Learning: External factors affect demand

**"Artisan Fair"**
- Effect: +100% demand for premium goods
- Duration: 2 days
- Learning: Events create opportunities

**"Food Safety Scare"**
- Effect: -70% demand for specific item
- Duration: 3 days
- Learning: Negative news affects markets

### Competitive Events
**"New Competitor Opens"** (Future)
- Effect: -20% customer traffic
- Duration: Permanent until countered
- Learning: Competition affects business

## Tutorial Flow

### Gentle Onboarding (7 Steps)

**Step 1: Welcome**
- "You've inherited a market stall. Let's get started!"
- Goal: Open Inventory
- Learning: Interface familiarity

**Step 2: Know Your Market**
- "Smart merchants watch market prices."
- Goal: Open Market Dashboard
- Learning: Market awareness

**Step 3: Stock Your Shelves**
- "Buy low, sell high - that's the game."
- Goal: Buy 1 item wholesale
- Learning: Wholesale purchasing

**Step 4: Price It Right**
- "Find the sweet spot between profit and sales."
- Goal: Set a price on any item
- Learning: Pricing mechanics

**Step 5: Make That Money**
- "Wait for customers. If the price is right, they'll buy."
- Goal: Complete 1 sale
- Learning: Customer behavior

**Step 6: The Invisible Hand**
- "Notice prices changing? That's supply and demand."
- Goal: Witness a market condition change
- Learning: Market dynamics

**Step 7: Profit!**
- "Make $50 in profit. Track your margins!"
- Goal: Reach $150 (started with $100)
- Learning: Profit calculation

After tutorial: "You're ready! Build your merchant empire."

## UI/UX Design

### HUD (Always Visible)
- **Top Left:** Money (large, prominent)
- **Top Center:** Day & Time
- **Top Right:** Reputation (with bar)
- **Bottom:** Navigation tabs (Shop | Market | Inventory | Settings)

### Shop Screen
**Main Focus: Your Stall**
- Grid of items for sale (card-based)
- Each card shows:
  - Item icon
  - Price (editable)
  - Stock quantity
  - "Market: $X" comparison
- Customer queue on side
- Active customer speech bubbles

### Market Dashboard
**Main Focus: Intelligence**
- List of all items with market data
- Price charts (line graphs)
- Supply/Demand bars
- Condition indicators (🔴🟢📈📉)
- "Best Buy" and "Best Sell" highlights
- Event notifications

### Inventory Screen
**Main Focus: Management**
- Grid of owned items
- Stock levels
- "Buy Wholesale" button per item
- Shows wholesale vs. market price
- Capacity indicator

### Visual Feedback

**Positive Feedback:**
- ✨ Sparkles on successful sale
- 💰 Coin particles when earning money
- ⭐ Stars when reputation increases
- 🎉 Confetti on level up
- 📈 Upward arrows on price increases

**Negative Feedback:**
- 😠 Angry emoji when customer complains
- 📉 Downward arrows on price drops
- ⚠️ Warning icon for low stock
- 🔴 Red flash on failed sale

**Neutral Feedback:**
- 💬 Speech bubbles for customer thoughts
- 📰 News ticker for events
- 🔔 Notification badges
- ⏰ Clock animations for time passing

## Monetization (Future)

### Ethical F2P Model
- **No Pay-to-Win:** Can't buy better prices or advantages
- **No Energy Systems:** Play as much as you want
- **No Forced Ads:** Optional rewarded ads only

### Optional Purchases
1. **Cosmetics:** Stall decorations, customer outfits
2. **Convenience:** Extra save slots, UI themes
3. **Content:** New item packs (still balanced)
4. **Remove Ads:** One-time purchase

### Rewarded Ads (Optional)
- Watch ad → Get market insight tip
- Watch ad → Spawn premium customer
- Watch ad → Small money boost
- Limit: 3 per day

## Technical Performance Targets

### iOS Optimization
- **Target FPS:** 60 FPS
- **Load Time:** < 3 seconds to gameplay
- **Memory:** < 200 MB RAM usage
- **Battery:** < 5% per 30 min session
- **Storage:** < 100 MB initial download

### Scalability
- Support 1000+ items (future)
- Support 50+ simultaneous customers
- Support 100+ events
- Save/load in < 1 second

## Success Metrics

### Engagement
- **Session Length:** 15-30 minutes average
- **Retention D1:** >50%
- **Retention D7:** >30%
- **Retention D30:** >15%

### Learning
- **Tutorial Completion:** >80%
- **Concept Mastery:** Players demonstrate understanding through optimal strategies
- **Self-Reported Learning:** Survey shows economics knowledge increase

### Satisfaction
- **App Store Rating:** >4.5 stars
- **Reviews:** Positive mentions of "fun" and "learned"
- **Recommendations:** >40% word-of-mouth growth

## Accessibility

### Inclusive Design
- **Colorblind Mode:** Shapes + colors for market conditions
- **Font Scaling:** Support system text size preferences
- **Touch Targets:** Minimum 44×44 points (Apple HIG)
- **Haptics:** Optional tactile feedback
- **VoiceOver:** Screen reader support (future)

### Difficulty Modes
- **Casual:** Lower price sensitivity, longer event warnings
- **Normal:** Balanced (default)
- **Expert:** High sensitivity, surprise events, competitor AI

## Localization (Future)

### Phase 1 Languages
- English (primary)
- Spanish
- Mandarin
- French

### Localization Considerations
- Currency symbols
- Cultural item preferences
- Market event themes
- Customer names

## Quality Assurance

### Testing Focus Areas
1. **Economy Balance:** No exploit strategies
2. **Customer AI:** Behaviors feel realistic
3. **Tutorial:** Clear and helpful
4. **Performance:** Smooth on target devices
5. **Progression:** Unlocks at good pace

### Beta Testing
- **Target:** 50-100 players age 16-21
- **Duration:** 2 weeks
- **Metrics:** Track session length, retention, sticking points
- **Surveys:** Collect qualitative feedback

## Launch Strategy

### Soft Launch (1 month)
- Release in 1-2 countries
- Gather data and feedback
- Iterate on balance and UX
- Fix critical bugs

### Full Launch
- App Store featured pitch
- Educational institution partnerships
- Teacher resource materials
- Press kit for education/gaming media

### Post-Launch Content
- Monthly new items
- Seasonal events
- Community challenges
- Educational blog posts explaining the economics

---

**This game design prioritizes fun first, education second. But because the economics are built into the core loop, players can't help but learn. That's the magic of the invisible hand.** ✨
