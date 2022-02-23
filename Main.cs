using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using static UnityEngine.GUILayout;
using UnityEngine.Rendering;
using Steamworks;
using rail;
using xiaoye97;
using crecheng.DSPModSave;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace DSPMarker
{

    [BepInPlugin("Appun.DSP.plugin.Marker", "DSPMarker", "0.2.0")]
    [BepInProcess("DSPGAME.exe")]
    [BepInDependency(DSPModSavePlugin.MODGUID)]



    public class Main : BaseUnityPlugin, IModCanSave
    {
        //public static ConfigEntry<bool> ShowStationInfo;
        //public static ConfigEntry<int> maxCount;
        public static bool showList = true;


        public static bool alwaysDisplay = true;
        public static bool throughPlanet = true;
        public static bool ShowArrow = true;

        //public static float signHeight = 3f;
        //public static float signSize = 0f;
        //public static bool signChanged = false;
        //public static bool addSign = false;

        public static Sprite merkerStripe;
        public static Sprite roundStripe;
        public static Sprite ButtonStripe;

        public static int maxMarker = 20;
        public static int markerCount = 0;

        //public static string jsonFilePath;

        //public static bool showSignButton = true;
       

        public void Start()
        {
            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            ////configの設定
            //ShowStationInfo = Config.Bind("General", "ShowStationInfo", false, "Show Station Information");
            ////maxCount = Config.Bind("General", "maxCount", 200, "Inventory Column Count");

            //jsonFilePath = Path.Combine(Paths.ConfigPath, "markers.json");

            ////テスト
            ////GameMain.data.mainPlayer.gameObject.AddComponent<DynamicCreateMesh>();
            ///
            LogManager.Logger.LogInfo("---------------------------------------------------------load icon");

            LoadIcon();
            MarkerPrefab.Create();
            MarkerEditor.Create();
            MarkerList.Create();
            MarkerPool.Create();

        }

        //[HarmonyPatch(typeof(VFPreload), "InvokeOnLoadWorkEnded")]

        //public static class VFPreload_InvokeOnLoadWorkEnded
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(VFPreload __instance)
        //    {

        //    }
        //}

        //ロード処理
        //markerPrefab.transform.Find("round/pinBaseIcon1").gameObject.GetComponent<Image>().sprite = null;



        //イベントの作成


        public void Update()
        {
            MarkerPool.Update();
        }


        //アイコンのロード
        public static void LoadIcon()
        {
            try
            {
                var assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DSPMarker.marker"));
                if (assetBundle == null)
                {
                    LogManager.Logger.LogInfo("Icon loaded.");
                }
                else
                {
                    merkerStripe = assetBundle.LoadAsset<Sprite>("marker");
                    roundStripe = assetBundle.LoadAsset<Sprite>("round");
                    ButtonStripe = assetBundle.LoadAsset<Sprite>("mapmarker");
                    assetBundle.Unload(false);
                }
            }
            catch (Exception e)
            {
                LogManager.Logger.LogInfo("e.Message " + e.Message);
                LogManager.Logger.LogInfo("e.StackTrace " + e.StackTrace);
            }
        }




        public void Import(BinaryReader r)
        {
            LogManager.Logger.LogInfo("---------------------------------------------------------Import");

            MarkerPool.markerPool.Clear();
            int num = r.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                var Key = r.ReadInt32();
                MarkerPool.Marker marker = new MarkerPool.Marker();
                marker.pos.x = r.ReadSingle();
                marker.pos.y = r.ReadSingle();
                marker.pos.z = r.ReadSingle();
                marker.icon1ID = r.ReadInt32();
                marker.icon2ID = r.ReadInt32();
                marker.color.r = r.ReadSingle();
                marker.color.g = r.ReadSingle();
                marker.color.b = r.ReadSingle();
                marker.color.a = r.ReadSingle();
                marker.desc = r.ReadString();
                marker.alwaysDisplay = r.ReadBoolean();
                marker.throughPlanet = r.ReadBoolean();
                marker.ShowArrow = r.ReadBoolean();
                MarkerPool.markerPool.Add(Key, marker);
                MarkerList.Refresh();
                MarkerPool.Refresh();

            }
        }

        public void Export(BinaryWriter w)
        {
            LogManager.Logger.LogInfo("---------------------------------------------------------Export");

            w.Write(MarkerPool.markerPool.Count);
            foreach (KeyValuePair<int, MarkerPool.Marker> keyValuePair in MarkerPool.markerPool)
            {
                w.Write(keyValuePair.Key);
                w.Write(keyValuePair.Value.pos.x);
                w.Write(keyValuePair.Value.pos.y);
                w.Write(keyValuePair.Value.pos.z);
                w.Write(keyValuePair.Value.icon1ID);
                w.Write(keyValuePair.Value.icon2ID);
                w.Write(keyValuePair.Value.color.r);
                w.Write(keyValuePair.Value.color.g);
                w.Write(keyValuePair.Value.color.b);
                w.Write(keyValuePair.Value.color.a);
                w.Write(keyValuePair.Value.desc);
                w.Write(keyValuePair.Value.alwaysDisplay);
                w.Write(keyValuePair.Value.throughPlanet);
                w.Write(keyValuePair.Value.ShowArrow);
            }

        }

        public void IntoOtherSave()
        {
            MarkerPool.markerPool.Clear();
        }

    }


    public class LogManager
    {
        public static ManualLogSource Logger;
    }

}