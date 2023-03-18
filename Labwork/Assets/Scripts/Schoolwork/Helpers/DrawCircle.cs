using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork.Helpers
{
    //Create a popints of circle than feed it to the line renderer
    public class DrawCircle : MonoBehaviour
    {

        public LineRenderer circleRenderer;
        [SerializeField]
        private int _steps = 100;
        [SerializeField]
        private float _radious = 0.75f;
        public float RadiousToDraw
        {
            get { return _radious; }
            set { _radious = value; }
        }

        void Start()
        {
            Draw(_steps, _radious);
        }

        void Draw(int steps, float radious)
        {
            circleRenderer.positionCount = steps;

            for (int currentSteps = 0; currentSteps < steps; currentSteps++)
            {
                float circumferenceProgress = (float)currentSteps / steps;

                float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radious;
                float y = yScaled * radious;

                Vector3 currentPosition = new Vector3(x, y, 0);
                circleRenderer.SetPosition(currentSteps, currentPosition);
            }
        }
    }
}
