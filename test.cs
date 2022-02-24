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
	public class CalcPos : MonoBehaviour
    {


        public static int pinCount = 0;
        public static Vector3 pinPosition;




        public static void Update2()
        {
            //if (Input.GetKeyDown(KeyCode.F3) && lineGizmo == null)
            //{
            //    pinPosition = GameMain.mainPlayer.position;

            //    LogManager.Logger.LogInfo("インジケーター作成" + pinPosition);

            //    //インジケーター作成
            //    //LineGizmo lineGizmo = LineGizmo.Create(1, GameMain.mainPlayer.position, pinPosition);
            //    pinCount = 1;

            //}












            //if (pinCount == 0)
            //{
            //    return;
            //}
            //if (UIGame.viewMode == EViewMode.Normal || UIGame.viewMode == EViewMode.Globe)
            //{
            //    MarkerPrefab.markerGroup.SetActive(true);
            //}
            //else
            //{
            //    MarkerPrefab.markerGroup.SetActive(false);
            //    return;
            //}
            //if (GameMain.data == null)
            //{
            //    return;
            //}
            //PlanetData localPlanet = GameMain.data.localPlanet;
            //if (localPlanet == null)
            //{
            //    return;
            //}
            //if (localPlanet.factory == null)
            //{
            //    return;
            //}









            //マーカー作成


            LogManager.Logger.LogInfo("-----------------------------------------------------------------------------マーカー作成 1");




            Vector3 localPosition = GameCamera.main.transform.localPosition;
            Vector3 forward = GameCamera.main.transform.forward;

            float realRadius = GameMain.localPlanet.realRadius;    //高さ

            Vector3 vector;
            vector = pinPosition.normalized * (realRadius + 15f);
            Vector3 vector2 = vector - localPosition;
            float magnitude = vector2.magnitude;
            float num = Vector3.Dot(forward, vector2);


            if (magnitude < 1f || num < 1f)
            {
                //tipPrefab.SetActive(false);
                //pinBasePrefab.SetActive(false);
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
                if (flag)
                {


                    UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), MarkerPrefab.markerGroup.GetComponent<RectTransform>(), out vector3);

                    Vector3 pinPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, pinPosition);
                    Vector3 playerPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, GameMain.mainPlayer.position);

                    //LogManager.Logger.LogInfo(" pinPos  " + pinPos.x + " : " + pinPos.y + " : " + pinPos.z);
                    //LogManager.Logger.LogInfo(" playerPos  " + playerPos.x + " : " + playerPos.y + " : " + playerPos.z);


                    //Vector3 dirction = (pinPos - playerPos).normalized;
                    //float distance = (pinPos - playerPos).magnitude;
                    //LogManager.Logger.LogInfo(" distance  " + distance);



                    int UIheight = DSPGame.globalOption.uiLayoutHeight;
                    int UIwidth = UIheight * Screen.width / Screen.height;

                    vector3.x = Mathf.Round(vector3.x);
                    if (vector3.x < 40)
                    {
                        vector3.x = 40;
                    }
                    else if (vector3.x > UIwidth - 40)
                    {
                        vector3.x = UIwidth - 40;
                    }

                    vector3.y = Mathf.Round(vector3.y);
                    if (vector3.y < 40)
                    {
                        vector3.y = 40;
                    }
                    else if (vector3.y > UIheight - 40)
                    {
                        vector3.y = UIheight - 40;
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


                    MarkerPrefab.pinBasePrefab.GetComponent<RectTransform>().anchoredPosition = vector3;


                    if (magnitude < 50)
                    {
                        MarkerPrefab.pinBasePrefab.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (magnitude < 250)
                    {
                        float num2 = (float)(1.15 - magnitude * 0.003);
                        MarkerPrefab.pinBasePrefab.transform.localScale = new Vector3(1, 1, 1) * num2;
                    }
                    else
                    {
                        MarkerPrefab.pinBasePrefab.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    }


                    MarkerPrefab.pinBasePrefab.SetActive(true);
                }else
                {

                    MarkerPrefab.pinBasePrefab.SetActive(false);
                }
            }

        }




    }


}
