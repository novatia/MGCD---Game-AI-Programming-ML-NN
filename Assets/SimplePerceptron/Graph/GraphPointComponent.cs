using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    public class GraphPointComponent : MonoBehaviour
    {
        // Fields

        private Image m_ImageComponent = null;
        private RectTransform m_RectTransformComponent = null;

        private bool m_bIsInitialized = false;

        // MonoBehaviour's interface

        private void Awake()
        {
            m_ImageComponent = GetComponent<Image>();
            m_RectTransformComponent = GetComponent<RectTransform>();
        }

        // LOGIC

        public void Init(GraphComponent i_Graph)
        {
            if (m_bIsInitialized)
                return;

            if (m_RectTransformComponent != null)
            {
                m_RectTransformComponent.SetParent((i_Graph != null) ? i_Graph.transform : null, false);

                m_RectTransformComponent.anchorMin = Vector2.zero;
                m_RectTransformComponent.anchorMax = Vector2.zero;
                m_RectTransformComponent.pivot = new Vector2(0.5f, 0.5f);

                m_RectTransformComponent.anchoredPosition = Vector2.zero;
            }

            m_bIsInitialized = true;
        }

        public void Uninit()
        {
            if (!m_bIsInitialized)
                return;

            m_bIsInitialized = false;
        }

        public void SetPosition(float i_X, float i_Y)
        {
            if (m_RectTransformComponent != null)
            {
                m_RectTransformComponent.anchoredPosition = new Vector2(i_X, i_Y);
            }
        }

        public void SetColor(Color i_Color)
        {
            if (m_ImageComponent != null)
            {
                m_ImageComponent.color = i_Color;
            }
        }
    }
}