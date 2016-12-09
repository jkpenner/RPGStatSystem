using UnityEngine;
using UnityEditor;
using RPGSystems.StatSystem.Database;
using UtilitySystems.XmlDatabase.Editor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RPGSystems.StatSystem.Editor {
    [CustomEditor(typeof(RPGStatCollection))]
    public class RPGStatCollectionEditor : UnityEditor.Editor {
        private int increaseAmount = 1;
        private List<int> activeStatIds = new List<int>();
        private bool showStatModifiers = false;

        public override void OnInspectorGUI() {
            var collection = (RPGStatCollection)target;

            DisplayCollectionAssetGUI(collection);

            GUILayout.Space(4);

            DisplayCollectionLevelGUI(collection);

            GUILayout.Space(4);

            DisplayCollectionGUI(collection);
        }

        private void DisplayCollectionAssetGUI(RPGStatCollection collection) {
            // Determine the string to display for the given stat collection
            var asset = RPGStatCollectionDatabase.Instance.Get(collection.StatCollectionId);
            string displayText;
            // If the asset is found within the database use it's name.
            if (asset != null) {
                displayText = asset.Name;
            }
            // If the id is below zero no collection is assigned.
            else if (collection.StatCollectionId <= 0) {
                displayText = "Not Set";
            }
            // If no asset is assigned and the id i above zero
            // the previous collection is currently missing.
            else {
                displayText = "Missing";
            }

            GUILayout.Space(4);

            // Show the collection's name and id and allow user to change
            // the assigned stat collection via a dialog popup
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            if (GUILayout.Button(string.Format("[ID: {0}] {1}", Mathf.Max(0, collection.StatCollectionId).ToString("D4"), displayText), EditorStyles.toolbarPopup)) {
                RPGStatCollectionDatabase.Instance.LoadDatabase();
                XmlDatabaseEditorUtility.ShowContext(RPGStatCollectionDatabase.Instance, (value) => {
                    collection.StatCollectionId = value.Id;
                }, typeof(RPGStatCollectionWindow));
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DisplayCollectionLevelGUI(RPGStatCollection collection) {
            // Show controls for editing values of stats in the editor
            GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            GUILayout.Label("Collection Level");
            GUILayout.EndHorizontal();

            GUILayout.Space(-4);

            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            if (collection.ScaledLevel == 0) {
                GUILayout.Label("Level", EditorStyles.miniButtonLeft, GUILayout.Width(80));
            } else {
                GUILayout.Label("Level(Scaled)", EditorStyles.miniButtonLeft, GUILayout.Width(80));
            }

            GUILayout.Label(collection.Level.ToString(), EditorStyles.miniButtonMid);
            if (GUILayout.Button("+", EditorStyles.miniButtonMid, GUILayout.Width(40))) {
                if (Application.isPlaying) {
                    collection.ScaleToLevel(collection.Level + 1);
                } else {
                    collection.SetLevel(collection.NormalLevel + 1);
                }
            }
            if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(40))) {
                if (Application.isPlaying) {
                    collection.ScaleToLevel(collection.Level - 1);
                } else {
                    collection.SetLevel(collection.NormalLevel - 1);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Exp", EditorStyles.miniButtonLeft, GUILayout.Width(80));
            GUILayout.Label(collection.CurrentExp.ToString(), EditorStyles.miniButtonMid);
            GUILayout.Label("", EditorStyles.miniButtonRight, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DisplayCollectionGUI(RPGStatCollection collection) {
            // Show controls for editing values of stats in the editor
            GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            GUILayout.Label("Stats");
            GUILayout.Label("Increment By", GUILayout.Width(90));
            // Increase the amount stat values are adjusted by multiples of 10
            if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(24)))
                increaseAmount = Mathf.Min(increaseAmount * 10, 10000);
            // Shows the current amount stat values are adjusted by in editor
            GUILayout.Label(increaseAmount.ToString("D5"), GUILayout.Width(40));
            // Decrease the amount stat values are adjusted by multiples of 10
            if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(24)))
                increaseAmount = Mathf.Max(increaseAmount / 10, 1);
            GUILayout.EndHorizontal();

            GUILayout.Space(-4);

            // Show all the stats within the editor only when the
            // editor is playing, else the stat list will not be
            // initialized.
            GUILayout.BeginVertical("Box");
            if (Application.isPlaying) {
                var groups = collection.StatDict.GroupBy(pair => {
                    var category = RPGStatCategoryDatabase.Instance.Get(pair.Value.StatCategoryId);
                    if (category != null) {
                        return category.Id;
                    }
                    return 0;
                });

                foreach (var group in groups) {
                    GUILayout.Label(RPGStatCategoryDatabase.Instance.Get(group.First().Value.StatCategoryId).Name, EditorStyles.centeredGreyMiniLabel);
                    foreach (var pair in group) {
                        var currentValue = pair.Value as IStatValueCurrent;

                        bool isActive = activeStatIds.Contains(pair.Key);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(string.Format("Id: {0, 6}", pair.Key.ToString()), EditorStyles.miniButtonLeft, GUILayout.Width(80));
                        GUILayout.Label(RPGStatTypeDatabase.Instance.Get(pair.Key).Name, EditorStyles.miniButtonMid);

                        if (currentValue == null) {
                            GUILayout.Label(string.Format("{0, 13}", pair.Value.StatValue), EditorStyles.miniButtonMid, GUILayout.Width(120));
                        } else {
                            GUILayout.Label(string.Format("{0, 6}", currentValue.StatValueCurrent), EditorStyles.miniButtonMid, GUILayout.Width(60));
                            GUILayout.Label(string.Format("{0, 6}", pair.Value.StatValue), EditorStyles.miniButtonMid, GUILayout.Width(60));
                        }

                        var clicked = GUILayout.Toggle(isActive, isActive ? '\u25BC'.ToString() : '\u25B6'.ToString(), EditorStyles.miniButtonRight, GUILayout.Width(60));
                        if (clicked == true && isActive == false) {
                            activeStatIds.Add(pair.Key);
                        } else if (clicked == false && isActive == true) {
                            activeStatIds.Remove(pair.Key);
                        }
                        GUILayout.EndHorizontal();


                        // Show the name and id of the current stat
                        //var clicked = GUILayout.Toggle(isActive, string.Format("ID {0, 4}: {1, -20}", 
                        //    pair.Key.ToString(), statName), EditorStyles.foldout);
                        //
                        //if (clicked) {
                        //    activeStatId = pair.Key;
                        //}

                        if (activeStatIds.Contains(pair.Key) == false) continue;

                        GUILayout.Space(-3);

                        // Display the stat's value along with controls to 
                        // modifiy the value by the current increase amount.
                        GUILayout.BeginVertical("Box");
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Value", GUILayout.Width(60));
                        GUILayout.Label(pair.Value.StatValue.ToString("D8"), EditorStyles.centeredGreyMiniLabel);

                        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(30))) {
                            pair.Value.ModifyBaseValue(increaseAmount);
                        }

                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(30))) {
                            pair.Value.ModifyBaseValue(-increaseAmount);
                        }
                        GUILayout.EndHorizontal();

                        // If the stat implements IStatCurrentValue display the
                        // current value of the given stat.
                        if (currentValue != null) {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Current", GUILayout.Width(60));
                            GUILayout.Label(currentValue.StatValueCurrent.ToString("D8"), EditorStyles.centeredGreyMiniLabel);
                            if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(30))) {
                                currentValue.StatValueCurrent += increaseAmount;
                            }

                            if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(30))) {
                                currentValue.StatValueCurrent -= increaseAmount;
                            }
                            GUILayout.EndHorizontal();
                        }

                        var iModifiable = pair.Value as IStatModifiable;
                        if (iModifiable != null) {
                            EditorGUI.indentLevel++;
                            GUILayout.BeginHorizontal();
                            showStatModifiers = GUILayout.Toggle(showStatModifiers, "Active Modifiers", EditorStyles.miniButton);
                            GUILayout.EndHorizontal();

                            if (showStatModifiers) {
                                GUILayout.BeginHorizontal("Box");
                                for (int i = 0; i < iModifiable.GetModifierCount(); i++) {
                                    var mod = iModifiable.GetModifierAt(i);

                                    if (mod != null) {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label(mod.GetType().Name);
                                        GUILayout.Label(mod.Value.ToString());
                                        if (GUILayout.Button("-", EditorStyles.miniButton)) {
                                            iModifiable.RemoveModifier(mod);
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                }

                                if (iModifiable.GetModifierCount() <= 0) {
                                    GUILayout.Label("No Active Modifier", EditorStyles.centeredGreyMiniLabel);
                                }
                                GUILayout.EndHorizontal();
                            }
                            EditorGUI.indentLevel--;
                        }

                        GUILayout.EndVertical();
                    }
                }

                if (collection.StatCollectionId <= 0) {
                    GUILayout.Label("No Stat Collection Selected", EditorStyles.centeredGreyMiniLabel);
                } else if (collection.StatDict.Count <= 0) {
                    GUILayout.Label("No Stats are contained within the collection", EditorStyles.centeredGreyMiniLabel);
                }
            } else {
                GUILayout.Label("Stats Appear in Playmode", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.EndVertical();
        }
    }
}
