using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DodgeBall
{
    public delegate void BulletShotEventCallback(float i_RedGreenInput, float i_SphereCubeInput);

    [Serializable]
    public class GunBulletData
    {
        // Serializable fields

        [SerializeField]
        private ThrowableObjectComponent m_Prefab = null;
        [SerializeField]
        private float m_RedGreenInput = 0f;
        [SerializeField]
        private float m_SphereCubeInput = 0f;
        [SerializeField]
        private KeyCode m_ShotKey = KeyCode.S;

        // ACCESSORS

        public ThrowableObjectComponent prefab
        {
            get
            {
                return m_Prefab;
            }
        }

        public float redGreenInput
        {
            get
            {
                return m_RedGreenInput;
            }
        }

        public float sphereCubeInput
        {
            get
            {
                return m_SphereCubeInput;
            }
        }

        public KeyCode shotKey
        {
            get
            {
                return m_ShotKey;
            }
        }
    }

    public class Gun : MonoBehaviour
    {
        // Serializable fields

        [SerializeField]
        private GunBulletData[] m_GunBullets = null;
        [SerializeField]
        private float m_GunForce = 500f;
        [SerializeField]
        private float m_GunCooldown = 2f;

        // Fields

        private float m_GunCooldownTimer = 0f;

        private event BulletShotEventCallback m_OnBulletShotEvent = null;

        // ACCESSORS

        public event BulletShotEventCallback onBulletShotEvent
        {
            add { m_OnBulletShotEvent += value; }
            remove { m_OnBulletShotEvent -= value; }
        }

        // MonoBehaviour's interface

        private void Update()
        {
            if (m_GunCooldownTimer == 0f)
            {
                if (m_GunBullets != null)
                {
                    for (int index = 0; index < m_GunBullets.Length; ++index)
                    {
                        GunBulletData gunBulletData = m_GunBullets[index];

                        if (gunBulletData == null || gunBulletData.prefab == null)
                            continue;

                        if (Input.GetKeyDown(gunBulletData.shotKey))
                        {
                            Internal_Shot(gunBulletData);
                        }
                    }
                }
            }
            else
            {
                m_GunCooldownTimer = Mathf.Max(0f, m_GunCooldownTimer - Time.deltaTime);
            }
        }

        // INTERNALS

        private void Internal_Shot(GunBulletData i_GunBulletData)
        {
            if (i_GunBulletData == null || i_GunBulletData.prefab == null)
                return;

            ThrowableObjectComponent gunBulletInstance = Instantiate<ThrowableObjectComponent>(i_GunBulletData.prefab);
            gunBulletInstance.transform.position = transform.position;
            gunBulletInstance.transform.rotation = transform.rotation;

            gunBulletInstance.Throw(transform.forward, m_GunForce);

            if (m_OnBulletShotEvent != null)
            {
                m_OnBulletShotEvent(i_GunBulletData.redGreenInput, i_GunBulletData.sphereCubeInput);
            }

            m_GunCooldownTimer = m_GunCooldown;
        }
    }
}