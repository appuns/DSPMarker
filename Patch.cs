using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Security;
using System.Security.Permissions;

namespace DSPMarker
{
    [HarmonyPatch]
    internal class Patch
    {
        //オプション変更後の情報ウインドウ位置修正
        [HarmonyPostfix, HarmonyPatch(typeof(UIOptionWindow), "ApplyOptions")]
        public static void UIOptionWindowa_ApplyOptions_Postfix(UIOptionWindow __instance)
        {
            //UI解像度計算
            int UIheight = DSPGame.globalOption.uiLayoutHeight;
            int UIwidth = UIheight * Screen.width / Screen.height;

            //位置の調整
            MarkerList.markerList.transform.localPosition = new Vector3(UIwidth / 2 - 60, UIheight / 2 - 70, 0);
            //MarkerList.listBase.transform.localPosition = new Vector3(UIwidth / 2 - 60, UIheight / 2 - 70, 0);
            int maxRow = (UIheight - 270 - 115) / MarkerList.boxSize;
            for (int i = 0; i < Main.maxMarker; i++)
            {
                float scale = (float)MarkerList.boxSize / 70;
                MarkerList.boxMarker[i].transform.localScale = new Vector3(scale, scale, 1);
                int x = 20 - (MarkerList.boxSize + 3) * (i / maxRow);
                int y = -68 - (MarkerList.boxSize + 3) * (i % maxRow);
                MarkerList.boxMarker[i].transform.localPosition = new Vector3(x, y, 0);

            }
        }

        //キーチップの非表示
        [HarmonyPrefix, HarmonyPatch(typeof(UIKeyTips), "_OnUpdate")]
        public static bool UIKeyTips_OnUpdate_Prefix(UIOptionWindow __instance)
        {
            if (Main.DisableKeyTips.Value)
            {
                return false;
            }
            return true;
        }

        //他の惑星に到着したら再表示
        [HarmonyPostfix, HarmonyPatch(typeof(GameData), "ArrivePlanet")]
        public static void UIStarDetail_ArrivePlanet_Postfix()
        {
            MarkerPrefab.markerGroup.SetActive(true);
            MarkerList.listBase.SetActive(true);

            MarkerPool.Update();
            ArrowPool.Update();
            MarkerList.Refresh();
            MarkerPool.Refresh();

        }

        //惑星を去ったら
        [HarmonyPostfix, HarmonyPatch(typeof(GameData), "LeavePlanet")]
        public static void UIStarDetail_LeavePlanet_Postfix()
        {
            MarkerPrefab.markerGroup.SetActive(false);
            MarkerList.listBase.SetActive(false);
        }
    }
}
