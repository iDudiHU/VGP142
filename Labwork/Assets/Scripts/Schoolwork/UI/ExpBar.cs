using System;
using Schoolwork.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Schoolwork.UI
{
    public class ExpBar : MonoBehaviour
    {
        private Slider expSlider;
        private LevelSystem _LevelSystem;
        private void Awake()
        {
            _LevelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
            _LevelSystem.OnExperienceChanged += LevelSystemOnExperienceChanged;
            expSlider = GetComponent<Slider>();
        }

        private void Start()
        {
            SetExperienceBarSize(_LevelSystem.GetExperienceNormalized());
        }

        private void LevelSystemOnExperienceChanged(object sender, System.EventArgs e)
        {
            SetExperienceBarSize(_LevelSystem.GetExperienceNormalized());
        }

        private void SetExperienceBarSize(float experienceNormalized)
        {
            expSlider.value = experienceNormalized;
        }



    }
}