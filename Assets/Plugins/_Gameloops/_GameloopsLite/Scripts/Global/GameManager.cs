using System;
using UnityEngine;
using Zenject;

namespace Gameloops
{
    public class GameManager : MonoBehaviour
    {

        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
            Time.timeScale = 1f;
        }
    }
}