# iOS Deployment Guide - The Invisible Hand

Complete guide for deploying test builds to iOS devices.

## 📋 Prerequisites Checklist

### Hardware & Software
- [ ] **Mac Computer** (macOS Monterey 12.0 or later)
- [ ] **iPhone/iPad** (iOS 13.0 or later for testing)
- [ ] **Unity 2022.3 LTS** installed
- [ ] **Xcode 14.0+** (from Mac App Store)
- [ ] **Apple Developer Account** (see options below)

### Apple Developer Accounts

**Option 1: Free Account (Development Only)**
- ✅ Can test on your own device (up to 3 devices)
- ✅ Good for initial testing
- ❌ Apps expire after 7 days
- ❌ No TestFlight distribution
- ❌ No App Store release
- **Cost:** FREE

**Option 2: Paid Developer Account (Recommended)**
- ✅ Unlimited device testing
- ✅ TestFlight beta distribution (up to 10,000 testers)
- ✅ App Store release capability
- ✅ App doesn't expire
- **Cost:** $99/year USD
- **Sign up:** https://developer.apple.com/programs/

## 🔌 Unity Packages Setup

### Install Unity iOS Build Support

1. Open **Unity Hub**
2. Go to **Installs** tab
3. Click ⚙️ on your Unity 2022.3 installation
4. Select **Add Modules**
5. Check these boxes:
   - ✅ **iOS Build Support**
   - ✅ **Mac Build Support (Mono)** (if not already installed)
   - ✅ **Documentation** (optional)
6. Click **Install**

### Update Package Manager

The `Packages/manifest.json` has been updated with:

**Core Packages:**
- `com.unity.textmeshpro` - UI text rendering
- `com.unity.ugui` - UI system
- `com.unity.mobile.notifications` - iOS notifications (future)
- `com.unity.device-simulator` - Test without device

**Optional Analytics & Services:**
- `com.unity.services.analytics` - Track player behavior
- `com.unity.services.core` - Unity Gaming Services
- `com.unity.purchasing` - In-app purchases (future)
- `com.unity.ads` - Ad monetization (future)
- `com.unity.remote-config` - Live game tuning

Unity will auto-install these when you open the project.

### Manual Package Installation (if needed)

1. Open Unity Editor
2. **Window → Package Manager**
3. Click **+** → **Add package by name**
4. Enter package name (e.g., `com.unity.services.analytics`)
5. Click **Add**

## 🛠️ Unity Project Configuration

### 1. iOS Player Settings

**File → Build Settings → iOS → Player Settings**

**Company Details:**
```
Company Name: InvisibleHandGames
Product Name: The Invisible Hand
```

**Identification:**
```
Bundle Identifier: com.invisiblehandgames.theinvisiblehand
Version: 0.1.0
Build Number: 1
```

**Minimum Requirements:**
```
Minimum iOS Version: 13.0
Target iOS Version: 17.0
Target Device: iPhone & iPad
Requires ARKit: NO
```

**Architecture:**
```
Architecture: ARM64
```

**Scripting Backend:**
```
Scripting Backend: IL2CPP
API Compatibility Level: .NET Standard 2.1
```

**Graphics:**
```
Auto Graphics API: YES (Metal default)
Color Space: Linear
```

**Orientation:**
```
Default Orientation: Auto Rotation
Allowed Orientations:
  ✅ Portrait
  ✅ Portrait Upside Down
  ✅ Landscape Left
  ✅ Landscape Right
```

**Other Settings:**
```
Accelerometer Frequency: 60 Hz
Requires Persistent WiFi: NO
Hide Home Button: NO
Status Bar Hidden: NO
Status Bar Style: Default
```

**Optimization:**
```
Script Call Optimization: Fast but no Exceptions
Vertex Compression: Mixed
Optimize Mesh Data: YES
Strip Engine Code: YES
Managed Stripping Level: Medium
```

### 2. Quality Settings

**Edit → Project Settings → Quality**

Create a **Mobile** quality preset:
```
V-Sync Count: Don't Sync
Texture Quality: Full Res
Anisotropic Textures: Per Texture
Anti Aliasing: 2x Multi Sampling
Soft Particles: NO
Shadow Resolution: Medium
Shadow Distance: 50
Pixel Light Count: 2
```

### 3. Build Settings

**File → Build Settings**

