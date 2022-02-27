using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.ComponentModel;
using System.IO;
using HarmonyLib;
using System.Reflection;
using xiaoye97;
using System.Security;
using System.Security.Permissions;


namespace DSPMarker
{
    public class MarkerList : ManualBehaviour
    {
        public static GameObject markerList = new GameObject();
        public static GameObject markerButton = new GameObject();
        public static GameObject listBase = new GameObject();
        public static GameObject boxBasePrefab = new GameObject();
        public static GameObject boxBaseSquare = new GameObject();
        public static GameObject boxBaseText = new GameObject();
        public static GameObject boxBaseIcon1 = new GameObject();
        public static GameObject boxBaseIcon2 = new GameObject();
        public static GameObject modeText = new GameObject();

        public static GameObject[] boxMarker;
        public static GameObject[] boxSquare;
        public static GameObject[] boxText;
        public static GameObject[] boxIcon1;
        public static GameObject[] boxIcon2;

        public static Sprite markerIcon;
        public static int boxSize = 40;

        public static bool showList = true;
        public static bool editMode;
        //public static Sprite purgeIcon;

        //UI解像度計算
        public static int UIheight = DSPGame.globalOption.uiLayoutHeight;
        public static int UIwidth = UIheight * Screen.width / Screen.height;


        public static void Create()
        {
            boxMarker = new GameObject[Main.maxMarker];
            boxSquare = new GameObject[Main.maxMarker];
            boxText = new GameObject[Main.maxMarker];
            boxIcon1 = new GameObject[Main.maxMarker];
            boxIcon2 = new GameObject[Main.maxMarker];

            //ボタン＆リスト用オブジェクトの作成
            markerList = new GameObject();
            markerList.name = "MarkerList";
            markerList.transform.SetParent(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows").transform);
            markerList.transform.localPosition = new Vector3(UIwidth / 2 - 60, UIheight / 2 - 70, 0);
            markerList.transform.localScale = new Vector3(1, 1, 1);

            //リストの表示切替用ボタンの作成
            markerButton = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Game Menu/detail-func-group/dfunc-1"), markerList.transform) as GameObject;
            markerButton.name = "markerButton";
            markerButton.transform.localPosition = new Vector3(0, 0, 0);
            markerButton.GetComponent<UIButton>().tips.tipTitle = "Marker List".Translate();
            markerButton.GetComponent<UIButton>().tips.tipText = "Click to show/hide Marker List.\nRight Click to enter/exit Edit mode.".Translate();
            markerButton.GetComponent<UIButton>().tips.corner = 4;
            markerButton.GetComponent<UIButton>().tips.offset = new Vector2(-50, 20);
            markerButton.GetComponent<UIButton>().tips.width = 215;
            markerButton.transform.Find("icon").GetComponent<Image>().sprite = Main.ButtonSprite;
            markerButton.GetComponent<UIButton>().highlighted = true;
            markerButton.AddComponent<UIClickHandler>();
            //ボタンイベントの作成
            markerButton.GetComponent<UIButton>().button.onClick.AddListener(new UnityAction(OnClickMarkerButton));
            //markerButton.GetComponent<UIButton>().onRightClick2 += OnRightClickMarkerButton;
            //markerButton.GetComponent<UIButton>().onRightClick.AddListener(() => OnRightClickMarkerButton());
            //markerButton.GetComponent<UIButton>().onRightClick.AddListener(new UnityAction(OnRightClickMarkerButton));



            //ボタン＆リスト用オブジェクトの作成
            listBase = new GameObject();
            listBase.name = "listBase";
            listBase.transform.SetParent(markerList.transform);
            listBase.transform.localPosition = new Vector3(0, 0, -1);
            listBase.transform.localScale = new Vector3(1, 1, 1);


            //リストprefabの作成

            boxBasePrefab.transform.SetParent(listBase.transform);
            boxBasePrefab.name = "boxBasePrefab";
            boxBasePrefab.AddComponent<Image>().color = new Color(0.7f, 0.5f, 0, 1);
            boxBasePrefab.AddComponent<Button>();
            boxBasePrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBasePrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBasePrefab.GetComponent<RectTransform>().sizeDelta = new Vector3(70, 70, 0);
            boxBasePrefab.transform.localPosition = new Vector3(5, -80, 0);
            boxBasePrefab.transform.localScale = new Vector3(1, 1, 1);
            boxBasePrefab.SetActive(false);

            boxBaseSquare.transform.SetParent(boxBasePrefab.transform);
            boxBaseSquare.name = "boxBaseSquare";
            boxBaseSquare.AddComponent<Image>().color = new Color(0.3f, 0.3f, 0, 1);
            boxBaseSquare.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBaseSquare.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBaseSquare.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
            boxBaseSquare.transform.localPosition = new Vector3(0, 0, 0);
            boxBaseSquare.transform.localScale = new Vector3(1, 1, 1);
            boxBaseSquare.SetActive(false);

            boxBaseText.transform.SetParent(boxBasePrefab.transform);
            boxBaseText.name = "boxBaseText";
            boxBaseText.AddComponent<Outline>().effectDistance = new Vector2(1, -1);
            boxBaseText.AddComponent<Text>().text = "New\nMarker".Translate();
            boxBaseText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            boxBaseText.GetComponent<Text>().lineSpacing = 0.7f;
            boxBaseText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
            boxBaseText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Truncate;
            boxBaseText.GetComponent<Text>().resizeTextForBestFit = true;
            boxBaseText.GetComponent<Text>().fontSize = 15;
            boxBaseText.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBaseText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBaseText.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
            //boxBaseText.GetComponent<RectTransform>().sizeDelta = new Vector3(60, 35, 0);
            //pinBaseText.GetComponent<Text>().resizeTextMaxSize = 35;
            //boxBaseText.transform.localPosition = new Vector3(0, -15, 0);
            boxBaseText.transform.localPosition = new Vector3(0, 0, 0);
            boxBaseText.transform.localScale = new Vector3(1, 1, 1);
            boxBaseText.SetActive(true);
            //pinBaseText.GetComponent<Text>().color = Color.magenta;
            //pinBaseText.GetComponent<Text>().fontSize = 50;

            modeText = Instantiate(boxBaseText.gameObject, markerList.transform);
            modeText.name = "modeText";
            modeText.transform.localPosition = new Vector3(-22, -21, 0);
            modeText.GetComponent<Text>().text = "Guide\nMode".Translate();
            modeText.GetComponent<RectTransform>().sizeDelta = new Vector3(50, 40, 0);
            modeText.SetActive(true);

            boxBaseIcon1.AddComponent<Image>().sprite = LDB.techs.Select(1001).iconSprite;
            boxBaseIcon1.name = "boxBaseIcon1";
            boxBaseIcon1.transform.SetParent(boxBasePrefab.transform);
            boxBaseIcon1.AddComponent<Outline>().effectDistance = new Vector2(1, -1);
            boxBaseIcon1.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            boxBaseIcon1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            boxBaseIcon1.GetComponent<RectTransform>().sizeDelta = new Vector3(30, 30, 0);
            boxBaseIcon1.transform.localPosition = new Vector3(-15, 15, 0);
            boxBaseIcon1.transform.localScale = new Vector3(1, 1, 1);
            boxBaseIcon1.SetActive(false);

            boxBaseIcon2 = Instantiate(boxBaseIcon1.gameObject);
            boxBaseIcon2.name = "boxBaseIcon2";
            boxBaseIcon2.transform.SetParent(boxBasePrefab.transform);
            boxBaseIcon2.transform.localPosition = new Vector3(15, 15, 0);
            boxBaseIcon2.transform.localScale = new Vector3(1, 1, 1);

            //リストの作成
            int maxRow = (UIheight - 270 - 115 - 40 ) / boxSize;
            float scale = (float)boxSize / 70;
            for (int i = 0; i < Main.maxMarker; i++)
            {
                boxMarker[i] = Instantiate(boxBasePrefab.gameObject, listBase.transform);
                boxMarker[i].name = "boxMarker" + i;
                boxMarker[i].transform.localScale = new Vector3(scale, scale, 1);
                int x = 20 - (boxSize + 3) * (i / maxRow);
                int y = -68 - (boxSize + 3) * (i % maxRow);
                boxMarker[i].transform.localPosition = new Vector3(x, y, 0);
                boxSquare[i] = boxMarker[i].transform.Find("boxBaseSquare").gameObject;
                boxText[i] = boxMarker[i].transform.Find("boxBaseText").gameObject;
                boxText[i].transform.localPosition = new Vector3(0, 0, 0);
                boxIcon1[i] = boxMarker[i].transform.Find("boxBaseIcon1").gameObject;
                boxIcon1[i].transform.localPosition = new Vector3(-15, 15, 0);
                boxIcon2[i] = boxMarker[i].transform.Find("boxBaseIcon2").gameObject;
                boxIcon2[i].transform.localPosition = new Vector3(15, 15, 0);
                var count = i;
                boxMarker[i].AddComponent<UIClickHandler>();
                boxMarker[i].GetComponent<Button>().onClick.AddListener(() => OnClickBoxMarker(count));
            }
            boxBasePrefab.SetActive(false);

            //for (int i = 0; i < Main.maxMarker; i++)
            //{
            //    ;
            //    boxMarker[0].GetComponent<Button>().onClick.AddListener(new UnityAction(onClickboxMarker0));
            //    //this.alarmSwitchButton.onClick += this.OnAlarmSwitchButtonClick;
            //    //this.nameInput.onEndEdit.AddListener(new UnityAction<string>(this.OnNameInputSubmit));
            //}
        }

        //イベント
        //リスト表示の切り替え
        public static void OnClickMarkerButton()
        {
            showList = !showList;
            listBase.gameObject.SetActive(showList);
            markerButton.GetComponent<UIButton>().highlighted = showList;

        }



        public static void Crear()
        {
            for (int i = 0; i < Main.maxMarker; i++)
            {
                boxMarker[i].gameObject.SetActive(false);
            }
        }


        //右クリックで新規

        //リストをクリックしたら
        public static void OnClickBoxMarker(int i)
        {
            //boxText[i].GetComponent<Text>().text = "TESTGO"+i;
            //boxIcon1[i].GetComponent<Image>().sprite = LDB.techs.Select(1001).iconSprite;
            //boxIcon2[i].GetComponent<Image>().sprite = LDB.techs.Select(1002).iconSprite;
            //LogManager.Logger.LogInfo("---------------------------------------------------------i : " + i);
            if (editMode)
            {
                //int i = Int32.Parse(obj.name.Replace("boxMarker", ""));
                //boxText[i].GetComponent<Text>().text = "RIGHT" + i;
                //LogManager.Logger.LogInfo("---------------------------------------------------------i : " + i);
                if (!MarkerEditor.window.activeSelf)
                {
                    MarkerEditor.Open(i);
                }
                else
                {
                    MarkerEditor.Close();
                }
            }else if (!GameMain.data.mainPlayer.sailing)
            {
                //GameMain.mainPlayer.gizmo.orderGizmos.Clear();
                Array.Clear(GameMain.mainPlayer.orders.orderQueue, 0, GameMain.mainPlayer.orders.orderQueue.Length);
                //LineGizmo.pool.Clear();
                //CircleGizmo.pool.Clear();

                int planetId = GameMain.data.localPlanet.id;
                var num = MarkerPool.markerIdInPlanet[planetId][i];
                //オーダー設定
                OrderNode order = new OrderNode();
                order.type = EOrderType.Move;
                order.target = MarkerPool.markerPool[num].pos;
                order.objType = EObjectType.Entity;
                order.objId = 0;
                order.objPos = MarkerPool.markerPool[num].pos;
                GameMain.mainPlayer.orders.currentOrder = order;
 
                CircleGizmo circleGizmo = CircleGizmo.Create(0, order.target, 0.27f);
                circleGizmo.relateObject = order;
                circleGizmo.color = MarkerPool.markerPool[num].color;
                circleGizmo.fadeInScale = 1.8f;
                circleGizmo.fadeInTime = 0.15f;
                circleGizmo.fadeOutScale = 1.8f;
                circleGizmo.fadeOutTime = 0.15f;
                circleGizmo.alphaMultiplier = 0.5f;
                circleGizmo.multiplier = 3f;
                circleGizmo.Open();
                GameMain.mainPlayer.gizmo.orderGizmos.Add(circleGizmo);

                LineGizmo lineGizmo = LineGizmo.Create(0, order.target, GameMain.mainPlayer.position);
                lineGizmo.color = MarkerPool.markerPool[num].color;
                lineGizmo.relateObject = order;
                lineGizmo.autoRefresh = true;
                lineGizmo.width = 3f;
                lineGizmo.alphaMultiplier = 0.5f;
                lineGizmo.multiplier = 3f;
                lineGizmo.spherical = true;
                lineGizmo.Open();
                GameMain.mainPlayer.gizmo.orderGizmos.Add(lineGizmo);

            }


        }

        //全ての右クリック
        public static void OnRightClick(GameObject obj)
        {
            LogManager.Logger.LogInfo("--------------------------------------------------------obj.name : " + obj.name);
            if (obj.name == "markerButton")
            {
                if (!GameMain.data.mainPlayer.sailing)
                {
                    if (editMode)
                    {
                        modeText.GetComponent<Text>().text = "Guide\nMode".Translate();
                        MarkerEditor.Close();
                        editMode = false;
                    }
                    else
                    {
                        modeText.GetComponent<Text>().text = "Edit\nMode".Translate();
                        editMode = true;
                    }
                    Refresh();
                    //LogManager.Logger.LogInfo("---------------------------------------------------------markerButton RIGHT cLICK ");
                }
            }
            else
            {
                //if (editMode)
                //{
                //    int i = Int32.Parse(obj.name.Replace("boxMarker", ""));
                //    //boxText[i].GetComponent<Text>().text = "RIGHT" + i;
                //    //LogManager.Logger.LogInfo("---------------------------------------------------------i : " + i);
                //    if (!MarkerEditor.window.activeSelf)
                //    {
                //        MarkerEditor.Open(i);
                //    }
                //    else
                //    {
                //        MarkerEditor.Close();
                //    }
                //}

            }
        }
        public static void Refresh()
        {
            //LogManager.Logger.LogInfo("---------------------------------------------------------refresh");
            //LogManager.Logger.LogInfo("---------------------------------------------------------MarkerPool.markerPool.Count : " + MarkerPool.markerPool.Count);

            int planetId = GameMain.localPlanet.id;

            if (!MarkerPool.markerIdInPlanet.ContainsKey(planetId))
            {
                List<int> list = new List<int>();
                MarkerPool.markerIdInPlanet.Add(planetId, list);
            }

            for (int i = 0; i < Main.maxMarker; i++)
            {
                if (i < MarkerPool.markerIdInPlanet[planetId].Count)
                {
                    var num = MarkerPool.markerIdInPlanet[planetId][i];
                    var marker = MarkerPool.markerPool[num];

                    boxMarker[i].GetComponent<Image>().color = marker.color;
                    boxMarker[i].SetActive(true);
                    boxSquare[i].GetComponent<Image>().color = new Color(marker.color.r * 0.3f, marker.color.g * 0.3f, marker.color.b * 0.3f, 1);
                    boxSquare[i].SetActive(true);
                    boxText[i].GetComponent<Text>().text = marker.desc;
                    //boxText[i].transform.localPosition = new Vector3(0, -15, 0);
                    //boxText[i].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 35, 0);
                    if (marker.icon1ID == 0 && marker.icon2ID == 0)
                    {
                        boxText[i].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                        boxText[i].transform.localPosition = new Vector3(0, 0, 0);
                    }else
                    {
                        boxText[i].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 35, 0);
                        boxText[i].transform.localPosition = new Vector3(0, -15, 0);
                    }
                    boxText[i].SetActive(true);

                    if (marker.icon1ID == 0)
                    {
                        boxIcon1[i].SetActive(false);
                    }
                    else
                    {
                        boxIcon1[i].GetComponent<Image>().sprite = LDB.signals.IconSprite(marker.icon1ID);
                        boxIcon1[i].SetActive(true);
                    }
                    if (marker.icon2ID == 0)
                    {
                        boxIcon2[i].SetActive(false);
                    }
                    else
                    {
                        boxIcon2[i].GetComponent<Image>().sprite = LDB.signals.IconSprite(marker.icon2ID);
                        boxIcon2[i].SetActive(true);
                    }
                    if (marker.icon2ID == 0 && marker.desc == "")
                    {
                        boxIcon1[i].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                        boxIcon1[i].transform.localPosition = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        boxIcon1[i].GetComponent<RectTransform>().sizeDelta = new Vector3(30, 30, 0);
                        boxIcon1[i].transform.localPosition = new Vector3(-15, 15, 0);
                    }


                }
                else
                {
                    boxMarker[i].SetActive(false);
                }
            }
            if (editMode)
            {
                if(MarkerPool.markerIdInPlanet[planetId].Count < Main.maxMarker)
                {
                    int count = MarkerPool.markerIdInPlanet[planetId].Count;
                    boxMarker[count].GetComponent<Image>().color = new Color(0.7f, 0.5f, 0, 1);
                    boxSquare[count].SetActive(false);
                    boxIcon1[count].SetActive(false);
                    boxIcon2[count].SetActive(false);
                    boxText[count].transform.localPosition = new Vector3(0, 0, 0);
                    boxText[count].GetComponent<Text>().text = "New\nMarker".Translate();
                    boxText[count].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);

                    boxMarker[count].SetActive(true);
                }
            }
        }

    }
    }
