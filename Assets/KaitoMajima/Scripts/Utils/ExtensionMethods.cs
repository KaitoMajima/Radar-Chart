using UnityEngine;

namespace KaitoMajima.Utils
{
    public static class FloatExtensionMethods
    {
        public static float Map(this float value, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            float normalizedValue = Mathf.InverseLerp(inputMin, inputMax, value);
            float resultValue = Mathf.Lerp(outputMin, outputMax, normalizedValue);

            return resultValue;
        }

    }

    public static class VectorExtensionMethods
    {
        public static Vector3 LerpAnimationCurve(Vector3 start, Vector3 end, float startedLerpTime, float lerpTime, AnimationCurve curve)
        {
            float timeSinceStart = Time.time - startedLerpTime;

            float completionPercentage = timeSinceStart / lerpTime;

            float percentageInCurve = curve.Evaluate(completionPercentage);

            return Vector3.Lerp(start, end, percentageInCurve);
        }

        
    }
}
