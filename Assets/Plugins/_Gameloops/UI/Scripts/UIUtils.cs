using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameloops.Player;
using Gameloops.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameloops
{
    public class UIUtils : MonoBehaviour
    {
        [Inject] private PlayerResource _playerResource;
        [Inject] private HapticManager _hapticManager;
        [Inject] private ResourceView _resourceView;
        private static EventSystem _tempEventSystem;
        private static PointerEventData _tempPointerEventData;
        [SerializeField] private LayerMask currentGuiLayers = 1 << 7;

        public IEnumerator IncrementScore(int from, int to, float duration, Action<int> onUpdate = null, Action onComplete = null)
        {
            int start = from;
            int score;

            for (float timer = 0; timer < duration; timer += Time.deltaTime) 
            {
                float progress = timer / duration;
                score = (int)Mathf.Lerp (start, to, progress);
                onUpdate?.Invoke(score);
                yield return null;
            }
            onUpdate?.Invoke(to);
            onComplete?.Invoke();
        }
        public void AddResourcesWithAnimation(Transform from, ResourceData resource, int score, 
            float scale = 1f, float spreadFactor = 100f, float moveToResourceDuration = 0.35f)
        {
            var amount = score / (float)scale;
            var remainder = Mod(score, scale);
            var runTime = Mathf.FloorToInt(amount);
            for (int i = 0; i < runTime; i++)
            {
                var currency = new GameObject("ResourceImage")
                {
                    transform =
                    {
                        parent = from
                    }
                };
                currency.AddComponent<Image>().sprite = resource.unitPreviewSprite;
                currency.transform.SetPositionAndRotation(from.position, Quaternion.identity);
                var currentPos = currency.transform.position;
                
                var randomPos = new Vector3(
                    currentPos.x + Random.Range(-spreadFactor, spreadFactor), 
                    currentPos.y + Random.Range(-spreadFactor, spreadFactor), 
                    currentPos.z);

                currency.transform.DOMove(randomPos, moveToResourceDuration).SetDelay(Random.Range(0f, moveToResourceDuration/2f)).OnComplete(() =>
                {
                    var finalResourceTransform = _resourceView.GetResourceTransform(resource);
                    currency.transform.DOMove(finalResourceTransform.position, 0.5f)
                        .OnComplete(() =>
                        {
                            finalResourceTransform.localScale = Vector3.one;
                            finalResourceTransform.DOScale(1.25f, 0.2f)
                                .OnComplete(() => finalResourceTransform.DOScale(1, .2f));
                            _hapticManager.HapticNotification(HapticNotificationType.Success);
                            Destroy(currency.gameObject);
                            _playerResource.GainResource(resource, scale);
                        });
                });
            }
            
            _playerResource.GainResource(resource, remainder);
        }
        public float Mod(int a, float n)
        {
            var result = a % n;
            if ((result<0 && n>0) || (result>0 && n<0)) {
                result += n;
            }
            return result;
        }
        public string MakeKmb(int num )
        {
            double numStr;
            if( num < 1000 )
            {
                return num.ToString();
            }
            else if( num < 1000000 )
            {
                numStr = num/1000f;
                return numStr.ToString("F1") + "K";
            }
            else if( num < 1000000000f )
            {
                numStr = num/1000000f;
                return numStr.ToString("F1") + "M";
            }
            else
            {
                numStr = num/1000000000f;
                return  numStr.ToString("F1") + "B";
            }
            
        }
        public string FormatTime(int timeInSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(timeInSeconds);
            return timeSpan.Minutes.ToString("00") + "m " + timeSpan.Seconds.ToString("00") + "s";
        }
        public  bool IsPointOverGui(Vector2 screenPosition)
		{
			return RaycastGui(screenPosition, currentGuiLayers).Count > 0;
		}

        private static List<RaycastResult> RaycastGui(Vector2 screenPosition, LayerMask layerMask)
		{
            List<RaycastResult> tempRaycastResults = new List<RaycastResult>();
            tempRaycastResults.Clear();

			var currentEventSystem = GetEventSystem();

			if (currentEventSystem != null)
			{
				// Create point event data for this event system?
				if (currentEventSystem != _tempEventSystem)
				{
					_tempEventSystem = currentEventSystem;

					if (_tempPointerEventData == null)
					{
						_tempPointerEventData = new PointerEventData(_tempEventSystem);
					}
					else
					{
						_tempPointerEventData.Reset();
					}
				}

				// Raycast event system at the specified point
				_tempPointerEventData.position = screenPosition;

				currentEventSystem.RaycastAll(_tempPointerEventData, tempRaycastResults);

				// Loop through all results and remove any that don't match the layer mask
				if (tempRaycastResults.Count > 0)
				{
					for (var i = tempRaycastResults.Count - 1; i >= 0; i--)
					{
						var raycastResult = tempRaycastResults[i];
						var raycastLayer  = 1 << raycastResult.gameObject.layer;

						if ((raycastLayer & layerMask) == 0)
						{
							tempRaycastResults.RemoveAt(i);
						}
					}
				}
			}
			else
			{
				Debug.LogError("Failed to RaycastGui because your scene doesn't have an event system! To add one, go to: GameObject/UI/EventSystem");
			}

			return tempRaycastResults;
		}

        private static EventSystem GetEventSystem()
        {
            var currentEventSystem = EventSystem.current;

            if (currentEventSystem == null)
            {
                currentEventSystem = FindObjectOfType<EventSystem>();
            }

            return currentEventSystem;
        }

    }
}