namespace Utils
{
    public static class LayerUtils
    {
        public static bool AreEqual(int firstLayer, int secondLayer)
        {
            return ((1 << firstLayer) & (1 << secondLayer)) != 0;
        }

        public static bool AreNotEqual(int firstLayer, int secondLayer)
        {
            return ((1 << firstLayer) & (1 << secondLayer)) == 0;
        }
    }
}