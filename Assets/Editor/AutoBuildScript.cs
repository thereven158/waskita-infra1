using System;
using System.Linq;
using UnityEditor;

namespace Agate.Editor.UnityCI
{
    public static class AutoBuildScript
    {
        // Define private variable for store value from Command Line
        private static string fileName = null;
        private static string androidSdkPath = null;
        private static string androidJdkPath = null;
        private static string androidNdkPath = null;
        private static string androidKeyStorePath = null;
        private static string androidKeyStorePass = null;
        private static string androidKeyAliasName = null;
        private static string androidKeyAliasPass = null;
        private static string defineSymbol = null;
        private static string[] assetPathNames;

        // Get all value from Command Line and store to variable
        private static void GetArgsVariable()
        {
            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                string CommandArgs = args[i];

                switch (CommandArgs)
                {
                    case "-fileNameExt":
                        i++;
                        fileName = $"{PlayerSettings.productName}_{args[i]}";
                        break;
                    case "-androidSdkPath":
                        i++;
                        androidSdkPath = args[i];
                        break;
                    case "-androidJdkPath":
                        i++;
                        androidJdkPath = args[i];
                        break;
                    case "-androidNdkPath":
                        i++;
                        androidNdkPath = args[i];
                        break;
                    case "-androidKeyStorePath":
                        i++;
                        androidKeyStorePath = args[i];
                        break;
                    case "-androidKeyStorePass":
                        i++;
                        androidKeyStorePass = args[i];
                        break;
                    case "-androidKeyAliasName":
                        i++;
                        androidKeyAliasName = args[i];
                        break;
                    case "-androidKeyAliasPass":
                        i++;
                        androidKeyAliasPass = args[i];
                        break;
                    case "-defineSymbol":
                        i++;
                        defineSymbol = args[i];
                        break;
                    case "-assetPathNames":
                        i++;
                        assetPathNames = args[i].Split(new[] { ";" }, StringSplitOptions.None);
                        break;
                }
            }
        }

        // Get all scenes from the project
        private static string[] GetScenePaths()
        {
            return EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        }

        // Setup the player setting for the project
        private static void BuildPlayer(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, string outputPath)
        {
            // Switch active build target
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);

            // Disable Unity Logo
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbol);

            // Setup BuildPlayerOptions and BuildProject
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = outputPath,
                targetGroup = buildTargetGroup,
                target = buildTarget,
                options = BuildOptions.None
            };

            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        // Build function for Android
        public static void BuildAndroid()
        {
            GetArgsVariable();
            string outputPath = "Builds/Android/" + fileName + ".apk";

            ConfigureAndroidKeystore();
            ConfigureAndroidBuildSystem();

            BuildPlayer(BuildTargetGroup.Android, BuildTarget.Android, outputPath);
        }

        private static void ConfigureAndroidBuildSystem()
        {

            // Set Android SDK & JDK path for Unity
            EditorPrefs.SetString("AndroidSdkRoot", androidSdkPath);
            EditorPrefs.SetString("AndroidJdkRoot", androidJdkPath);
            EditorPrefs.SetString("AndroidNdkRoot", androidNdkPath);
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        }

        private static void ConfigureAndroidKeystore()
        {
            // Set project keystore and keyalias to this line
            PlayerSettings.Android.keystoreName = androidKeyStorePath;
            PlayerSettings.Android.keystorePass = androidKeyStorePass;
            PlayerSettings.Android.keyaliasName = androidKeyAliasName;
            PlayerSettings.Android.keyaliasPass = androidKeyAliasPass;
        }

        public static void BuildAAB()
        {
            EditorUserBuildSettings.buildAppBundle = true;
            GetArgsVariable();
            string outputPath = "Builds/AAB/" + fileName + ".aab";

            ConfigureAndroidKeystore();
            ConfigureAndroidBuildSystem();

            BuildPlayer(BuildTargetGroup.Android, BuildTarget.Android, outputPath);
        }

        // Build function for iOS
        public static void BuildiOS()
        {
            GetArgsVariable();
            string outputPath = "Builds/iOS";

            BuildPlayer(BuildTargetGroup.iOS, BuildTarget.iOS, outputPath);
        }

        // Build function for Windows Desktop
        public static void BuildWindows()
        {
            GetArgsVariable();
            string outputPath = "Builds/Windows/" + fileName + ".exe";

            BuildPlayer(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows, outputPath);
        }

        // Build function for Linux
        public static void BuildLinux()
        {
            GetArgsVariable();
            string outputPath = "Builds/Linux/" + fileName;

            BuildPlayer(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinuxUniversal, outputPath);
        }

        // Build function for Export UnityPackage
        public static void BuildUnityPackage()
        {
            GetArgsVariable();
            string outputPath = "Builds/UnityPackage/" + fileName + ".unitypackage";

            AssetDatabase.ExportPackage(assetPathNames, outputPath, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
        }
    }
}
