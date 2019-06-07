using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    public class PerceptronGraphComponent : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private GraphComponent m_GraphComponent = null;

        // Fields

        private PerceptronComponent m_PerceptronComponent = null;

        // MonoBehaviour's interface

        private void Awake()
        {
            m_PerceptronComponent = GetComponent<PerceptronComponent>();
        }

        private void OnEnable()
        {
            if (m_PerceptronComponent != null)
            {
                m_PerceptronComponent.onPerceptronDataChangedEvent += OnPerceptronDataChanged;
            }
        }

        private void OnDisable()
        {
            if (m_PerceptronComponent != null)
            {
                m_PerceptronComponent.onPerceptronDataChangedEvent -= OnPerceptronDataChanged;
            }
        }

        // INTERNALS

        private Perceptron Internal_GetPerceptron()
        {
            return (m_PerceptronComponent != null) ? m_PerceptronComponent.perceptron : null;
        }

        private TrainingSet Internal_GetTrainingSet()
        {
            return (m_PerceptronComponent != null) ? m_PerceptronComponent.trainingSet : null;
        }

        private void Internal_RefreshGraph()
        {
            if (m_GraphComponent == null)
                return;

            float graphWidth = m_GraphComponent.graphWidth;
            float graphHeight = m_GraphComponent.graphHeight;

            m_GraphComponent.Clear();

            Perceptron p = Internal_GetPerceptron();

            if (p == null || p.weightCount < 2)
                return;

            // Draw line: W1 * I1 + W2 * I2 + Bias = 0.

            // Line is perpendicular to (W1, W2) and, along (W1, W2) is distanced 'Bias' from (0, 0).

            float firstX = 0f;
            float firstY = -p.bias / p.GetWeight(1);
            firstX *= graphWidth;
            firstY *= graphHeight;

            float secondX = 1f;
            float secondY = (-p.GetWeight(0) - p.bias) / p.GetWeight(1);
            secondX *= graphWidth;
            secondY *= graphHeight;

            for (int v = 0; v < 100; ++v)
            {
                float t = (float)v / 100f;

                float x = Mathf.Lerp(firstX, secondX, t);
                float y = Mathf.Lerp(firstY, secondY, t);

                m_GraphComponent.AddPoint(x, y, Color.green);
            }

            // Draw points.

            TrainingSet trainingSet = Internal_GetTrainingSet();

            if (trainingSet != null)
            {
                for (int index = 0; index < trainingSet.entryCount; ++index)
                {
                    TrainingSetEntry trainingSetEntry = trainingSet.GetEntry(index);

                    if (trainingSetEntry == null)
                        continue;

                    if (trainingSet.entryInputSize < 2)
                        continue;

                    float x = trainingSetEntry.GetInput(0);
                    float y = trainingSetEntry.GetInput(1);

                    x *= graphWidth;
                    y *= graphHeight;

                    float output = p.Run(trainingSetEntry);
                    Color pointColor = (output > 0.5f) ? Color.blue : Color.red;

                    m_GraphComponent.AddPoint(x, y, pointColor);
                }
            }
        }

        private void OnPerceptronDataChanged()
        {
            Internal_RefreshGraph();
        }
    }
}