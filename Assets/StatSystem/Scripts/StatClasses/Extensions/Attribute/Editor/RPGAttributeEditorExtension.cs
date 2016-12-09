using System;
using UnityEngine;
using UnityEditor;
using UtilitySystems.XmlDatabase.Editor;

namespace RPGSystems.StatSystem.Editor {
    public class RPGAttributeEditorExtension : EditorExtension {
        private Vector2 scroll = Vector2.zero;

        public override bool CanHandleType(Type type) {
            return typeof(RPGAttributeAsset).IsAssignableFrom(type);
        }

        public override void OnGUI(object asset) {
            var stat = asset as RPGAttributeAsset;

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Stat Linkers");
            if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(60))) {
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
                        GUILayout.Space(10);

                        // Add space to the left
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(4);


                        GUILayout.BeginVertical("Box");

                        GUILayout.Space(-10);
                        GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
                        EditorGUILayout.LabelField("Linker", linker.GetType().Name);

                        if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(70))) {
                            stat.StatLinkers.RemoveAt(i);
                        }
                        GUILayout.EndHorizontal();

                        

                        foreach (var extension in RPGStatLinkerEditorUtility.GetExtensions()) {
                            //Debug.Log("CHecking linker of type : " + linker.GetType().ToString());
                            if (extension.CanHandleType(linker.GetType())) {
                                extension.OnGUI(linker);
                            }
                        }
                        GUILayout.EndVertical();

                        // Add space to right 
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