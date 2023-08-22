using System;
using Lofelt.NiceVibrations;
using UnityEngine;
using Zenject;

namespace Gameloops
{
    public class HapticManager
    {
        private StorageManager _storageManager;

        [Inject]
        public HapticManager(StorageManager storageManager)
        {
            _storageManager = storageManager;
            HapticController.fallbackPreset = HapticPatterns.PresetType.Selection;
        }

        public bool HapticEnabled
        {
            get
            {
                return _storageManager.IsHapticOn;
            }
            set
            {
                _storageManager.IsHapticOn = value;
            }
        }
        private void ForAndroidManifest()
        {
            Handheld.Vibrate();
        }

        public void HapticImpact(HapticImpactType impactType)
        {
            switch (impactType)
            {
                case HapticImpactType.Light:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                    break;
                case HapticImpactType.Medium:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                    break;
                case HapticImpactType.Heavy:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                    break;
                case HapticImpactType.Rigid:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
                    break;
                case HapticImpactType.Soft:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
                    break;
            }
        }


        public void HapticNotification(HapticNotificationType notificationType)
        {
            switch (notificationType)
            {
                case HapticNotificationType.Success:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                    break;
                case HapticNotificationType.Failure:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
                    break;
                case HapticNotificationType.Warning:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
                    break;
            }
        }


        public void ToggleHaptic()
        {
            HapticEnabled = !HapticEnabled;
        }

        public void HapticSelection()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
        }
    }
    public enum HapticImpactType
    {
        Light,
        Medium,
        Heavy,
        Rigid,
        Soft
    }

    public enum HapticNotificationType
    {
        Success,
        Failure,
        Warning
    }
}