

namespace SzymonPeszek.Misc
{
    public static class ExtensionMethods
    {
        public static float Remap(this float value, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            return (value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
        }
    }
}