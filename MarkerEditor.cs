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
    internal class MarkerEditor : MonoBehaviour
    {


        //マーカーエディタウインドウ作成
        public static GameObject window = new GameObject();
        public static GameObject markerPrefab = new GameObject();
        public static GameObject textBox = new GameObject();
        public static GameObject descBox = new GameObject();
        public static GameObject iconBox1 = new GameObject();
        public static GameObject iconBox2 = new GameObject();
        public static GameObject colorSelecter = new GameObject();
        public static GameObject colorTitle = new GameObject();
        public static GameObject[] colorBox = new GameObject[7];
        public static GameObject iconTitle = new GameObject();
        public static GameObject descTitle = new GameObject();
        public static GameObject checkTitle1 = new GameObject();
        public static GameObject checkTitle2 = new GameObject();
        public static GameObject checkTitle3 = new GameObject();
        public static GameObject checkBox1 = new GameObject();
        public static GameObject checkBox2 = new GameObject();
        public static GameObject checkBox3 = new GameObject();
        public static GameObject closeButton = new GameObject();
        public static GameObject applyButton = new GameObject();
        public static GameObject previewText = new GameObject();

        public static Color[] color = new Color[7];

        public static int id;
        public static Vector3 pos;
        public static int iconID1;
        public static int iconID2;
        public static Color baseColor;
        public static string desc;
        public static bool alwaysDisplay;
        public static bool throughPlanet;
        public static bool ShowArrow;

        public static Sprite emptySprite;


        //フラグ
        public static void Open(int num)
        {
            var pos1 = MarkerList.boxMarker[num].transform.localPosition;
            window.transform.localPosition = new Vector3(pos1.x + 637, pos1.y + 530, 0);
            int serchNum = GameMain.localPlanet.id * 100 + num;
            Sprite sprite1;
            Sprite sprite2;
            id = num;

            if (MarkerPool.markerPool.ContainsKey(serchNum))
            {
                MarkerPool.Marker marker = MarkerPool.markerPool[serchNum];
                pos = marker.pos;
                iconID1 = marker.icon1ID;
                iconID2 = marker.icon2ID;
                sprite1 = LDB.signals.IconSprite(marker.icon1ID);
                sprite2 = LDB.signals.IconSprite(marker.icon2ID);
                baseColor = marker.color;
                desc = marker.desc;
                alwaysDisplay = marker.alwaysDisplay;
                throughPlanet = marker.throughPlanet;
                ShowArrow = marker.ShowArrow;
            }
            else
            {
                iconID1 = 0;
                iconID2 = 0;
                sprite1 = emptySprite;
                sprite2 = emptySprite;
                baseColor = color[0];
                desc = "enter\ndesc";
                alwaysDisplay = true;
                throughPlanet = true;
                ShowArrow = true;
            }
            MarkerEditor.iconBox1.GetComponent<Image>().sprite = sprite1;
            MarkerEditor.markerPrefab.transform.Find("round/pinBaseIcon1").gameObject.GetComponent<Image>().sprite = sprite1;
            MarkerEditor.iconBox2.GetComponent<Image>().sprite = sprite2;
            MarkerEditor.markerPrefab.transform.Find("round/pinBaseIcon2").gameObject.GetComponent<Image>().sprite = sprite2;
            var halfColor = new Color(baseColor.r * 0.3f, baseColor.g * 0.3f, baseColor.b * 0.3f, 1f);
            MarkerEditor.markerPrefab.GetComponent<Image>().color = baseColor;
            MarkerEditor.markerPrefab.transform.Find("round").gameObject.GetComponent<Image>().color = halfColor;
            MarkerEditor.markerPrefab.transform.Find("round/pinBaseText").GetComponent<Text>().text = desc;
            descBox.gameObject.GetComponent<InputField>().text = desc;
            MarkerEditor.checkBox1.transform.Find("checked").gameObject.GetComponent<Image>().enabled = alwaysDisplay;
            MarkerEditor.checkBox2.transform.Find("checked").gameObject.GetComponent<Image>().enabled = throughPlanet;
            MarkerEditor.checkBox3.transform.Find("checked").gameObject.GetComponent<Image>().enabled = ShowArrow;

            window.SetActive(true);

        }


        //18 -77
        //655 448
        public static void Close()
        {
            window.SetActive(false);
        }

        //public static void update()
        //{
        //    if()



        //}



        public static void Create()
        {
            color[0] = new Color(1.00f, 0.73f, 0.78f);
            color[1] = new Color(0.90f, 0.00f, 0.50f);
            color[2] = new Color(0.92f, 0.34f, 0.06f);
            color[3] = new Color(1.00f, 0.95f, 0.00f);
            color[4] = new Color(0.16f, 0.68f, 0.23f);
            color[5] = new Color(0.00f, 0.63f, 0.92f);
            color[6] = new Color(0.39f, 0.12f, 0.53f);


            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 1");

            //ウインドウ作成
            //GameObject overlayCanvas = GameObject.Find("UI Root/Overlay Canvas");

            window = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Replicator Window").gameObject, GameObject.Find("UI Root/Overlay Canvas/In Game/Windows").transform);
            window.name = "Marker Editer";
            Destroy(window.transform.Find("panel-bg/title-text").gameObject.GetComponent<Localizer>());
            window.transform.Find("panel-bg/title-text").GetComponent<Text>().text = "Marker Editor".Translate();
            //Destroy(window.transform.Find("title").GetComponent<UIMechaWindow>());
            window.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

            Destroy(window.GetComponent<UIReplicatorWindow>());
            Destroy(window.transform.Find("tree-tweener-1").gameObject);
            Destroy(window.transform.Find("tree-tweener-2").gameObject);
            Destroy(window.transform.Find("queue-group").gameObject);
            Destroy(window.transform.Find("recipe-group").gameObject);
            Destroy(window.transform.Find("recipe-tree").gameObject);

            window.SetActive(false);


            //マーカー例
            markerPrefab = Instantiate(MarkerPrefab.pinBasePrefab.gameObject, window.transform);
            markerPrefab.name = "markerPrefab";
            markerPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            markerPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            markerPrefab.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            markerPrefab.transform.localPosition = new Vector3(-225, -95, 0);
            markerPrefab.transform.localScale = new Vector3(1, 1, 0);
            markerPrefab.transform.Find("round/pinBaseIcon1").gameObject.GetComponent<Image>().sprite = null;
            markerPrefab.transform.Find("round/pinBaseIcon1").gameObject.transform.localPosition = new Vector3(-30, 30, 0);
            markerPrefab.transform.Find("round/pinBaseIcon2").gameObject.GetComponent<Image>().sprite = null;
            markerPrefab.transform.Find("round/pinBaseIcon2").gameObject.transform.localPosition = new Vector3(30, 30, 0);


            markerPrefab.SetActive(true);

            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 2");

            //色選択用オブジェクトタイトル
            colorTitle = Instantiate(window.transform.Find("panel-bg/title-text").gameObject, window.transform);
            colorTitle.name = "colorTitle";
            colorTitle.transform.localPosition = new Vector3(-45, -70, 0);
            colorTitle.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            colorTitle.GetComponent<Text>().text = "Color".Translate();
            colorTitle.GetComponent<Text>().fontSize = 14;
            colorTitle.GetComponent<RectTransform>().sizeDelta = new Vector3(100, 36, 0);

            //色選択用オブジェクト枠
            colorSelecter = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Mecha Window/appearance/card-mask").gameObject, window.transform);
            colorSelecter.name = "colorSelecter"; ;
            colorSelecter.transform.localPosition = new Vector3(135, -103, 0);
            colorSelecter.SetActive(true);
            int count = colorSelecter.transform.childCount;
            foreach (Transform n in colorSelecter.transform)
            {
                if (count == 1)
                {
                    n.gameObject.SetActive(false);
                    break;
                }

                GameObject.Destroy(n.gameObject);
                count--;
            }
            for (int i = 0; i < 7; i++)
            {
                colorBox[i] = Instantiate(colorSelecter.transform.Find("color").gameObject, colorSelecter.transform);
                colorBox[i].name = "color" + i;
                colorBox[i].transform.localPosition = new Vector3(-55 + 20 * i, 13.2f, 0);
                colorBox[i].GetComponent<Image>().enabled = true;
                colorBox[i].GetComponent<Image>().color = color[i];
                colorBox[i].GetComponent<Shadow>().enabled = true;
                colorBox[i].GetComponent<Button>().enabled = true;
                colorBox[i].SetActive(true);
                var No = i;
                colorBox[i].GetComponent<Button>().onClick.AddListener(() => onClickColor(No));
            }

            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 3");

            //アイコン選択用オブジェクトタイトル
            iconTitle = Instantiate(colorTitle, window.transform);
            iconTitle.name = "iconTitle";
            iconTitle.transform.localPosition = new Vector3(-45, -125, 0);
            iconTitle.GetComponent<Text>().text = "Icons".Translate();


            //アイコン選択オブジェクト
            iconBox1 = Instantiate(UIRoot.instance.uiGame.blueprintBrowser.inspector.thumbIconImage1.gameObject, window.transform);

            //iconBox1 = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Blueprint Browser/inspector-group/group-1/thumbnail-image/icon-1").gameObject, window.transform);
            iconBox1.name = "iconBox1";
            iconBox1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            iconBox1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            iconBox1.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            iconBox1.GetComponent<RectTransform>().sizeDelta = new Vector3(50, 50, 0);
            iconBox1.AddComponent<Button>();
            iconBox1.transform.localPosition = new Vector3(65, -120, 0);
            iconBox1.SetActive(true);

            emptySprite = iconBox1.GetComponent<Image>().sprite;

            iconBox2 = Instantiate(iconBox1.gameObject, window.transform);
            iconBox2.name = "iconBox2";
            iconBox2.transform.localPosition = new Vector3(115, -120, 0);

            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 4");

            //文字入力用オブジェクトタイトル
            descTitle = Instantiate(colorTitle, window.transform);
            descTitle.name = "descTitle";
            descTitle.transform.localPosition = new Vector3(-45, -185, 0);
            descTitle.GetComponent<Text>().text = "Description".Translate();
            
            //preview
            previewText = Instantiate(colorTitle, window.transform);
            previewText.name = "previewText";
            previewText.transform.localPosition = new Vector3(-200, -60, 0);
            previewText.GetComponent<Text>().text = "Preview".Translate();

            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 5");

            //文字入力用オブジェクト
            descBox = Instantiate(UIRoot.instance.uiGame.blueprintBrowser.inspector.descTextInput.gameObject, window.transform);

            //descBox = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Blueprint Browser/inspector-group/group-1/input-desc-text").gameObject, window.transform);
            descBox.name = "descBox";
            descBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
            descBox.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.5f, 0.5f, 0);
            descBox.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            descBox.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            descBox.GetComponent<RectTransform>().offsetMax = new Vector2(65, 35f);
            descBox.GetComponent<RectTransform>().offsetMin = new Vector2(-65f, -35f);
            descBox.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            descBox.GetComponent<RectTransform>().sizeDelta = new Vector3(130, 70, 0);
            descBox.transform.localPosition = new Vector3(115, -205, 0);
            descBox.transform.localScale = new Vector3(0.7f, 0.7f, 0);

            //descBox.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().text = "!TEST!";
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().lineSpacing = 0.6f;
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Truncate;
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().resizeTextForBestFit = true;
            descBox.transform.Find("value-text").gameObject.GetComponent<Text>().fontSize = 30;
            descBox.SetActive(true);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 6");

            //チェックボックス１タイトル
            checkTitle1 = Instantiate(colorTitle, window.transform);
            checkTitle1.name = "checkTitle1";
            checkTitle1.transform.localPosition = new Vector3(-240, -250, 0);
            checkTitle1.GetComponent<Text>().text = "Always displayed".Translate();
            checkTitle1.GetComponent<RectTransform>().sizeDelta = new Vector3(300, 32, 0);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 7");

            //チェックボックス１
            checkBox1 = Instantiate(UIRoot.instance.uiGame.buildMenu.uxFacilityCheck.gameObject, window.transform);
            //checkBox1 = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Function Panel/Build Menu/ux-group/checkbox-facilities").gameObject, window.transform);
            checkBox1.name = "checkBox1";
            checkBox1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            checkBox1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            checkBox1.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            checkBox1.AddComponent<Button>();
            checkBox1.transform.Find("checked").gameObject.GetComponent<Image>().enabled = true;
            checkBox1.transform.localPosition = new Vector3(65, -250, 0);
            Destroy(checkBox1.transform.Find("text").gameObject);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 8");

            //チェックボックス２タイトル
            checkTitle2 = Instantiate(checkTitle1, window.transform);
            checkTitle2.name = "checkTitle2";
            checkTitle2.transform.localPosition = new Vector3(-240, -285, 0);
            checkTitle2.GetComponent<Text>().text = "Seen through the planet".Translate();

            //チェックボックス２
            checkBox2 = Instantiate(checkBox1.gameObject, window.transform);
            checkBox2.name = "checkBox2";
            checkBox2.transform.localPosition = new Vector3(65, -285, 0);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 9");

            //チェックボックス３タイトル
            checkTitle3 = Instantiate(checkTitle1, window.transform);
            checkTitle3.name = "checkTitle3";
            checkTitle3.transform.localPosition = new Vector3(-240, -320, 0);
            checkTitle3.GetComponent<Text>().text = "Show the Arrow Guide".Translate();

            //チェックボックス３
            checkBox3 = Instantiate(checkBox1.gameObject, window.transform);
            checkBox3.name = "checkBox3";
            checkBox3.transform.localPosition = new Vector3(65, -320, 0);
            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 10");

            //「適応」ボタン
            applyButton = Instantiate(UIRoot.instance.uiGame.stationWindow.storageUIPrefab.localSdButton.gameObject, window.transform) as GameObject;
            applyButton.transform.localPosition = new Vector3(200, -400, 0);
            applyButton.name = "applyButton";
            applyButton.GetComponentInChildren<Text>().text = "Apply".Translate();
            applyButton.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
            applyButton.GetComponent<Image>().color = new Color(0.240f, 0.55f, 0.65f, 0.7f);
            applyButton.SetActive(true);



            //閉じるボタン
            closeButton = window.transform.Find("panel-bg/btn-box/close-btn").gameObject;

            //イベント作成
            //LogManager.Logger.LogInfo("---------------------------------------------------------make EditorWindow 12");
            iconBox1.GetComponent<Button>().onClick.AddListener(onClickIconBox1);
            iconBox2.GetComponent<Button>().onClick.AddListener(onClickIconBox2);

            checkBox1.GetComponent<Button>().onClick.AddListener(onClickCheckBox1);
            checkBox2.GetComponent<Button>().onClick.AddListener(onClickCheckBox2);
            checkBox3.GetComponent<Button>().onClick.AddListener(onClickCheckBox3);

            descBox.GetComponent<InputField>().onEndEdit.AddListener(new UnityAction<string>(onEndEditDescBox));
            closeButton.GetComponent<Button>().onClick.AddListener(onClickCloseButton);

            applyButton.GetComponent<Button>().onClick.AddListener(onClickApplyButton);




        }


        //イベント
        public static void onClickApplyButton()
        {

            var realRadius = GameMain.localPlanet.realRadius;
 
            MarkerPool.Marker marker = new MarkerPool.Marker();
            //marker.planetID = GameMain.localPlanet.id;
            marker.icon1ID = iconID1;
            marker.icon2ID = iconID2;
            marker.color = baseColor;
            marker.desc = desc;
            marker.alwaysDisplay = alwaysDisplay;
            marker.throughPlanet = throughPlanet;
            marker.ShowArrow = ShowArrow;
            var num = GameMain.localPlanet.id * 100 + id;
            
            if(MarkerPool.markerPool.ContainsKey(num))
            {
                marker.pos = pos;
                MarkerPool.markerPool[num] = marker;
            }
            else
            {
                marker.pos = GameMain.mainPlayer.position;
                MarkerPool.markerPool.Add(num, marker);
            }
            LogManager.Logger.LogInfo("------------------------------------------------------------------num : " + num);

            MarkerList.Refresh();
            MarkerPool.Refresh();



            Close();
        }

        public static void onClickCloseButton()
        {
            Close();
        }

        public static void onClickColor(int i)
        {
            baseColor = MarkerEditor.colorBox[i].GetComponent<Image>().color;
            var halfColor = new Color(baseColor.r * 0.3f, baseColor.g * 0.3f, baseColor.b * 0.3f, 1f);
            MarkerEditor.markerPrefab.GetComponent<Image>().color = baseColor;
            MarkerEditor.markerPrefab.transform.Find("round").gameObject.GetComponent<Image>().color = halfColor;
        }



        public static void onClickIconBox1()
        {
            LogManager.Logger.LogInfo("---------------------------------------------------------onClickIconBox1");

            if (UISignalPicker.isOpened)
            {
                UISignalPicker.Close();
                return;
            }
            UISignalPicker.Popup(new Vector2(50f, 350f), new Action<int>(onIconBox1Changed));
        }
        public static void onClickIconBox2()
        {
            if (UISignalPicker.isOpened)
            {
                UISignalPicker.Close();
                return;
            }
            UISignalPicker.Popup(new Vector2(50f, 350f), new Action<int>(onIconBox2Changed));
        }

        public static void onIconBox1Changed(int signalId)
        {
            Sprite sprite = LDB.signals.IconSprite(signalId);
            if (sprite != null)
            {
                MarkerEditor.iconBox1.GetComponent<Image>().sprite = sprite;
                MarkerEditor.markerPrefab.transform.Find("round/pinBaseIcon1").gameObject.GetComponent<Image>().sprite = sprite;
                iconID1 = signalId;
            }
        }
        public static void onIconBox2Changed(int signalId)
        {
            Sprite sprite = LDB.signals.IconSprite(signalId);
            if (sprite != null)
            {
                MarkerEditor.iconBox2.GetComponent<Image>().sprite = sprite;
                MarkerEditor.markerPrefab.transform.Find("round/pinBaseIcon2").gameObject.GetComponent<Image>().sprite = sprite;
                iconID2 = signalId;
            }
        }
        public static void onClickCheckBox1()
        {
            alwaysDisplay = !alwaysDisplay;
            MarkerEditor.checkBox1.transform.Find("checked").gameObject.GetComponent<Image>().enabled = alwaysDisplay;
        }

        public static void onClickCheckBox2()
        {
            throughPlanet = !throughPlanet;
            MarkerEditor.checkBox2.transform.Find("checked").gameObject.GetComponent<Image>().enabled = throughPlanet;
        }
        public static void onClickCheckBox3()
        {
            ShowArrow = !ShowArrow;
            MarkerEditor.checkBox3.transform.Find("checked").gameObject.GetComponent<Image>().enabled = ShowArrow;
        }

        public static void onEndEditDescBox(string str)
        {
            MarkerEditor.markerPrefab.transform.Find("round/pinBaseText").GetComponent<Text>().text = str;
            desc = str;
        }

    }
}
