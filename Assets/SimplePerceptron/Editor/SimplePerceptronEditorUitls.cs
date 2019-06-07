using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    public static class SimplePerceptronEditorUitls
    {
        [MenuItem("Assets/Create/SimplePerceptron/TrainingSet")]
        public static void CreateTrainingSet()
        {
            ScriptableObjectUtility.CreateAsset<TrainingSet>();
        }
    }
}