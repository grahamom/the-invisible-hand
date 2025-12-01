using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

namespace TheInvisibleHand.Editor
{
    /// <summary>
    /// Automated build tools for iOS deployment
    /// Usage: Build → iOS Release Build
    /// </summary>
    public class BuildAutomation
    {
        private const string BUILD_PATH = "Builds/iOS";
        private const string SCENE_PATH = "Assets/Scenes/MainScene.unity";

        [MenuItem("Build/iOS Development Build")]
        public static void BuildIOSDevelopment()
        {
            BuildIOS(true);
        }

        [MenuItem("Build/iOS Release Build")]
        public static void BuildIOSRelease()
        {
            BuildIOS(false);
        }

        private static void BuildIOS(bool development)
        {
            Debug.Log("=== Starting iOS Build ===");

            // Ensure build directory exists
            if (!Directory.Exists(BUILD_PATH))
            {
                Directory.CreateDirectory(BUILD_PATH);
            }

            // Build player options
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { SCENE_PATH },
                locationPathName = BUILD_PATH,
                target = BuildTarget.iOS,
                options = development
                    ? BuildOptions.Development | BuildOptions.AllowDebugging
                    : BuildOptions.None
            };

            // Perform build
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"✅ Build succeeded!");
                Debug.Log($"   Size: {summary.totalSize / 1024 / 1024} MB");
                Debug.Log($"   Time: {summary.totalTime}");
                Debug.Log($"   Output: {BUILD_PATH}");
                Debug.Log("\nNext Steps:");
                Debug.Log("1. Open the Xcode project in: " + Path.GetFullPath(BUILD_PATH));
                Debug.Log("2. Select your signing team");
                Debug.Log("3. Product → Archive → Distribute");

                // Open build folder
                EditorUtility.RevealInFinder(BUILD_PATH);
            }
            else
            {
                Debug.LogError($"❌ Build failed: {summary.result}");

                // Log errors
                foreach (var step in report.steps)
                {
                    foreach (var message in step.messages)
                    {
                        if (message.type == LogType.Error || message.type == LogType.Exception)
                        {
                            Debug.LogError($"  - {message.content}");
                        }
                    }
                }
            }
        }

        [MenuItem("Build/Increment Build Number")]
        public static void IncrementBuildNumber()
        {
            string currentVersion = PlayerSettings.bundleVersion;
            int buildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
            buildNumber++;
            PlayerSettings.iOS.buildNumber = buildNumber.ToString();

            Debug.Log($"Build number incremented to: {buildNumber} (Version: {currentVersion})");
        }

        [MenuItem("Build/Configure for Development")]
        public static void ConfigureForDevelopment()
        {
            // Development settings
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)AppleMobileArchitecture.ARM64);
            PlayerSettings.iOS.targetOSVersionString = "13.0";

            // Enable development features
            EditorUserBuildSettings.development = true;
            EditorUserBuildSettings.allowDebugging = true;

            Debug.Log("✅ Configured for Development");
        }

        [MenuItem("Build/Configure for Release")]
        public static void ConfigureForRelease()
        {
            // Production settings
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)AppleMobileArchitecture.ARM64);
            PlayerSettings.iOS.targetOSVersionString = "13.0";

            // Disable development features
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.allowDebugging = false;

            // Enable optimizations
            PlayerSettings.stripEngineCode = true;
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Medium);

            Debug.Log("✅ Configured for Release");
        }

        [MenuItem("Build/Show Current Settings")]
        public static void ShowCurrentSettings()
        {
            Debug.Log("=== Current Build Settings ===");
            Debug.Log($"Product Name: {PlayerSettings.productName}");
            Debug.Log($"Bundle ID: {PlayerSettings.applicationIdentifier}");
            Debug.Log($"Version: {PlayerSettings.bundleVersion}");
            Debug.Log($"Build Number: {PlayerSettings.iOS.buildNumber}");
            Debug.Log($"Min iOS: {PlayerSettings.iOS.targetOSVersionString}");
            Debug.Log($"Architecture: {PlayerSettings.GetArchitecture(BuildTargetGroup.iOS)}");
            Debug.Log($"Scripting Backend: {PlayerSettings.GetScriptingBackend(BuildTargetGroup.iOS)}");
            Debug.Log($"Development Build: {EditorUserBuildSettings.development}");
            Debug.Log($"Strip Engine Code: {PlayerSettings.stripEngineCode}");
        }
    }
}
