using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using RPGSystems.StatSystem;
using RPGSystems.StatSystem.Database;
using RPGSystems.Utility.Editor;
using UtilitySystem.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGAttributeEditorExtension : IEditorExtension {
        private Vector2 scroll = Vector2.zero;

        public bool CanHandleType(Type type) {
            return typeof(RPGAttributeAsset).IsAssignableFrom(type);
        }

        public void OnGUI(object asset) {
            var stat = asset as RPGAttributeAsset;

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Stat Linkers");
            if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(30))) {
                XmlDatabaseEditorUtility.GetGenericMenu(RPGStatLinkerEditorUtility.GetNames(), (index) => {
                    var newLinker = RPGStatLinkerEditorUtility.CreateAsset(index);
                    stat.StatLinkers.Add(newLinker);
                    EditorWindow.FocusWindowIfItsOpen<RPGStatCollectionWindow>();
                }).ShowAsContext();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            scroll = GUILayout.BeginScrollView(scroll, false, true);
            if (stat.StatLinkers.Count > 0) {
                for (int i = 0; i < stat.StatLinkers.Count; i++) {
                    var linker = stat.StatLinkers[i];
                    if (linker != null) {
                        GUILayout.BeginHorizontal(EditorStyles.miniButton);
                        GUILayout.Label(string.Format("Linker Type: {0}", linker.GetType().Name));
                        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(30))) {
                            stat.StatLinkers.RemoveAt(i);
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(-3);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(8);
                        GUILayout.BeginVertical("Box");

                        foreach (var extension in RPGStatLinkerEditorUtility.GetExtensions()) {
                            //Debug.Log("CHecking linker of type : " + linker.GetType().ToString());
                            if (extension.CanHandleType(linker.GetType())) {
                                extension.OnGUI(linker);
                            }
                        }
                        GUILayout.EndVertical();
                        GUILayout.Space(4);
                        GUILayout.EndHorizontal();
                    } else {
                        stat.StatLinkers.RemoveAt(i);
                        GUILayout.Label("Error: Linker is null");
                    }
                }


            } else {
                GUILayout.Label("Empty", EditorStyles.centeredGreyMiniLabel);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}