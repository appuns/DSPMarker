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
	public class test : MonoBehaviour
    {

		public LineRenderer lineRenderer;
		public bool autoRefresh;
		public int textureIndex;
		public Color color = Color.white;
		public float multiplier = 1f;
		public float alphaMultiplier = 1f;
		public float width = 0.5f;
		public bool spherical = true;
		public bool tiling = true;
		public Vector3 startPoint;
		public Vector3 endPoint;

        public static int pinCount = 0;
        public static Vector3 pinPosition;
        public static bool indicatorOn;
        public static LineGizmo lineGizmo;

        public static int maxCount = 30;

        public static GameObject[] tip = new GameObject[maxCount];
        public static GameObject stationTip;
        public static GameObject tipPrefab;
        public static GameObject signButton;
        public static GameObject Arrow;

        public static GameObject pinBasePrefab = new GameObject();


        public void stationTipCreate()
        {
            stationTip = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Scene UIs/Vein Marks"), GameObject.Find("UI Root/Overlay Canvas/In Game/Scene UIs").transform) as GameObject;
            stationTip.name = "stationTip";
            Destroy(stationTip.GetComponent<UIVeinDetail>());

            ////ボタンの作成
            //signButton = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Game Menu/detail-func-group/dfunc-1"), GameObject.Find("UI Root/Overlay Canvas/In Game/Game Menu/detail-func-group").transform) as GameObject;
            //signButton.name = "signButton";
            //signButton.transform.localPosition = new Vector3(-155, 163, 0);
            //signButton.GetComponent<UIButton>().tips.tipTitle = "Station Contents".Translate(); // "ステーション情報";
            //signButton.GetComponent<UIButton>().tips.tipText = "Click to turn ON / OFF real-time contents of stations".Translate(); //"ステーションのストレージ情報を表示/非表示します。";
            ////ボタンイベントの作成
            //signButton.GetComponent<UIButton>().button.onClick.AddListener(new UnityAction(OnSignButtonClick));

            //tipPrefab
            tipPrefab = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Scene UIs/Vein Marks/vein-tip-prefab"), stationTip.transform) as GameObject;
            tipPrefab.name = "tipPrefab";
            tipPrefab.GetComponent<Image>().sprite = tipPrefab.GetComponent<Image>().sprite;
            tipPrefab.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);  //new Color(0.23f, 0.45f, 0.7f, 0.2f);
            tipPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            tipPrefab.GetComponent<Image>().enabled = true;
            //tipPrefab.transform.Find("info-text").GetComponent<Shadow>().enabled = true;
            tipPrefab.transform.localPosition = new Vector3(200, 800, 0);
            tipPrefab.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            Destroy(tipPrefab.GetComponent<UIVeinDetailNode>());
            tipPrefab.SetActive(false);




        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                pinPosition = GameMain.mainPlayer.position;

                LogManager.Logger.LogInfo("pinPosition : " + pinPosition);
                pinCount++;

                /////////////////////////////////////////sign作成でマーカーを作ってみる！！
                ////まずはentity作成
                //EntityData entitydata = new EntityData();
                //PlanetFactory factory = GameMain.data.localPlanet.factory;
                //entitydata.pos = pinPosition;
                //entitydata.id = GameMain.data.localPlanet.factory.AddEntityData(entitydata);
                //LogManager.Logger.LogInfo(" entitydata.id  " + entitydata.id);

                //Vector3 vector0 = pinPosition;
                //vector0.Normalize();

                ////次にSign作成

                //vector0 *= pinPosition.magnitude + 10f;


                //factory.entitySignPool[entitydata.id].iconType = 1U;
                ////1U =　アイテムID 
                ////2U = レシピID
                ////3U = techIda
                ////4U = signalId
                //factory.entitySignPool[entitydata.id].signType = 5U;
                //factory.entitySignPool[entitydata.id].iconId0 = 1001U;
                //factory.entitySignPool[entitydata.id].iconId1 = 1002U;
                //factory.entitySignPool[entitydata.id].iconId2 = 1003U;
                //factory.entitySignPool[entitydata.id].iconId3 = 1004U;
                //factory.entitySignPool[entitydata.id].count0 = 11111f;
                //factory.entitySignPool[entitydata.id].count1 = 22222f;
                //factory.entitySignPool[entitydata.id].count2 = 33333f;
                //factory.entitySignPool[entitydata.id].count3 = 44444f;
                //factory.entitySignPool[entitydata.id].x = vector0.x;
                //factory.entitySignPool[entitydata.id].y = vector0.y;
                //factory.entitySignPool[entitydata.id].z = vector0.z;
                //factory.entitySignPool[entitydata.id].w = 10f;


                //LogManager.Logger.LogInfo(" pinPos  " + pinPosition.x + " : " + pinPosition.y + " : " + pinPosition.z);
                //LogManager.Logger.LogInfo(" vector0.Pos  " + vector0.x + " : " + vector0.y + " : " + vector0.z);


            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                pinPosition = GameMain.mainPlayer.position;

                LogManager.Logger.LogInfo("pinPosition : " + pinPosition);
                pinCount++;

                ///////////////////////////////////////sign作成でマーカーを作ってみる！！
                //まずはentity作成
                EntityData entitydata = new EntityData();
                PlanetFactory factory = GameMain.data.localPlanet.factory;
                entitydata.pos = pinPosition;
                entitydata.id = GameMain.data.localPlanet.factory.AddEntityData(entitydata);
                LogManager.Logger.LogInfo(" entitydata.id  " + entitydata.id);

                Vector3 vector0 = pinPosition;
                vector0.Normalize();

                //次にSign作成

                vector0 *= pinPosition.magnitude + 10f;


                factory.entitySignPool[entitydata.id].iconType = 4U;
                //1U =　アイテムID 
                //2U = レシピID
                //3U = techIda
                //4U = signalId

                factory.entitySignPool[entitydata.id].signType = 0U;
                factory.entitySignPool[entitydata.id].iconId0 = 501U;
                factory.entitySignPool[entitydata.id].iconId1 = 502U;
                factory.entitySignPool[entitydata.id].iconId2 = 503U;
                factory.entitySignPool[entitydata.id].iconId3 = 504U;
                factory.entitySignPool[entitydata.id].count0 = 11111f;
                factory.entitySignPool[entitydata.id].count1 = 22222f;
                factory.entitySignPool[entitydata.id].count2 = 33333f;
                factory.entitySignPool[entitydata.id].count3 = 44444f;
                factory.entitySignPool[entitydata.id].x = vector0.x;
                factory.entitySignPool[entitydata.id].y = vector0.y;
                factory.entitySignPool[entitydata.id].z = vector0.z;
                factory.entitySignPool[entitydata.id].w = 10f;


                LogManager.Logger.LogInfo(" pinPos  " + pinPosition.x + " : " + pinPosition.y + " : " + pinPosition.z);
                LogManager.Logger.LogInfo(" vector0.Pos  " + vector0.x + " : " + vector0.y + " : " + vector0.z);


            }
            if (Input.GetKeyDown(KeyCode.F3) && lineGizmo == null)
            {
                pinPosition = GameMain.mainPlayer.position;

                LogManager.Logger.LogInfo("インジケーター作成" + pinPosition);

                //インジケーター作成
                //LineGizmo lineGizmo = LineGizmo.Create(1, GameMain.mainPlayer.position, pinPosition);

                lineGizmo = UnityEngine.Object.Instantiate<LineGizmo>(Configs.builtin.lineGizmoPrefab, GameGizmo.gizmoGroup);
                lineGizmo.Reset();
                lineGizmo.transform.position = Vector3.zero;
                lineGizmo.textureIndex = 1;
                lineGizmo.startPoint = GameMain.mainPlayer.position;
                lineGizmo.endPoint = pinPosition;
                lineGizmo.autoRefresh = true;
                lineGizmo.width = 5f;
                lineGizmo.multiplier = 1f;
                lineGizmo.alphaMultiplier = 0.5f;
                lineGizmo.spherical = true;     //地面に沿う？
                lineGizmo.tiling = true;       //タイリング？
                lineGizmo.color = Color.cyan;
                lineGizmo.gameObject.SetActive(true);


            }
            if (lineGizmo != null)
            {
                lineGizmo.startPoint = GameMain.mainPlayer.position;
            }












            if (pinCount == 0)
            {
                return;
            }
            if (UIGame.viewMode == EViewMode.Normal || UIGame.viewMode == EViewMode.Globe)
            {
                stationTip.SetActive(true);
            }
            else
            {
                stationTip.SetActive(false);
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
            if (localPlanet.factory == null)
            {
                return;
            }









            //マーカー作成






            Vector3 localPosition = GameCamera.main.transform.localPosition;
            Vector3 forward = GameCamera.main.transform.forward;

            float realRadius = localPlanet.realRadius;    //高さ

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
                bool flag = UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), stationTip.GetComponent<RectTransform>(), out vector3);
                //if (Mathf.Abs(vector3.x) > 8000f)
                //{
                //    flag = false;
                //}
                //if (Mathf.Abs(vector3.y) > 8000f)
                //{
                //    flag = false;
                //}
                RCHCPU rchcpu;
                //if (Phys.RayCastSphere(localPosition, vector2 / magnitude, magnitude, Vector3.zero, realRadius, out rchcpu))
                //{
                //    flag = false;
                // }
                //if (flag)
                //{


                UIRoot.ScreenPointIntoRect(GameCamera.main.WorldToScreenPoint(vector), stationTip.GetComponent<RectTransform>(), out vector3);

                Vector3 pinPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, pinPosition);
                Vector3 playerPos = RectTransformUtility.WorldToScreenPoint(GameCamera.main, GameMain.mainPlayer.position);

                //LogManager.Logger.LogInfo(" pinPos  " + pinPos.x + " : " + pinPos.y + " : " + pinPos.z);
                //LogManager.Logger.LogInfo(" playerPos  " + playerPos.x + " : " + playerPos.y + " : " + playerPos.z);


                //Vector3 dirction = (pinPos - playerPos).normalized;
                //float distance = (pinPos - playerPos).magnitude;
                //LogManager.Logger.LogInfo(" distance  " + distance);

                if (Arrow != null)
                {
                    //var direction = pinPosition - Arrow.transform.position;
                    //direction.y = 0;

                    //var lookRotation = Quaternion.LookRotation(direction, Vector3.up);a
                    //Arrow.transform.rotation = Quaternion.Lerp(Arrow.transform.rotation, lookRotation, 0.1f);
                    //Arrow.transform.LookAt(pinPosition, Vector3.up);
                    // Arrow.transform.rotation = Quaternion.LookRotation(dirction);
                    //Arrow.transform.rotation = Quaternion.Euler(dirction.x, dirction.z, 0);
                    //Arrow.transform.localScale = new Vector3(1, 1, 5);
                }


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


                //tipPrefab.GetComponent<RectTransform>().anchoredPosition = vector3;
                pinBasePrefab.GetComponent<RectTransform>().anchoredPosition = vector3;


                if (magnitude < 50)
                {
                    //tipPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    pinBasePrefab.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (magnitude < 250)
                {
                    float num2 = (float)(1.15 - magnitude * 0.003);
                    //tipPrefab.transform.localScale = new Vector3(1, 1, 1) * num2;
                    pinBasePrefab.transform.localScale = new Vector3(1, 1, 1) * num2;
                }
                else
                {
                    //tipPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    pinBasePrefab.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                }


                //tipPrefab.transform.Find("info-text").GetComponent<Text>().text = pinPosition.x + " : " + pinPosition.y + " : " + pinPosition.z;
                //tipPrefab.SetActive(true);
                pinBasePrefab.SetActive(true);

                //}else
                //{
                //    tipPrefab.SetActive(false);


                //}
            }
        }






    }


}
