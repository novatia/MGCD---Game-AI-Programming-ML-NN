using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DodgeBall
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableObjectComponent : MonoBehaviour
    {
        // STATIC

        private static float s_MaxLifeTime = 5f;

        // Fields

        private Rigidbody m_Body = null;

        private bool m_Thrown = false;
        private float m_LiveTimer = 0f;

        // MonoBehaviour's interface

        private void Awake()
        {
            m_Body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (m_Thrown)
            {
                m_LiveTimer = Mathf.Max(0f, m_LiveTimer - Time.deltaTime);

                if (m_LiveTimer == 0f)
                {
                    Destroy(gameObject);
                }
            }
        }

        // LOGIC

        public void Throw(Vector3 i_Direction, float i_ForceMagnitude)
        {
            if (m_Thrown)
                return;

            m_Body.AddForce(i_Direction * i_ForceMagnitude);

            m_LiveTimer = s_MaxLifeTime;

            m_Thrown = true;
        }
    }
}