using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    public class PerceptronComponent : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private TrainingSet m_TrainingSet = null;
        [SerializeField]
        private int m_MaxTrainingEpoch = 0;

        [SerializeField]
        private bool m_ForcePerceptronData = false;
        [SerializeField]
        private PerceptronParams m_ForcedPerceptronParams = null;

        // Fields

        private Perceptron m_Perceptron = null;

        private event Action m_OnPerceptronDataChangedEvent = null;

        // ACCESSORS

        public Perceptron perceptron
        {
            get
            {
                return m_Perceptron;
            }
        }

        public TrainingSet trainingSet
        {
            get
            {
                return m_TrainingSet;
            }
        }

        public event Action onPerceptronDataChangedEvent
        {
            add { m_OnPerceptronDataChangedEvent += value; }
            remove { m_OnPerceptronDataChangedEvent -= value; }
        }

        // MonoBehaviour's interface

        private void Awake()
        {
            m_Perceptron = new Perceptron();
        }

        private void OnEnable()
        {
            m_Perceptron.onDataChangedEvent += OnPerceptronDataChanged;
        }

        private void OnDisable()
        {
            m_Perceptron.onDataChangedEvent -= OnPerceptronDataChanged;
        }

        private void Start()
        {
            m_Perceptron.RandomReset((m_TrainingSet != null) ? m_TrainingSet.entryInputSize : 0);

            Internal_ForceDataOnPerceptron();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Internal_Train();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Internal_Reset();
            }
        }

        // INTERNALS

        private void Internal_ForceDataOnPerceptron()
        {
            if (m_ForcePerceptronData)
            {
                m_Perceptron.ApplyParams(m_ForcedPerceptronParams);
            }
        }

        private void Internal_Train()
        {
            Internal_ForceDataOnPerceptron();

            int epochToTrain = m_Perceptron.Train(m_TrainingSet, m_MaxTrainingEpoch, !m_ForcePerceptronData);
            bool trainingSuccess = (epochToTrain >= 0);

            Debug.Log("Training result: " + ((trainingSuccess) ? "SUCCESS!" : "FAIL!"));
            if (trainingSuccess)
            {
                Debug.Log("Training ended in " + epochToTrain + " epochs.");

                Debug.Log("Perceptron weights:");

                for (int index = 0; index < m_Perceptron.weightCount; ++index)
                {
                    float weight = m_Perceptron.GetWeight(index);
                    Debug.Log("     Weight " + (index + 1) + ". " + weight.ToString("F2") + ".");
                }

                Debug.Log("Perceptron bias: " + m_Perceptron.bias.ToString("F2"));
            }
        }

        private void Internal_Reset()
        {
            m_Perceptron.RandomReset();
        }

        private void OnPerceptronDataChanged()
        {
            if (m_OnPerceptronDataChangedEvent != null)
            {
                m_OnPerceptronDataChangedEvent();
            }
        }
    }
}