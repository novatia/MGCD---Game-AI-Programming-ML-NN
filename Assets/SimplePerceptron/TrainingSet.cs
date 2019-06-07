using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    [Serializable]
    public class TrainingSetEntry
    {
        // Serializable fields

        [SerializeField]
        private string m_EntryName = "";
        [SerializeField]
        private List<float> m_Inputs = null;
        [SerializeField]
        private float m_Output = 0f;

        // ACCESSORS

        public string entryName
        {
            get
            {
                return m_EntryName;
            }
        }

        public int inputCount
        {
            get
            {
                return m_Inputs.Count;
            }
        }

        public float output
        {
            get
            {
                return m_Output;
            }
        }

        // LOGIC

        public float GetInput(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Inputs.Count)
            {
                return 0f;
            }

            return m_Inputs[i_Index];
        }

        // CTOR

        public TrainingSetEntry()
        {
            m_Inputs = new List<float>();
        }
    }

    [Serializable]
    public class TrainingSet : ScriptableObject
    {
        // Serializable fields

        [SerializeField]
        private List<TrainingSetEntry> m_Entries = null;

        // ACCESSORS

        public int entryCount
        {
            get
            {
                return m_Entries.Count;
            }
        }

        public int entryInputSize
        {
            get
            {
                TrainingSetEntry firstEntry = GetEntry(0);
                return (firstEntry != null) ? firstEntry.inputCount : 0;
            }
        }

        // LOGIC

        public TrainingSetEntry GetEntry(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Entries.Count)
            {
                return null;
            }

            return m_Entries[i_Index];
        }

        // CTOR

        public TrainingSet()
        {
            m_Entries = new List<TrainingSetEntry>();
        }
    }
}