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

    [BepInPlugin("Appun.DSP.plugin.Marker", "DSPMarker", "0.0.4")]
    [BepInProcess("DSPGAME.exe")]
    [BepInDependency(DSPModSavePlugin.MODGUID)]



    public class Main : BaseUnityPlugin, IModCanSave
    {



        //public static ConfigEntry<bool> ShowStationInfo;
        public static ConfigEntry<bool> DisableKeyTips;
        //public static ConfigEntry<bool> alwaysDisplay;
        //public static ConfigEntry<bool> throughPlanet;
        //public static ConfigEntry<bool> ShowArrow;
        public static ConfigEntry<int> maxMarkers;
        public static bool showList = true;


        //public static float signHeight = 3f;
        //public static float signSize = 0f;
        //public static bool signChanged = false;
        //public static bool addSign = false;

        public static Sprite arrowSprite;
        public static Sprite merkerSprite;
        public static Sprite roundSprite;
        public static Sprite ButtonSprite;

        public static int maxMarker;
        public static int markerCount = 0;

        //public static string jsonFilePath;

        //public static bool showSignButton = true;


        public void Start()
        {
            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            ////configの設定
            DisableKeyTips = Config.Bind("UI", "DisableKeyTips", false, "Disable Key Tips on right side");
            maxMarkers = Config.Bind("Marker", "maxMarker", 20, "Maximum number of markers");
            maxMarker = maxMarkers.Value;
            //alwaysDisplay = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            //throughPlanet = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            //ShowArrow = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            ////maxCount = Config.Bind("General", "maxCount", 200, "Inventory Column Count");

            //jsonFilePath = Path.Combine(Paths.ConfigPath, "markers.json");

            ////テスト
            ////GameMain.data.mainPlayer.gameObject.AddComponent<DynamicCreateMesh>();
            ///
            //LogManager.Logger.LogInfo("---------------------------------------------------------load icon");

            LoadIcon();
            MarkerPrefab.Create();
            MarkerEditor.Create();
            MarkerList.Create();
            MarkerPool.Create();
            //ArrowPool.Create();
            MarkerPrefab.markerGroup.SetActive(false);
            MarkerList.listBase.SetActive(false);

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
            if (GameMain.data == null)
            {
                return;
            }
            //LogManager.Logger.LogInfo("---------------------------------------------------------GameMain.data != null ");

            //LogManager.Logger.LogInfo("---------------------------------------------------------GameMain.localPlanet != null ");

            //LogManager.Logger.LogInfo("---------------------------------------------------------GameMain.localPlanet : " + GameMain.localPlanet.id);
            if (DSPGame.Game != null && DSPGame.Game.isMenuDemo || !GameMain.isRunning)
            {
                return;
            }

            if (GameMain.localPlanet == null)
            {
                MarkerList.markerList.SetActive(false);
                MarkerPrefab.markerGroup.SetActive(false);
                //LogManager.Logger.LogInfo("---------------------------------------------------------GameMain.localPlanet : non");
                return;
            }

            if (UIGame.viewMode == EViewMode.Starmap || UIGame.viewMode == EViewMode.MilkyWay || UIGame.viewMode == EViewMode.DysonEditor)
            // if (UIGame.viewMode != EViewMode.Sail && UIGame.viewMode != EViewMode.Normal && UIGame.viewMode != EViewMode.Globe && UIGame.viewMode != EViewMode.Build)
            {
                MarkerList.markerList.SetActive(false);
                MarkerPrefab.markerGroup.SetActive(false);

                return;
            }
            //LogManager.Logger.LogInfo("---------------------------------------------------------UIGame.viewMode ");
            //LogManager.Logger.LogInfo("---------------------------------------------------------DSPGame.Game != null ");
            //if (GameMain.data.mainPlayer.sailing)
            //{
            //    MarkerPrefab.markerGroup.SetActive(false);
            //    MarkerList.listBase.SetActive(false);

            //    //LogManager.Logger.LogInfo("---------------------------------------------------------sailing ");
            //    return;
            //}
            //LogManager.Logger.LogInfo("---------------------------------------------------------no sailing ");
            MarkerList.markerList.SetActive(true);

            MarkerPrefab.markerGroup.SetActive(true);
            //MarkerList.listBase.SetActive(true);

            //LogManager.Logger.LogInfo("---------------------------------------------------------update ");

            MarkerPool.Update();
            ArrowPool.Update();
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
                    arrowSprite = assetBundle.LoadAsset<Sprite>("arrow");
                    merkerSprite = assetBundle.LoadAsset<Sprite>("marker");
                    roundSprite = assetBundle.LoadAsset<Sprite>("round");
                    ButtonSprite = assetBundle.LoadAsset<Sprite>("mapmarker");
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
            //MarkerPool.countInPlanet.Clear();
            MarkerPool.markerPool.Clear();
            MarkerPool.markerIdInPlanet.Clear();

            if (r.ReadInt32() == 1)
            {
                //markerCursorの読み込み
                MarkerPool.markerCursor = r.ReadInt32();

                //markerIdInPlanetの読み込み
                int num = r.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    var Key = r.ReadInt32();
                    var num2 = r.ReadInt32();
                    List<int> list = new List<int>();
                    for (int j = 0; j < num2; j++)
                    {
                        list.Add(r.ReadInt32());
                    }
                    MarkerPool.markerIdInPlanet.Add(Key, list);
                }
                //markerIdInPlanetの読み込み
                int num3 = r.ReadInt32();
                for (int i = 0; i < num3; i++)
                {
                    var Key = r.ReadInt32();
                    MarkerPool.Marker marker = new MarkerPool.Marker();
                    marker.planetID = r.ReadInt32();
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
                }
                MarkerPool.Refresh();
                MarkerList.Refresh();
            }
            else
            {
                LogManager.Logger.LogInfo("Save data version error");
            }

            //MarkerList.Reset();

        }

        public void Export(BinaryWriter w)
        {
            LogManager.Logger.LogInfo("---------------------------------------------------------Export");
            w.Write(1); //セーブデータバージョン
            //markerCursorの書き込み
            w.Write(MarkerPool.markerCursor);
            //markerIdInPlanetの書き込み
            w.Write(MarkerPool.markerIdInPlanet.Count);
            foreach (KeyValuePair<int, List<int>> keyValuePair in MarkerPool.markerIdInPlanet)
            {
                w.Write(keyValuePair.Key);
                w.Write(keyValuePair.Value.Count);
                foreach (int markerId in keyValuePair.Value)
                {
                    w.Write(markerId);
                }

            }
            //markerPoolの書き込み
            w.Write(MarkerPool.markerPool.Count);
            foreach (KeyValuePair<int, MarkerPool.Marker> keyValuePair in MarkerPool.markerPool)
            {
                w.Write(keyValuePair.Key);
                w.Write(keyValuePair.Value.planetID);
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
            if (MarkerPool.markerPool.Count > 0)
            {
                MarkerPool.markerIdInPlanet.Clear();
                MarkerPool.markerPool.Clear();
                MarkerPool.Refresh();
                MarkerList.Refresh();
            }
        }
    }


    public class LogManager
    {
        public static ManualLogSource Logger;
    }

}