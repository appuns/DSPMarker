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
        public static Dictionary<int, Marker> markerPool = new Dictionary<int, Marker>();

        //public static int Main.maxMarker = 20;
        public static bool markerEnable = true;

        public static GameObject[] markers = new GameObject[Main.maxMarker];
        public static Image[] Base = new Image[Main.maxMarker];
        public static Image[] BaseRound = new Image[Main.maxMarker];
        public static Image[] icon1s = new Image[Main.maxMarker];
        public static Image[] icon2s = new Image[Main.maxMarker];
        public static Text[] decs = new Text[Main.maxMarker];



        public struct Marker
        {
            //public int planetID;
            public Vector3 pos;
            public int icon1ID;
            public int icon2ID;
            public Color color;
            public string desc;
            public bool alwaysDisplay;
            public bool throughPlanet;
            public bool ShowArrow;
        }




        public static void Create()
        {
            for (int i = 0; i < Main.maxMarker; i++)
            {
                markers[i] = Instantiate(MarkerPrefab.pinBasePrefab.gameObject, MarkerPrefab.markerGroup.transform);
                Base[i] = markers[i].GetComponent<Image>();
                BaseRound[i] = markers[i].transform.Find("round").GetComponent<Image>();
                icon1s[i] = markers[i].transform.Find("round/pinBaseIcon1").GetComponent<Image>();
                icon2s[i] = markers[i].transform.Find("round/pinBaseIcon2").GetComponent<Image>();
                decs[i] = markers[i].transform.Find("round/pinBaseText").GetComponent<Text>();
            }


        }

        public static void Refresh()
        {

            for (int i = 0; i < Main.maxMarker; i++)
            {
                var num = GameMain.localPlanet.id * 100 + i;
                if (markerPool.ContainsKey(num))
                {
                    Base[i].color = markerPool[num].color;
                    var halfColor = new Color(markerPool[num].color.r * 0.3f, markerPool[num].color.g * 0.3f, markerPool[num].color.b * 0.3f, 1f);
                    BaseRound[i].color = halfColor;
                    icon1s[i].sprite = LDB.signals.IconSprite(markerPool[num].icon1ID); ;
                    icon2s[i].sprite = LDB.signals.IconSprite(markerPool[num].icon2ID); ;
                    decs[i].text = markerPool[num].desc;
                }
            }


        }
        public static void Update()
        {
            if (!markerEnable)
            {
                return;
            }
            if (UIGame.viewMode != EViewMode.Normal && UIGame.viewMode != EViewMode.Globe && UIGame.viewMode != EViewMode.Build)
            {
                MarkerPrefab.markerGroup.SetActive(false);
                return;
            }
            if (GameMain.data == null)
            {
                return;
            }
            PlanetData localPlanet = GameMain.data.localPlanet;
            if (localPlanet == null)
            {
                return;
            }
            MarkerPrefab.markerGroup.SetActive(true);

            Vector3 cameraPosition = GameCamera.main.transform.localPosition;
            float realRadius = localPlanet.realRadius;    //惑星の半径

            for (int i = 0; i < Main.maxMarker; i++)
            {
                //LogManager.Logger.LogInfo("------------------------------------------------------------------i : " + i);

                var num = GameMain.localPlanet.id * 100 + i;
                if (markerPool.ContainsKey(num))
                {
                    //LogManager.Logger.LogInfo("------------------------------------------------------------------num : " + num);
                    Vector3 vector = markerPool[num].pos;
                    //Vector3 vector2 = vector - cameraPosition;
                    //float magnitude = vector2.magnitude;
                    //if (magnitude > 1f)
                    //{
                        Vector2 vector3;
                        bool flag = UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), markers[i].GetComponent<RectTransform>(), out vector3);
                        //if (!markerPool[num].throughPlanet)
                        //{
                        //    if (Mathf.Abs(vector3.x) > 8000f)
                        //    {
                        //        flag = false;
                        //    }
                        //    if (Mathf.Abs(vector3.y) > 8000f)
                        //    {
                        //        flag = false;
                        //    }
                        //    RCHCPU rchcpu;
                        //    if (Phys.RayCastSphere(cameraPosition, vector2 / magnitude, magnitude, Vector3.zero, realRadius, out rchcpu))
                        //    {
                        //        flag = false;
                        //    }
                        //}
                        //if (flag) //見えたら？
                        //{
                        //Vector3 uiPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, vector);
                        vector3.x = Mathf.Round(vector3.x);
                        vector3.y = Mathf.Round(vector3.y);
                        markers[i].GetComponent<RectTransform>().anchoredPosition = vector3;
                            markers[i].gameObject.SetActive(true);
                        //}
                    //}
                }
                else
                {
                    markers[i].gameObject.SetActive(false);
                }
            }
        }
    }
}