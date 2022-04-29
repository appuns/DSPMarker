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
    public class MarkerPool : MonoBehaviour
    {
        public static Dictionary<int, List<int>> markerIdInPlanet = new Dictionary<int, List<int>>();
        public static Dictionary<int, Marker> markerPool = new Dictionary<int, Marker>();

        public static int markerCursor = 0;
        //public static Dictionary<int, Marker> markerPoolLocal = new Dictionary<int, Marker>();

        //public static int Main.maxMarker = 20;
        public static bool markerEnable = true;

        public static GameObject[] markers;
        public static Image[] Base;
        public static Image[] BaseRound;
        public static Image[] icon1s;
        public static Image[] icon2s;
        public static Text[] decs;

        public static bool markerCreated = false;


        public struct Marker
        {
            public int planetID;
            public Vector3 pos;
            public int icon1ID;
            public int icon2ID;
            public Color color;
            public string desc;
            public bool alwaysDisplay;
            public bool throughPlanet;
            public bool ShowArrow;
        }

        //public static int CountInLocalPlanet()
        //{
        //    var count = 0;
        //    for (int i = 0; i < markerPool.Count; i++)
        //    {
        //        if (markerPool[i].planetID == GameMain.localPlanet.id)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}


        public static void Create()
        {

            markers = new GameObject[Main.maxMarker];
            Base = new Image[Main.maxMarker];
            BaseRound = new Image[Main.maxMarker];
            icon1s = new Image[Main.maxMarker];
            icon2s = new Image[Main.maxMarker];
            decs = new Text[Main.maxMarker];

            for (int i = 0; i < Main.maxMarker; i++)
            {
                markers[i] = Instantiate(MarkerPrefab.pinBasePrefab.gameObject, MarkerPrefab.markerGroup.transform);
                Base[i] = markers[i].GetComponent<Image>();
                BaseRound[i] = markers[i].transform.Find("round").GetComponent<Image>();
                icon1s[i] = markers[i].transform.Find("round/pinBaseIcon1").GetComponent<Image>();
                icon2s[i] = markers[i].transform.Find("round/pinBaseIcon2").GetComponent<Image>();
                decs[i] = markers[i].transform.Find("round/pinBaseText").GetComponent<Text>();
            }

            markerCreated = true;

        }



        public static void Refresh()
        {
            int planetId = GameMain.localPlanet.id;

            if (!MarkerPool.markerIdInPlanet.ContainsKey(planetId))
            {
                List<int> list = new List<int>();
                MarkerPool.markerIdInPlanet.Add(planetId, list);
            }

            if (markerIdInPlanet[planetId].Count > 0 && markerCreated)
            {
                for (int i = 0; i < markerIdInPlanet[planetId].Count; i++)
                {
                    var num = markerIdInPlanet[planetId][i];
                    if (markerPool.ContainsKey(num))
                    {
                        Base[i].color = markerPool[num].color;
                        var halfColor = new Color(markerPool[num].color.r * 0.3f, markerPool[num].color.g * 0.3f, markerPool[num].color.b * 0.3f, 1f);
                        BaseRound[i].color = halfColor;
                        if (markerPool[num].icon1ID == 0)
                        {
                            icon1s[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            icon1s[i].gameObject.SetActive(true);
                            icon1s[i].sprite = LDB.signals.IconSprite(markerPool[num].icon1ID);
                        }
                        if (markerPool[num].icon2ID == 0)
                        {
                            icon2s[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            icon2s[i].gameObject.SetActive(true);
                            icon2s[i].sprite = LDB.signals.IconSprite(markerPool[num].icon2ID);
                        }
                        decs[i].text = markerPool[num].desc;
                        if (markerPool[num].icon1ID == 0 && markerPool[num].icon2ID == 0)
                        {
                            decs[i].GetComponent<RectTransform>().sizeDelta = new Vector3(130, 130, 0);
                            decs[i].transform.localPosition = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            decs[i].GetComponent<RectTransform>().sizeDelta = new Vector3(130, 70, 0);
                            decs[i].transform.localPosition = new Vector3(0, -28, 0);
                        }
                        if (markerPool[num].icon2ID == 0 && markerPool[num].desc == "")
                        {
                            icon1s[i].GetComponent<RectTransform>().sizeDelta = new Vector3(130, 130, 0);
                            icon1s[i].transform.localPosition = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            icon1s[i].GetComponent<RectTransform>().sizeDelta = new Vector3(60, 60, 0);
                            icon1s[i].transform.localPosition = new Vector3(-30, 30, 0);
                        }



                    }
                }
            }

        }

        public static void Crear()
        {
            for (int i = 0; i < Main.maxMarker; i++)
            {
                markers[i].gameObject.SetActive(false);
            }
        }

        public static void Update()
        {
            //LogManager.Logger.LogInfo("------------------------------------------------------------------markerPool Update ");
            if (!MarkerList.showList)
            {
                MarkerPrefab.markerGroup.SetActive(false);
                return;
            }

            //if (!markerEnable)
            //{
            //    return;
            //}
            int planetId = GameMain.data.localPlanet.id;
            MarkerPrefab.markerGroup.SetActive(true);

            //Vector3 localPosition = GameCamera.main.transform.localPosition;
            //Vector3 forward = GameCamera.main.transform.forward;
            //float realRadius = localPlanet.realRadius;    //惑星の半径

            for (int i = 0; i < Main.maxMarker; i++)
            {
                //LogManager.Logger.LogInfo("------------------------------------------------------------------i : " + i);
                if (MarkerPool.markerIdInPlanet.ContainsKey(planetId) && i < markerIdInPlanet[planetId].Count)
                {
                    var num = markerIdInPlanet[planetId][i];
                    TransformPosAndDraw(i, num);
                }
                else
                {
                    markers[i].gameObject.SetActive(false);
                }
            }
        }

        public static void TransformPosAndDraw(int i, int num6)
        {
            //var num6 = GameMain.localPlanet.id * 100 + i;

            Vector3 localPosition = GameCamera.main.transform.localPosition;
            Vector3 forward = GameCamera.main.transform.forward;

            float realRadius = GameMain.localPlanet.realRadius;    //高さ

            Vector3 vector;
            vector = markerPool[num6].pos.normalized * (realRadius + 15f);
            Vector3 vector2 = vector - localPosition;
            float magnitude = vector2.magnitude;
            float num = Vector3.Dot(forward, vector2);


            if (magnitude < 1f || num < 1f)
            {
                markers[i].SetActive(false);
            }
            else
            {

                Vector2 vector3;
                bool flag = UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), MarkerPrefab.markerGroup.GetComponent<RectTransform>(), out vector3);
                if (Mathf.Abs(vector3.x) > 8000f)
                {
                    flag = false;
                }
                if (Mathf.Abs(vector3.y) > 8000f)
                {
                    flag = false;
                }
                RCHCPU rchcpu;
                if (Phys.RayCastSphere(localPosition, vector2 / magnitude, magnitude, Vector3.zero, realRadius, out rchcpu))
                {

                    flag = false;
                }
                if (flag || markerPool[num6].throughPlanet)
                {


                    UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), MarkerPrefab.markerGroup.GetComponent<RectTransform>(), out vector3);

                    Vector3 pinPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, markerPool[num6].pos);
                    Vector3 playerPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, GameMain.mainPlayer.position);

                    //LogManager.Logger.LogInfo(" pinPos  " + pinPos.x + " : " + pinPos.y + " : " + pinPos.z);
                    //LogManager.Logger.LogInfo(" playerPos  " + playerPos.x + " : " + playerPos.y + " : " + playerPos.z);


                    //Vector3 dirction = (pinPos - playerPos).normalized;
                    //float distance = (pinPos - playerPos).magnitude;
                    //LogManager.Logger.LogInfo(" distance  " + distance);



                    int UIheight = DSPGame.globalOption.uiLayoutHeight;
                    int UIwidth = UIheight * Screen.width / Screen.height;
                    if (markerPool[num6].alwaysDisplay)
                    {

                        vector3.x = Mathf.Round(vector3.x);
                        if (vector3.x < 30)
                        {
                            vector3.x = 30;
                        }
                        else if (vector3.x > UIwidth - 30)
                        {
                            vector3.x = UIwidth - 30;
                        }

                        vector3.y = Mathf.Round(vector3.y);
                        if (vector3.y < 30)
                        {
                            vector3.y = 30;
                        }
                        else if (vector3.y > UIheight - 30)
                        {
                            vector3.y = UIheight - 30;
                        }
                        else if (Phys.RayCastSphere(localPosition, vector2 / magnitude, magnitude, Vector3.zero, realRadius, out rchcpu))
                        {
                            float num3 = vector3.y / (vector3.x - UIwidth / 2);
                            float num4 = UIheight / (UIwidth / 2);
                            if (num3 > num4)
                            {
                                //LogManager.Logger.LogInfo("num3 > num4");

                            }
                            else if (-num3 > num4)
                            {
                                //LogManager.Logger.LogInfo("num3 < - num4");

                            }



                            float num5 = vector3.y / num3;

                        }

                    }


                    markers[i].GetComponent<RectTransform>().anchoredPosition = vector3;


                    if (magnitude < 50)
                    {
                        MarkerPrefab.pinBasePrefab.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    }
                    else if (magnitude < 250)
                    {
                        float num2 = (float)(0.8 - magnitude * 0.002);
                        markers[i].transform.localScale = new Vector3(1, 1, 1) * num2;
                    }
                    else
                    {
                        markers[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    }


                    markers[i].SetActive(true);
                }
                else
                {

                    markers[i].SetActive(false);
                }
            }

        }


    }
}