using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    [Serializable]
    public class Perceptron
    {
        // STATIC

        private static int s_MaxTrainingEpoch = 10000;

        private static float s_OutputTolerance = 0.0001f;
        private static float s_ErrorTolerance = 0.0001f;

        private static float s_WeightMinStartingValue = -1f;
        private static float s_WeightMaxStartingValue = 1f;
        private static float s_BiasMinStartingValue = -1f;
        private static float s_BiasMaxStartingValue = 1f;

        // Fields

        private List<float> m_Weights = null;
        private float m_Bias = 0f;

        private event Action m_OnDataChangedEvent = null;

        // ACCESSORS

        public int weightCount
        {
            get
            {
                return m_Weights.Count;
            }
        }

        public float bias
        {
            get
            {
                return m_Bias;
            }
        }

        public event Action onDataChangedEvent
        {
            add { m_OnDataChangedEvent += value; }
            remove { m_OnDataChangedEvent -= value; }
        }

        // LOGIC

        public void RandomReset()
        {
            RandomReset(m_Weights.Count);
        }

        public void RandomReset(int i_InputCount)
        {
            Internal_Reset(i_InputCount);
        }

        public PerceptronParams ExtractParams()
        {
            PerceptronParams param = new PerceptronParams();
            param.SetWeightCount(m_Weights.Count);
            for (int index = 0; index < param.weightCount; ++index)
            {
                param.SetWeight(index, GetWeight(index));
            }
            param.SetBias(m_Bias);
            return param;
        }

        public void ApplyParams(PerceptronParams i_Param)
        {
            if (i_Param == null)
            {
                return;
            }

            Internal_Reset(i_Param.weightCount);

            for (int index = 0; index < i_Param.weightCount; ++index)
            {
                m_Weights[index] = i_Param.GetWeight(index);
            }

            m_Bias = i_Param.bias;

            if (m_OnDataChangedEvent != null) m_OnDataChangedEvent();
        }

        public float GetWeight(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Weights.Count)
            {
                return 0f;
            }

            return m_Weights[i_Index];
        }

        public void SetWeight(int i_Index, float i_Weight)
        {
            if (i_Index < 0 || i_Index >= m_Weights.Count)
            {
                return;
            }

            m_Weights[i_Index] = i_Weight;

            if (m_OnDataChangedEvent != null) m_OnDataChangedEvent();
        }

        public void SetBias(float i_Bias)
        {
            m_Bias = i_Bias;

            if (m_OnDataChangedEvent != null) m_OnDataChangedEvent();
        }

        public float Run(List<float> i_Inputs)
        {
            float output;
            TryRun(i_Inputs, out output);
            return output;
        }

        public bool TryRun(List<float> i_Inputs, out float o_Output)
        {
            o_Output = 0f;

            if (!IsInputValid(i_Inputs))
            {
                return false;
            }

            float output = 0f;
            int dataCount = m_Weights.Count;

            for (int index = 0; index < dataCount; ++index)
            {
                float input = i_Inputs[index];
                float weight = m_Weights[index];

                float current = input * weight;
                output += current;
            }

            output += m_Bias;

            o_Output = Internal_Firing(output);
            return true;
        }

        public float Run(TrainingSetEntry i_TrainingSetEntry)
        {
            float output;
            TryRun(i_TrainingSetEntry, out output);
            return output;
        }

        public bool TryRun(TrainingSetEntry i_TrainingSetEntry, out float o_Output)
        {
            o_Output = 0f;

            if (!IsInputValid(i_TrainingSetEntry))
            {
                return false;
            }

            float output = 0f;
            int dataCount = m_Weights.Count;

            for (int index = 0; index < dataCount; ++index)
            {
                float input = i_TrainingSetEntry.GetInput(index);
                float weight = m_Weights[index];

                float current = input * weight;
                output += current;
            }

            output += m_Bias;

            o_Output = Internal_Firing(output);
            return true;
        }

        public bool IsInputValid(List<float> i_Inputs)
        {
            int inputCount = (i_Inputs != null) ? i_Inputs.Count : 0;
            return IsInputValid(inputCount);
        }

        public bool IsInputValid(TrainingSetEntry i_TrainingSetEntry)
        {
            int inputCount = (i_TrainingSetEntry != null) ? i_TrainingSetEntry.inputCount : 0;
            return IsInputValid(inputCount);
        }

        public bool IsInputValid(int i_InputCount)
        {
            if (i_InputCount != m_Weights.Count)
            {
                return false;
            }

            return true;
        }

        public int Train(TrainingSet i_TrainingSet, int i_MaxEpochCount = 0, bool i_ResetData = true)
        {
            if (i_TrainingSet == null)
            {
                return -1;
            }

            if (i_ResetData)
            {
                Internal_Reset(i_TrainingSet.entryInputSize);
            }

            int trainingSetEntryCount = i_TrainingSet.entryCount;

            bool bIsTrained = IsTrainedFor(i_TrainingSet);
            int currentEpoch = 0;

            while (!bIsTrained && (i_MaxEpochCount <= 0 || currentEpoch < i_MaxEpochCount) && currentEpoch < s_MaxTrainingEpoch)
            {
                ++currentEpoch;

                for (int trainingSetEntryIndex = 0; trainingSetEntryIndex < trainingSetEntryCount; ++trainingSetEntryIndex)
                {
                    TrainingSetEntry trainingSetEntry = i_TrainingSet.GetEntry(trainingSetEntryIndex);

                    float error;
                    IsTrainedFor(trainingSetEntry, out error);

                    Internal_UpdatePerceptronParams(trainingSetEntry, error);
                }

                bIsTrained = IsTrainedFor(i_TrainingSet);
            }

            if (m_OnDataChangedEvent != null) m_OnDataChangedEvent();

            return (bIsTrained) ? currentEpoch : -1;
        }

        public bool IsTrainedFor(TrainingSet i_TrainingSet)
        {
            float totalError;
            return IsTrainedFor(i_TrainingSet, out totalError);
        }

        public bool IsTrainedFor(TrainingSet i_TrainingSet, out float o_TotalError)
        {
            o_TotalError = 0f;

            if (i_TrainingSet == null)
            {
                return false;
            }

            bool bIsTrained = true;

            for (int trainingSetEntryIndex = 0; trainingSetEntryIndex < i_TrainingSet.entryCount; ++trainingSetEntryIndex)
            {
                TrainingSetEntry trainingSetEntry = i_TrainingSet.GetEntry(trainingSetEntryIndex);

                float error;
                bool bCurrentIsTrained = IsTrainedFor(trainingSetEntry, out error);

                o_TotalError += (bCurrentIsTrained ? 0f : Mathf.Abs(error));

                bIsTrained &= bCurrentIsTrained;
            }

            return bIsTrained;
        }

        public bool IsTrainedFor(TrainingSetEntry i_TrainingSetEntry)
        {
            float error;
            return IsTrainedFor(i_TrainingSetEntry, out error);
        }

        public bool IsTrainedFor(TrainingSetEntry i_TrainingSetEntry, out float o_Error)
        {
            o_Error = 0f;

            if (i_TrainingSetEntry == null)
            {
                return false;
            }

            if (!IsInputValid(i_TrainingSetEntry))
            {
                return false;
            }

            float output = Run(i_TrainingSetEntry);
            float desiredOutput = i_TrainingSetEntry.output;

            float error = desiredOutput - output;
            float absError = Mathf.Abs(error);

            if (absError > s_ErrorTolerance)
            {
                o_Error += error;
            }

            return (o_Error == 0f);
        }

        // INTERNALS

        private float Internal_Firing(float i_Value)
        {
            return (i_Value > s_OutputTolerance) ? 1f : 0f;
        }

        private void Internal_Reset(int i_InputCount)
        {
            m_Weights.Clear();

            for (int index = 0; index < i_InputCount; ++index)
            {
                float weight = UnityEngine.Random.Range(s_WeightMinStartingValue, s_WeightMaxStartingValue);
                m_Weights.Add(weight);
            }

            m_Bias = UnityEngine.Random.Range(s_BiasMinStartingValue, s_BiasMaxStartingValue);

            if (m_OnDataChangedEvent != null) m_OnDataChangedEvent();
        }

        private void Internal_UpdatePerceptronParams(TrainingSetEntry i_TrainingSetEntry, float i_Error)
        {
            if (i_TrainingSetEntry == null)
                return;

            if (!IsInputValid(i_TrainingSetEntry))
                return;

            // Update weights.

            for (int weightIndex = 0; weightIndex < m_Weights.Count; ++weightIndex)
            {
                float currentWeight = m_Weights[weightIndex];
                float input = i_TrainingSetEntry.GetInput(weightIndex);

                float newWeight = input * i_Error + currentWeight;
                m_Weights[weightIndex] = newWeight;
            }

            // Update bias.

            m_Bias += i_Error;
        }

        // CTOR

        public Perceptron(int i_InputCount = 0)
        {
            m_Weights = new List<float>();

            Internal_Reset(i_InputCount);
        }
    }
}