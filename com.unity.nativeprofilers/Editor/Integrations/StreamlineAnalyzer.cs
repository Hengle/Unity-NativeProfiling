using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.UIElements;

namespace Unity.NativeProfiling
{
    public class StreamlineAnalyzerIntegration : NativeTool
    {
        public string Name
        {
            get { return "Arm Streamline Analyzer"; }
        }

        public string[] RequiredPackages
        {
            get { return null; }
        }

        public IEnumerable<NativeToolPhase> GetPhases()
        {
            yield return new ValidationCollectionPhase("Unity project setup", new ValidationCollectionPhase.ValidationParam[] {
                new ValidationCollectionPhase.ValidationParam("Active target - Android", () => EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android, () => { EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android); }),
                new ValidationCollectionPhase.ValidationParam("Gradle Export", () => EditorUserBuildSettings.exportAsGoogleAndroidProject, () => { EditorUserBuildSettings.exportAsGoogleAndroidProject = true; }),
                new ValidationCollectionPhase.ValidationParam("Minification mode", () => EditorUserBuildSettings.androidDebugMinification == AndroidMinification.Proguard, () => { EditorUserBuildSettings.androidDebugMinification = AndroidMinification.Proguard; }),
                new ValidationCollectionPhase.ValidationParam("Development mode", () => EditorUserBuildSettings.development == false, () => { EditorUserBuildSettings.development = false; }),
                new ValidationCollectionPhase.ValidationParam("Scripting Backend", () => PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.IL2CPP, () => { PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP); }),
                new ValidationCollectionPhase.ValidationParam("Internet permissions", () => PlayerSettings.Android.forceInternetPermission, () => { PlayerSettings.Android.forceInternetPermission = true; }),
                new ValidationCollectionPhase.ValidationParam("Force SD Card permissions", () => PlayerSettings.Android.forceSDCardPermission, () => { PlayerSettings.Android.forceSDCardPermission = true; }),
                new ValidationCollectionPhase.ValidationParam("Installation location - external", () => PlayerSettings.Android.preferredInstallLocation == AndroidPreferredInstallLocation.PreferExternal, () => { PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.PreferExternal; }),
#if UNITY_2017_3_OR_NEWER
                new ValidationCollectionPhase.ValidationParam("Limit to ARM v7 target", () => PlayerSettings.Android.targetArchitectures == AndroidArchitecture.ARMv7, () => { PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7; }),
#else
                new ValidationCollectionPhase.ValidationParam("Limit to ARM v7 target", () => { return PlayerSettings.Android.targetDevice == AndroidTargetDevice.ARMv7; }, () => { PlayerSettings.Android.targetDevice = AndroidTargetDevice.ARMv7; } ),
#endif
#if UNITY_2018_3_OR_NEWER
                new ValidationCollectionPhase.ValidationParam("Stripping level", () => PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android) == ManagedStrippingLevel.Low, () => { PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Low); }),
#else
                new ValidationCollectionPhase.ValidationParam("Stripping level", () => PlayerSettings.strippingLevel == StrippingLevel.Disabled, () => { PlayerSettings.strippingLevel = StrippingLevel.Disabled; } ),
#endif
                new ValidationCollectionPhase.ValidationParam("Engine code stripping", () => !PlayerSettings.stripEngineCode, () => { PlayerSettings.stripEngineCode = false; }),
            });

            yield return new ValidationCollectionPhase("Phone setup", new ValidationCollectionPhase.ValidationParam[] {
            });

            yield return new InstructionPhase();

        }
    }
}
