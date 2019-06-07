using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DodgeBall
{
    public class BrainComponent : MonoBehaviour
    {
        // STATIC

        private static int s_AnimatorId_Trigger_Crouch = Animator.StringToHash("Crouch");

        // Serializable fields

        [SerializeField]
        private Gun m_Gun = null;

        // Fields

        private Perceptron m_Perceptron = null;
        private TrainingSet m_TrainingSet = null;

        private Animator m_AnimatorComponent = null;
        private Rigidbody m_BodyComponent = null;

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
        
        // MonoBehaviour's interface

        private void Awake()
        {
            m_Perceptron = new Perceptron();
            m_TrainingSet = new TrainingSet();

            m_AnimatorComponent = GetComponent<Animator>();
            m_BodyComponent = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            m_Perceptron.RandomReset(2);
        }

        private void OnEnable()
        {
            if (m_Gun != null)
            {
                m_Gun.onBulletShotEvent += OnBulletShot;
            }
        }

        private void OnDisable()
        {
            if (m_Gun != null)
            {
                m_Gun.onBulletShotEvent -= OnBulletShot;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Internal_Reset();
            }
        }

        // INTERNALS

        private void Internal_AddNewTrainingSetEntry(float i_RedGreenInput, float i_SphereCubeInput)
        {
            TrainingSetEntry trainingSetEntry = new TrainingSetEntry("");
            trainingSetEntry.AddInput(i_RedGreenInput);
            trainingSetEntry.AddInput(i_SphereCubeInput);

            float output = (i_RedGreenInput < 0.5f && i_SphereCubeInput < 0.5f) ? 0f : 1f;
            trainingSetEntry.SetOutput(output);

            m_TrainingSet.AddEntry(trainingSetEntry);
        }

        private void Internal_Train(int i_MaxEpochCount = 0)
        {
            m_Perceptron.Train(m_TrainingSet, i_MaxEpochCount, false);
        }

        private void Internal_Reset()
        {
            m_TrainingSet.Clear();
            m_Perceptron.RandomReset(2);
        }

        private void Internal_Dodge()
        {
            if (m_AnimatorComponent != null)
            {
                m_AnimatorComponent.SetTrigger(s_AnimatorId_Trigger_Crouch);   
            }

            if (m_BodyComponent != null)
            {
                m_BodyComponent.isKinematic = true;
            }
        }

        private void Internal_Idle()
        {
            if (m_BodyComponent != null)
            {
                m_BodyComponent.isKinematic = false;
            }
        }

        // EVENTS

        private void OnBulletShot(float i_RedGreenInput, float i_SphereCubeInput)
        {
            // Decide what to do.

            List<float> inputs = new List<float>();
            inputs.Add(i_RedGreenInput);
            inputs.Add(i_SphereCubeInput);

            float output = m_Perceptron.Run(inputs);

            if (output == 0f)
            {
                Internal_Dodge();
            }
            else
            {
                Internal_Idle();
            }

            // Add information to training.

            Internal_AddNewTrainingSetEntry(i_RedGreenInput, i_SphereCubeInput);
            Internal_Train();
        }
    }
}