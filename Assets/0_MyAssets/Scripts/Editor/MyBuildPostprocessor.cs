#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System;
using UnityEditor.Build;

public class MyBuildPostprocessor : IPreprocessBuild
{
    // 実行順
    public int callbackOrder { get { return 0; } }


    // ビルド前処理
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        // SetPlayserSetting();
    }

    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(pathToBuiltProject);
        //SetPlayserSetting();
    }

    /*
       [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        string projectPath = PBXProject.GetPBXProjectPath(path);

        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        string target = pbxProject.TargetGuidByName("Unity-iPhone");


        pbxProject.AddCapability(target, PBXCapabilityType.InAppPurchase);

        // Plistの設定のための初期化
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // 文字列の設定
        plist.root.SetString("GADApplicationIdentifier", "ca-app-pub-5360625587484429~3693067110");



        //bundleId
        string bundleId = Debug.isDebugBuild ? "com.brick.games.dev" : "com.brick.games";
        pbxProject.SetBuildProperty(target, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);

        string signStyle = Debug.isDebugBuild ? "Manual" : "Automatic";
        //プロビジョニングファイルを手動にする設定
        pbxProject.SetBuildProperty(target, "CODE_SIGN_STYLE", signStyle);

        if (Debug.isDebugBuild)
        {
            //これを設定しない場合、プロビジョニングファイルと紐づいていない証明書が選ばれることがありました。
            string codesignName = "Apple Distribution: daisuke kawano (8563CX263K)";
            pbxProject.SetBuildProperty(target, "CODE_SIGN_IDENTITY", codesignName);

            //provisioning Profileの指定
            string provisioningProfileName = Debug.isDebugBuild ? "brick Ad Hoc" : "";
            pbxProject.SetBuildProperty(target, "PROVISIONING_PROFILE_SPECIFIER", provisioningProfileName);

        }

        //アプリ名
        string dateName = DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2");
        string timeName = DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2");
        string appName = Debug.isDebugBuild ? $"{dateName}_debug" : "Block Crusher";
        plist.root.SetString("CFBundleDisplayName", appName);

        //バージョン
        string version = "2.8";
        plist.root.SetString("CFBundleShortVersionString", version);

        //apple id
        string appleDeveloperTeamID = Debug.isDebugBuild ? "8563CX263K" : "SU25JZCYM7";
        pbxProject.SetBuildProperty(target, "DEVELOPMENT_TEAM", appleDeveloperTeamID);

        //ipa名
        string buildMode = Debug.isDebugBuild ? "debug" : "release";
        string name = $"brick_{buildMode}_ver{version}_{dateName}_{timeName}";
        Debug.LogError($"~~~~~~~~~~~~~~~\n{name}\n~~~~~~~~~~~~~~~");

        // ビルド番号
        var buildKey = "CFBundleVersion";
        plist.root.SetString(buildKey, "1.0");

        PlistElementDict rootDict = plist.root;
        string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
        if (rootDict.values.ContainsKey(exitsOnSuspendKey))
        {
            rootDict.values.Remove(exitsOnSuspendKey);
        }

        // 設定を反映
        plist.WriteToFile(plistPath);

        string[] fileNames = {
            };

        foreach (var fileName in fileNames)
        {
            pbxProject.AddFrameworkToProject(target, fileName, false);
        }
        pbxProject.WriteToFile(projectPath);
    }
    */



}
#endif