1. Click **Add Open Scenes** (if scene is ready)
2. Platform: **iOS**
3. Click **Switch Platform**
4. Click **Player Settings** to configure above

## 📱 TestFlight Distribution Setup

### Option 1: Ad-Hoc Distribution (Free Account)

**Direct device installation without App Store:**

1. Connect device via USB
2. Build from Unity
3. Open Xcode project
4. Select your device
5. Click ▶️ Run

Limitations:
- App expires in 7 days
- Max 3 devices
- Must re-install weekly

### Option 2: TestFlight (Paid Account - Recommended)

**Professional beta testing platform:**

**Setup Steps:**

1. **App Store Connect Setup:**
   - Go to https://appstoreconnect.apple.com
   - Click **My Apps** → **+** → **New App**
   - Fill in:
     - Platform: iOS
     - Name: The Invisible Hand
     - Primary Language: English
     - Bundle ID: com.invisiblehandgames.theinvisiblehand
     - SKU: theinvisiblehand001

2. **Create App Icon:**
   - Required: 1024×1024 PNG (no transparency)
   - Tools: Figma, Canva, or Photoshop
   - Temporary: Use a placeholder icon

3. **Build for TestFlight:**
   ```
   Unity → Build → Generate Xcode project
   Xcode → Product → Archive
   Xcode → Distribute → App Store Connect
   Upload build
   ```

4. **Add Testers:**
   - App Store Connect → TestFlight tab
   - Add internal testers (up to 100)
   - Or external testers (up to 10,000, requires review)
   - Testers get email with TestFlight link

5. **Testers Install:**
   - Install TestFlight app from App Store
   - Click invite link
   - Download & test your game

## 🔧 Alternative Testing Platforms

### 1. Unity Cloud Build (CI/CD)

**Automated builds on every git push:**

- **Setup:** https://unity.com/products/cloud-build
- **Cost:** Free tier available
- **Benefits:**
  - Automatic builds on commit
  - No Mac required for builds
  - Direct TestFlight integration
  - Build history

**Setup Steps:**
1. Link Unity project to Unity Cloud
2. Connect GitHub repo
3. Configure iOS build settings
4. Enable auto-build on push
5. Download .ipa or push to TestFlight

### 2. Firebase App Distribution

**Google's alternative to TestFlight:**

- **Setup:** https://firebase.google.com/products/app-distribution
- **Cost:** Free
- **Benefits:**
  - Easier than TestFlight
  - Android + iOS support
  - No Apple review for testers
  - Crash reporting included

**Setup Steps:**
```bash
# Install Firebase CLI
npm install -g firebase-tools

# Login
firebase login

# Initialize Firebase in project
firebase init

# Add Firebase Unity SDK (optional)
# Download from: https://firebase.google.com/download/unity
```

**Package Manager:**
```
Add: com.google.firebase.analytics
Add: com.google.firebase.crashlytics
```

### 3. App Center (Microsoft)

**Another testing distribution option:**

- **Setup:** https://appcenter.ms
- **Cost:** Free tier available
- **Benefits:**
  - iOS + Android
  - Analytics & crash reporting
  - Easy tester management
  - CI/CD integration

## 📊 Analytics & Monitoring Setup

### Unity Analytics (Built-in)

**Enable in Unity:**

1. **Window → Services → Analytics**
2. Link to Unity Cloud Project
3. Enable Analytics
4. Add to code:

```csharp
using Unity.Services.Core;
using Unity.Services.Analytics;

async void Start()
{
    await UnityServices.InitializeAsync();

    // Track events
    AnalyticsService.Instance.CustomData("game_started", new Dictionary<string, object>
    {
        { "day", GameManager.Instance.CurrentDay },
        { "money", PlayerInventory.Instance.Money }
    });
}
```

**Dashboard:** https://dashboard.unity3d.com

### Firebase Analytics (Advanced)

**Better insights & funnels:**

1. Create Firebase project
2. Add iOS app with Bundle ID
3. Download `GoogleService-Info.plist`
4. Import Firebase Unity SDK
5. Add plist to Xcode project

```csharp
using Firebase.Analytics;

void TrackPurchase(string item, float price)
{
    FirebaseAnalytics.LogEvent("item_purchased",
        new Parameter("item_name", item),
        new Parameter("price", price)
    );
}
```

## 🚀 Deployment Workflow

### First Time Setup (One-time)

