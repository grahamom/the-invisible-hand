# Quick Start Guide - Deploy Test Build in 30 Minutes

Get your economics game on an iPhone/iPad for testing ASAP.

## ⚡ Prerequisites (15 min)

### 1. Install Required Software

**On Mac:**
```bash
# Install Xcode from App Store (if not installed)
# Launch Xcode once to accept license

# Verify Xcode command line tools
xcode-select --install
```

**Unity Hub:**
- Download from https://unity.com/download
- Install Unity 2022.3 LTS
- Add module: **iOS Build Support**

### 2. Apple Developer Account

**Quick Option - Free Account:**
- Use your existing Apple ID
- ✅ Can test on your device today
- ❌ App expires in 7 days
- ❌ No TestFlight

**Better Option - Paid ($99/year):**
- Sign up at https://developer.apple.com/programs/
- ✅ TestFlight for up to 10,000 testers
- ✅ App Store publishing capability
- ✅ No expiration

## 🚀 Build & Deploy (15 min)

### Step 1: Open Project in Unity

```bash
# Clone if you haven't
git clone https://github.com/grahamom/the-invisible-hand.git
cd the-invisible-hand

# Open Unity Hub → Add → Select this folder
# Open with Unity 2022.3 LTS
```

### Step 2: Configure iOS Settings (Automated!)

**In Unity Editor:**

1. **Switch to iOS Platform**
   - File → Build Settings
   - Platform: iOS
   - Click "Switch Platform" (wait 2-5 min)

2. **Auto-Configure Settings (One Click!)**
   - Unity Menu: **Build → Configure for Development**
   - This automatically sets:
     - Bundle Identifier: `com.invisiblehandgames.theinvisiblehand`
     - iOS version: 13.0 minimum
     - Architecture: ARM64
     - Scripting Backend: IL2CPP
     - Development mode: Enabled

3. **Verify Settings (Optional)**
   - Unity Menu: **Build → Show Current Settings**
   - Check console for current configuration

4. **Customize Bundle ID (If Needed)**
   - Edit → Project Settings → Player → iOS tab
   - Bundle Identifier: `com.YOUR_NAME.theinvisiblehand`
   - ⚠️ Must be unique if publishing to App Store!

### Step 3: Build for iOS

**Option A: Use Our Build Tool (Recommended)**
```
Unity Menu: Build → iOS Development Build
```
This will:
- Create optimized build
- Open Builds/iOS folder
- Show next steps

**Option B: Manual Build**
```
File → Build Settings → Build
Choose folder: Builds/iOS
Wait 5-10 minutes
```

### Step 4: Deploy to Device

**Connect iPhone/iPad via USB**

**Open Xcode:**
```bash
# Navigate to build folder
cd Builds/iOS

# Open Xcode project
open Unity-iPhone.xcodeproj
```

**In Xcode:**

1. **Select Device**
   - Top bar: Click "Any iOS Device"
   - Choose your connected iPhone/iPad

2. **Configure Signing**
   - Click project name (left sidebar)
   - Select "Unity-iPhone" target
   - Signing & Capabilities tab
   - Check ☑️ "Automatically manage signing"
   - Team: Select your Apple ID

3. **Trust Developer on Device**
   - First time only: iPhone → Settings → General → VPN & Device Management
   - Trust your developer certificate

4. **Run!**
   - Click ▶️ Play button (or Cmd+R)
   - Wait 2-3 minutes
   - App launches on your device!

## 📱 TestFlight (Paid Account Only)

For distributing to testers:

### One-Time Setup (10 min)

**1. App Store Connect:**
- Visit https://appstoreconnect.apple.com
- My Apps → + → New App
  - Name: The Invisible Hand
  - Bundle ID: (your bundle ID from Unity)
  - SKU: invisible-hand-001

**2. Upload Build:**
```
Xcode → Product → Archive (wait 10 min)
Window → Organizer → Distribute App → App Store Connect
Upload → Done (wait 5 min processing)
```

**3. Add Testers:**
```
App Store Connect → TestFlight tab
Internal Testing → + → Add names/emails
Testers get email invite
```

**4. Testers Download:**
```
Install TestFlight app from App Store
Open invite email → Accept → Install game
```

## 🔄 Update Workflow

**For each new version:**

1. **Increment build:**
   ```
   Unity: Build → Increment Build Number
   ```

2. **Build:**
   ```
   Unity: Build → iOS Development Build
   ```

3. **Upload:**
   ```
   Xcode → Product → Archive → Distribute
   ```

4. **Notify testers:**
   ```
   TestFlight auto-notifies when build ready
   ```

## 🐛 Troubleshooting

### "No Code Signing Identities Found"
**Fix:**
```
Xcode → Preferences → Accounts → Add Apple ID
Signing → Select your team
```

### "Failed to Code Sign"
**Fix:**
```
iPhone → Settings → General → Device Management
Trust your developer certificate
```

### "Build Failed in Unity"
**Fix:**
```
Build → Configure for Development (reset settings)
Try again
```

### "App Crashes on Launch"
**Fix:**
```
Xcode → View → Debug Area → Console
Check error messages
Usually missing framework or asset issue
```

### "Can't Find My Device"
**Fix:**
```
- Unlock iPhone
- Trust this computer (popup on iPhone)
- Check cable connection
- Window → Devices and Simulators (Xcode)
```

## 📊 Testing Checklist

### Before Sending to Testers

- [ ] App launches without crash
- [ ] Tutorial works start to finish
- [ ] Can buy items from wholesale
- [ ] Can set prices
- [ ] Customers appear and buy
- [ ] Money increases on sales
- [ ] Day/night cycle works
- [ ] No major UI bugs
- [ ] Performance is smooth (60 FPS)

### Ask Testers to Check

- [ ] First impression (confusing? fun?)
- [ ] Tutorial clarity
- [ ] Difficulty balance
- [ ] UI/UX issues
- [ ] Crashes or freezes
- [ ] Performance problems
- [ ] What they liked
- [ ] What confused them

## 🎯 Success!

If you can:
- ✅ See the app on your device
- ✅ Complete tutorial
- ✅ Buy and sell items
- ✅ Observe market changes

**You're ready for beta testing!** 🎉

## 📚 Next Steps

- **Read DEPLOYMENT.md** for advanced options
- **Check GAME_DESIGN.md** for balancing ideas
- **Add visual assets** (sprites, icons, UI)
- **Gather feedback** from testers
- **Iterate** on balance and UX

## 🆘 Need Help?

- **Deployment Guide:** See DEPLOYMENT.md
- **Technical Docs:** See README.md
- **Unity Forums:** https://forum.unity.com
- **Stack Overflow:** Tag [unity3d] + [ios]

---

**Time to see your economics game in action! Good luck! 🚀**
