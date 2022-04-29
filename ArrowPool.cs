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
    internal class ArrowPool : MonoBehaviour
    {
        public static GameObject guideArrowBase;
        public static GameObject[] guideArrow;


        public static void Update()
        {
            int planetId = GameMain.localPlanet.id;
            if (!MarkerPool.markerIdInPlanet.ContainsKey(planetId))
            {
                List<int> list = new List<int>();
                MarkerPool.markerIdInPlanet.Add(planetId, list);
            }
            if (!MarkerList.showList)
            {
                guideArrowBase.gameObject.SetActive(false);
                return;
            }

            if (guideArrowBase)
            {
                if (!MarkerList.showList || GameMain.localPlanet != null && !GameMain.data.mainPlayer.sailing)
                {
                    GameObject Player = GameMain.data.mainPlayer.gameObject;
                    if (Player.activeSelf)
                    {

                        Plane plane = new Plane(Player.transform.up, Player.transform.position);
                        guideArrowBase.gameObject.SetActive(true);
                        

                        for (int i = 0; i < Main.maxMarker; i++)
                        {
                            //var num = GameMain.localPlanet.id * 100 + i;

                            if (i < MarkerPool.markerIdInPlanet[planetId].Count)
                            {
                                var num = MarkerPool.markerIdInPlanet[planetId][i];
                                if (MarkerPool.markerPool[num].ShowArrow)
                                {
                                    var point = MarkerPool.markerPool[num].pos;
                                    var planePoint = plane.ClosestPointOnPlane(point);

                                    //guideArrow[i].transform.localPosition = new Vector3(0, 0.8f, 0);
                                    //var direction = planePoint - Player.transform.position;
                                    guideArrow[i].transform.LookAt(planePoint, Player.transform.up);
                                    guideArrow[i].GetComponent<MeshRenderer>().material.color = MarkerPool.markerPool[num].color;
                                    guideArrow[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", MarkerPool.markerPool[num].color);
                                    guideArrow[i].gameObject.SetActive(true);
                                }else
                                {
                                    guideArrow[i].gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                guideArrow[i].gameObject.SetActive(false);
                            }
                        }
                    }
                }
                else
                {
                    guideArrowBase.gameObject.SetActive(false);
                }
            }
            else
            {
                guideArrow = new GameObject[Main.maxMarker];

                //作成
                GameObject Player = GameMain.data.mainPlayer.gameObject;

                guideArrowBase = new GameObject("guideArrowBase");
                guideArrowBase.transform.parent = Player.transform;
                guideArrowBase.transform.localPosition = new Vector3(0, 0.8f, 0);
                guideArrowBase.transform.localScale = new Vector3(1, 1, 1);

                for (int i = 0; i < Main.maxMarker; i++)
                //for (int i = 0; i < 1; i++)
                {
                    guideArrow[i] = new GameObject("guideArrow" + i);
                    guideArrow[i].transform.parent = guideArrowBase.transform;
                    guideArrow[i].AddComponent<CreateTriangleMesh>();
                    guideArrow[i].AddComponent<MeshRenderer>();
                    guideArrow[i].AddComponent<MeshFilter>();
                    guideArrow[i].transform.localPosition = new Vector3(0, 0, 0);
                    //guideArrow[i].SetActive(true);
                }
                //ArrowRed = new GameObject("ArrowRed");
                //ArrowRed.transform.parent = guideArrowBase.transform;
                //ArrowRed.AddComponent<CreateTriangleMesh>();
                //ArrowRed.AddComponent<MeshRenderer>();
                //ArrowRed.AddComponent<MeshFilter>();
                //ArrowRed.transform.localPosition = new Vector3(0, 0, 0);
                //ArrowRed.SetActive(true);

            }

        }
    }

    public class CreateTriangleMesh : MonoBehaviour
    {
        void Start()
        {
            var mesh = new Mesh();

            var Vertices = new List<Vector3> {
                      new Vector3 (-0.4f,0, 2.4f),
                      new Vector3 (0, 0, 3f),
                      new Vector3 (0.4f, 0, 2.4f),
                      new Vector3 (0, 0, 2.6f),
               };
            mesh.SetVertices(Vertices);
            var triangles = new List<int> { 0, 1, 3, 1, 2, 3, 3, 2, 1, 3, 1, 0 };
            mesh.SetTriangles(triangles, 0);

            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var renderer = GetComponent<MeshRenderer>();
            renderer.material.color = Color.blue; // new Color(1, 0.7f, 0, 1);
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", Color.blue);// new Color(1, 0.7f, 0, 1));
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

    }

    //public class CreateTriangleMeshOutline : MonoBehaviour
    //{
    //    void Start()
    //    {
    //        var mesh = new Mesh();

    //        var Vertices = new List<Vector3> {
    //                  new Vector3 (-0.45f,0, 2.45f),
    //                  new Vector3 (0, 0, 3.1f),
    //                  new Vector3 (0.45f, 0, 2.45f),
    //                  new Vector3 (0, 0, 2.5f),
    //           };
    //        mesh.SetVertices(Vertices);
    //        var triangles = new List<int> { 0, 1, 3, 1, 2, 3, 3, 2, 1, 3, 1, 0 };
    //        mesh.SetTriangles(triangles, 0);

    //        var meshFilter = GetComponent<MeshFilter>();
    //        meshFilter.mesh = mesh;

    //        var renderer = GetComponent<MeshRenderer>();
    //        renderer.material.color = Color.blue; // new Color(1, 0.7f, 0, 1);
    //        renderer.material.EnableKeyword("_EMISSION");
    //        renderer.material.SetColor("_EmissionColor", Color.blue);// new Color(1, 0.7f, 0, 1));
    //        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    //        renderer.receiveShadows = false;
    //    }

    //}

}
