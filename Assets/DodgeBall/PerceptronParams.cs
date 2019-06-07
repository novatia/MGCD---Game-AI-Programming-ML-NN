using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DodgeBall
{
    [Serializable]
    public class PerceptronParams
    {
        // Serializable fields

        [SerializeField]
        private List<float> m_Weights = null;
        [SerializeField]
        private float m_Bias = 0f;

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

        // LOGIC

        public void SetWeightCount(int i_Count)
        {
            while (m_Weights.Count != i_Count)
            {
                int diff = m_Weights.Count - i_Count;

                if (diff < 0)
                {
                    m_Weights.Add(0f);
                }
                else
                {
                    m_Weights.RemoveAt(m_Weights.Count - 1);
                }
            }
        }

        public void SetWeight(int i_Index, float i_Weight)
        {
            if (i_Index < 0 || i_Index >= m_Weights.Count)
            {
                return;
            }

            m_Weights[i_Index] = i_Weight;
        }

        public float GetWeight(int i_Index)
        {
            if (i_Index< 0 || i_Index >= m_Weights.Count)
            {
                return 0f;
            }

            return m_Weights[i_Index];
        }

        public void SetBias(float i_Bias)
        {
            m_Bias = i_Bias;
        }

        // CTOR

        public PerceptronParams()
        {
            m_Weights = new List<float>();
        }
    }
}