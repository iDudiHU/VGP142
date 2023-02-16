using UnityEngine;
using UnityEngine.UI;

namespace Schoolwork.UI
{
    public class PlayerHitUI : MonoBehaviour
    {
        float timeElapsed;
        float lerpDuration = 1;
        float startValue = 1;
        float endValue = 0;
        float valueToLerp;
        private RawImage image;

        private void Start()
        {
            image = GetComponent<RawImage>();
        }
        void Update()
        {
            if (timeElapsed < lerpDuration)
            {
                Color end = image.color;
                end.a = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                image.color = end;
                timeElapsed += Time.deltaTime;
            }
            else
            {
                Color end = image.color;
                end.a = 1;
                image.color = end;
                timeElapsed = 0;
                gameObject.SetActive(false);
            }
        }
    }
}