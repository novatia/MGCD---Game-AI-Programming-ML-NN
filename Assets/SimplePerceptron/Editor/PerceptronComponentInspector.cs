using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    [CustomEditor(typeof(PerceptronComponent))]
    public class PerceptronComponentInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
                return;

            PerceptronComponent perceptronComponent = target as PerceptronComponent;

            if (perceptronComponent == null)
                return;

            Perceptron perceptron = perceptronComponent.perceptron;

            if (perceptron == null)
                return;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.LabelField("Perceptron details", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Weights", EditorStyles.miniBoldLabel);

            for (int weightIndex = 0; weightIndex < perceptron.weightCount; ++weightIndex)
            {
                float weight = perceptron.GetWeight(weightIndex);
                EditorGUILayout.LabelField("W" + (weightIndex + 1) + " = " + weight.ToString("F2"));
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField("Bias = " + perceptron.bias.ToString("F2"));

            EditorGUILayout.EndVertical();

            Repaint();
        }
    }
}