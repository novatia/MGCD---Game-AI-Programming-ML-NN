using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SimplePerceptron
{
    public class GraphPointData
    {
        // Fields

        private float m_X = 0f;
        private float m_Y = 0f;

        private Color m_Color = Color.black;

        private GraphPointComponent m_GraphPointInstance = null;

        // ACCESSORS

        public float x
        {
            get
            {
                return m_X;
            }
        }

        public float y
        {
            get
            {
                return m_Y;
            }
        }

        public Vector2 position
        {
            get
            {
                return new Vector2(m_X, m_Y);
            }
        }

        public Color color
        {
            get
            {
                return m_Color;
            }
        }

        // LOGIC

        public void Init(GraphComponent i_Graph, GraphPointComponent i_GraphPointPrefab)
        {
            Uninit();

            if (i_GraphPointPrefab == null)
                return;

            m_GraphPointInstance = GameObject.Instantiate<GraphPointComponent>(i_GraphPointPrefab);
            m_GraphPointInstance.Init(i_Graph);

            SetPosition(0f, 0f);
        }

        public void Uninit()
        {
            if (m_GraphPointInstance != null)
            {
                GameObject.Destroy(m_GraphPointInstance.gameObject);
                m_GraphPointInstance = null;
            }
        }

        public void SetX(float i_X)
        {
            SetPosition(i_X, m_Y);
        }

        public void SetY(float i_Y)
        {
            SetPosition(m_X, i_Y);
        }

        public void SetPosition(float i_X, float i_Y)
        {
            m_X = i_X;
            m_Y = i_Y;

            if (m_GraphPointInstance != null)
            {
                m_GraphPointInstance.SetPosition(m_X, m_Y);
            }
        }

        public void SetColor(Color i_Color)
        {
            if (m_GraphPointInstance != null)
            {
                m_GraphPointInstance.SetColor(i_Color);
            }
        }
    }

    public class GraphComponent : MonoBehaviour
    {
        // Serializable Fields

        [SerializeField]
        private GraphPointComponent m_GraphPointPrefab = null;

        // Fields

        private RectTransform m_RectTransformComponent = null;
        private List<GraphPointData> m_Points = new List<GraphPointData>();

        // ACCESSORS

        public float graphWidth
        {
            get
            {
                if (m_RectTransformComponent != null)
                {
                    return m_RectTransformComponent.rect.width;
                }

                return 0f;
            }
        }

        public float graphHeight
        {
            get
            {
                if (m_RectTransformComponent != null)
                {
                    return m_RectTransformComponent.rect.height;
                }

                return 0f;
            }
        }

        public int pointCount
        {
            get
            {
                return m_Points.Count;
            }
        }

        // MonoBehaviour's interface

        private void Awake()
        {
            m_RectTransformComponent = GetComponent<RectTransform>();
        }

        // LOGIC

        public void Clear()
        {
            for (int index = m_Points.Count - 1; index >= 0; --index)
            {
                RemovePoint(index);
            }
        }

        public void AddPoint(float i_X, float i_Y, Color i_Color)
        {
            GraphPointData newGraphPointData = Internal_SpawnGraphPointData();

            if (newGraphPointData == null)
                return;

            newGraphPointData.SetPosition(i_X, i_Y);
            newGraphPointData.SetColor(i_Color);
        }

        public void RemovePoint(int i_Index)
        {
            GraphPointData pointData = Internal_GetGraphPoint(i_Index);

            if (pointData == null)
                return;

            pointData.Uninit();

            m_Points.RemoveAt(i_Index);
        }

        public void SetPointPosition(int i_Index, float i_X, float i_Y)
        {
            GraphPointData pointData = Internal_GetGraphPoint(i_Index);

            if (pointData != null)
            {
                pointData.SetPosition(i_X, i_Y);
            }
        }

        public void SetPointColor(int i_Index, Color i_Color)
        {
            GraphPointData pointData = Internal_GetGraphPoint(i_Index);

            if (pointData != null)
            {
                pointData.SetColor(i_Color);
            }
        }

        // INTERNALS

        private GraphPointData Internal_GetGraphPoint(int i_Index)
        {
            if (i_Index < 0 || i_Index >= m_Points.Count)
            {
                return null;
            }

            return m_Points[i_Index];
        }

        private GraphPointData Internal_SpawnGraphPointData()
        {
            GraphPointData pointData = new GraphPointData();
            pointData.Init(this, m_GraphPointPrefab);

            m_Points.Add(pointData);

            return pointData;
        }
    }
}