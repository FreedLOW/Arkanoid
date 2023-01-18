using System;
using System.Collections;
using UnityEngine;

namespace NM.UI
{
    public class UITool
    {
        /// <summary>
        /// Changing CanvasGroup state between visible and not visible
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="isVisible"></param>
        public static void State(ref CanvasGroup canvas, bool isVisible)
        {
            canvas.alpha = isVisible ? 1 : 0;
            canvas.interactable = isVisible;
            canvas.blocksRaycasts = isVisible;
        }
   
        /// <summary>
        /// Changing CanvasGroup state between visible and not visible
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="canvas"></param>
        /// <param name="isVisible"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerator State(CanvasGroup canvas, bool isVisible, float delay, Action<CanvasGroup> action = null)
        {
            canvas.alpha = isVisible ? 1 : 0;
            canvas.blocksRaycasts = isVisible;
            yield return new WaitForSecondsRealtime(delay);
            canvas.interactable = isVisible;
            action?.Invoke(canvas);     
        }
    
        /// <summary>
        /// Changing CanvasGroup state between intractable and not intractable, and setting alpha
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="isInteractable"></param>
        /// <param name="alpha"></param>
        public static void State(ref CanvasGroup canvas, bool isInteractable, float alpha)
        {
            canvas.alpha = alpha;
            canvas.interactable = isInteractable;
            canvas.blocksRaycasts = isInteractable;
        }
        
    
        /// <summary>
        /// Converting world position of click to pressed ui rectangle
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static Vector2 UIClickPosition(Transform transform, Vector3 worldPos)
        {
            var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, pos, Camera.main,
                out var rectanglePos);
            return rectanglePos;
        }
    }
}