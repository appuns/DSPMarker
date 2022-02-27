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

namespace DSPMarker
{
    internal class MarkerPrefab : MonoBehaviour
    {
        //マーカーprefab作成
        public static GameObject markerGroup = new GameObject();
        public static GameObject pinBasePrefab = new GameObject();
        public static GameObject pinBaseRound = new GameObject();
        public static GameObject pinBaseText = new GameObject();
        public static GameObject pinBaseIcon1 = new GameObject();
        public static GameObject pinBaseIcon2 = new GameObject();

        public static void Create()

        {
            markerGroup = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Scene UIs/Vein Marks"), GameObject.Find("UI Root/Overlay Canvas/In Game/Scene UIs").transform);
            markerGroup.name = "Marker Group";
            Destroy(markerGroup.GetComponent<UIVeinDetail>());
            markerGroup.SetActive(true);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make marker 1");

            pinBasePrefab.transform.SetParent(markerGroup.transform);
            pinBasePrefab.name = "pinBasePrefab";
            pinBasePrefab.AddComponent<Image>().sprite = Main.merkerSprite;
            pinBasePrefab.GetComponent<Image>().color = new Color(0, 1, 1, 1);
            pinBasePrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            pinBasePrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            pinBasePrefab.GetComponent<RectTransform>().sizeDelta = new Vector3(200, 200, 0);
            pinBasePrefab.transform.localPosition = new Vector3(200, 900, 0);
            pinBasePrefab.transform.localScale = new Vector3(1, 1, 1);
            pinBasePrefab.SetActive(false);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make marker 2");

            pinBaseRound.transform.SetParent(pinBasePrefab.transform);
            pinBaseRound.name = "round";
            pinBaseRound.AddComponent<Image>().sprite = Main.roundSprite;
            pinBaseRound.GetComponent<Image>().color = new Color(0, 0.4f, 0.4f, 1);
            pinBaseRound.GetComponent<RectTransform>().sizeDelta = new Vector3(155, 155, 0);
            pinBaseRound.transform.localPosition = new Vector3(0, 18, 0);
            pinBaseRound.transform.localScale = new Vector3(1, 1, 1);
            pinBaseRound.SetActive(true);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make marker 3");

            pinBaseText = Instantiate(markerGroup.transform.Find("vein-tip-prefab/info-text").gameObject, pinBaseRound.transform);
            pinBaseText.transform.SetParent(pinBaseRound.transform);
            pinBaseText.name = "pinBaseText";
            pinBaseText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
            pinBaseText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.5f, 0.5f, 0);
            pinBaseText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            pinBaseText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            pinBaseText.GetComponent<RectTransform>().offsetMax = new Vector2(65, 35f);
            pinBaseText.GetComponent<RectTransform>().offsetMin = new Vector2(-65f, -35f);
            pinBaseText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            pinBaseText.GetComponent<RectTransform>().sizeDelta = new Vector3(130, 70, 0);
            pinBaseText.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
            pinBaseText.GetComponent<Text>().text = "!TEST!";
            pinBaseText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            pinBaseText.GetComponent<Text>().lineSpacing = 0.6f;
            pinBaseText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
            pinBaseText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Truncate;
            pinBaseText.GetComponent<Text>().resizeTextForBestFit = true;
            pinBaseText.GetComponent<Text>().resizeTextMaxSize = 60;
            pinBaseText.GetComponent<Text>().fontSize = 30;
            //pinBaseText.GetComponent<Text>().resizeTextMaxSize = 35;
            pinBaseText.transform.localPosition = new Vector3(0, -28, 0);
            Destroy(pinBaseText.GetComponent<Shadow>());
            pinBaseText.SetActive(true);
            //pinBaseText.GetComponent<Text>().color = Color.magenta;
            //pinBaseText.GetComponent<Text>().fontSize = 50;
           // LogManager.Logger.LogInfo("---------------------------------------------------------make marker 4");

            pinBaseIcon1.AddComponent<Image>().sprite = LDB.techs.Select(1001).iconSprite;
            pinBaseIcon1.name = "pinBaseIcon1";
            pinBaseIcon1.transform.SetParent(pinBaseRound.transform);
            pinBaseIcon1.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
            pinBaseIcon1.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
            pinBaseIcon1.transform.localPosition = new Vector3(-30, 30, 0);
            pinBaseIcon1.transform.localScale = new Vector3(1, 1, 1);
            pinBaseIcon1.SetActive(true);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make marker 5");

            pinBaseIcon2.AddComponent<Image>().sprite = LDB.recipes.Select(2).iconSprite;
            pinBaseIcon2.name = "pinBaseIcon2";
            pinBaseIcon2.transform.SetParent(pinBaseRound.transform);
            pinBaseIcon2.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
            pinBaseIcon2.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
            pinBaseIcon2.transform.localPosition = new Vector3(30, 30, 0);
            pinBaseIcon2.transform.localScale = new Vector3(1, 1, 1);
            pinBaseIcon2.SetActive(true);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make marker 6");

            Destroy(markerGroup.transform.Find("vein-tip-prefab").gameObject);
            Destroy(markerGroup.transform.Find("vein-tip-cursor").gameObject);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make marker end");


        }

    }
}