```bash
# 1. Ensure Unity iOS module installed
# 2. Open project in Unity
# 3. Configure iOS Player Settings (see above)
# 4. Switch platform to iOS
```

### Build Process (Each Update)

**Option A: Manual Build**

```bash
# 1. Unity: File → Build Settings → Build
# 2. Choose output folder: Builds/iOS/
# 3. Wait for build (5-10 minutes)
# 4. Open .xcodeproj in Xcode
# 5. Select signing team
# 6. Product → Archive
# 7. Distribute → TestFlight
```

**Option B: Automated Script**

Create `Editor/BuildScript.cs`:

```csharp
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    [MenuItem("Build/iOS Build")]
    public static void BuildIOS()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        buildPlayerOptions.locationPathName = "Builds/iOS";
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + report.summary.totalSize + " bytes");
        }
        else
        {
            Debug.LogError("Build failed");
        }
    }
}
```

Then: **Build → iOS Build** from Unity menu

### Version Management

**For each new test build:**

```
Version: 0.1.0 → 0.1.1 → 0.2.0 (feature updates)
Build Number: Auto-increment (1, 2, 3, 4...)
```

Update in: **Edit → Project Settings → Player → Version**

## 🧪 Testing Checklist

### Before Submitting to TestFlight

- [ ] Game launches without crashes
- [ ] All core systems work (buy/sell/economy)
- [ ] Tutorial completes successfully
- [ ] No critical bugs in logs
- [ ] UI scales properly on different devices
- [ ] Performance is 60 FPS on target device
- [ ] No placeholder text/graphics (or mark as WIP)
- [ ] Build size is reasonable (<100MB)

### Device Testing Matrix

**Minimum Test Devices:**
- [ ] iPhone SE (small screen, older hardware)
- [ ] iPhone 14 Pro (modern, notch)
- [ ] iPad (large screen, different aspect ratio)

**iOS Versions:**
- [ ] iOS 13.0 (minimum)
- [ ] iOS 17.0 (latest)

### Beta Testing Metrics

**Track with analytics:**
- Session length (target: 15-30 min)
- Crash rate (target: <1%)
- Tutorial completion (target: >80%)
- Day 1 retention (target: >50%)
- Day 7 retention (target: >30%)

## 🐛 Common Deployment Issues

### "Code Signing Error"
**Solution:**
- Xcode → Signing & Capabilities
- Check "Automatically manage signing"
- Select your team

### "Build Failed - IL2CPP"
**Solution:**
- Update Xcode to latest
- Check Unity version compatibility
- Try: Edit → Preferences → External Tools → Regenerate project files

### "App Crashes on Launch"
**Solution:**
- Check Xcode console for errors
- Verify all required frameworks included
- Test in Unity Device Simulator first

### "Can't Install on Device"
**Solution:**
- Check device iOS version >= minimum
- Check provisioning profile includes device UDID
- Trust developer certificate on device: Settings → General → VPN & Device Management

### "TestFlight Says 'Invalid Binary'"
**Solution:**
- Increment build number
- Check all required metadata in App Store Connect
- Verify Bundle ID matches exactly

## 📞 Support Resources

**Unity Documentation:**
- iOS Build: https://docs.unity3d.com/Manual/iphone-GettingStarted.html
- IL2CPP: https://docs.unity3d.com/Manual/IL2CPP.html

**Apple Documentation:**
- TestFlight: https://developer.apple.com/testflight/
- App Store Connect: https://help.apple.com/app-store-connect/

**Community:**
- Unity Forums: https://forum.unity.com
- Stack Overflow: [unity3d] + [ios] tags
- Discord: Unity Developer Community

## 🎯 Quick Start (TL;DR)

**Fastest path to testing:**

1. **Install:** Unity iOS Build Support
2. **Configure:** iOS Player Settings (Bundle ID!)
3. **Build:** File → Build Settings → iOS → Build
4. **Open:** Generated .xcodeproj in Xcode
5. **Sign:** Select your team in Signing
6. **Run:** Connect device, click ▶️

**For TestFlight:**
7. **Archive:** Product → Archive in Xcode
8. **Upload:** Distribute → App Store Connect
9. **Add Testers:** App Store Connect → TestFlight
10. **Test:** Testers download via TestFlight app

---

**You're now ready to deploy test builds and gather feedback! Good luck! 🚀**
