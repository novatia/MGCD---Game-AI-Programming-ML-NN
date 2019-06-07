using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DodgeBall
{
    [CustomEditor(typeof(BrainComponent))]
    public class BrainComponentInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
                return;

            BrainComponent brainComponent = target as BrainComponent;

            if (brainComponent == null)
                return;

            Perceptron p = brainComponent.perceptron;
            TrainingSet ts = brainComponent.trainingSet;

            if (p == null || ts == null)
                return;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Brain additional info", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField("Perceptron", EditorStyles.boldLabel);

                    EditorGUILayout.BeginVertical("Box");
                    {
                        EditorGUILayout.LabelField("Weights", EditorStyles.boldLabel);

                        for (int weightIndex = 0; weightIndex < p.weightCount; ++weightIndex)
                        {
                            float weight = p.GetWeight(weightIndex);
                            EditorGUILayout.LabelField("W" + (weightIndex + 1) + " = " + weight.ToString("F2"));
                        }
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.LabelField("Bias = " + p.bias.ToString("F2"));
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField("TrainingSet", EditorStyles.boldLabel);

                    for (int rowIndex = 0; rowIndex < ts.entryCount; ++rowIndex)
                    {
                        TrainingSetEntry tsEntry = ts.GetEntry(rowIndex);

                        if (tsEntry == null)
                            continue;

                        EditorGUILayout.BeginHorizontal();
                        {
                            string label = (rowIndex + 1) + ". ";
                            for (int inputIndex = 0; inputIndex < tsEntry.inputCount; ++inputIndex)
                            {
                                float input = tsEntry.GetInput(inputIndex);
                                label += "I" + (inputIndex + 1) + "=" + input.ToString("F2") + " ";
                            }
                            label += "O=" + tsEntry.output.ToString("F2");

                            EditorGUILayout.LabelField(label, EditorStyles.label);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            Repaint();
        }
    }
}