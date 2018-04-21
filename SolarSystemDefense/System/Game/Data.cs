namespace SolarSystemDefense
{
    static class Data
    {
        public enum Type
        {
            Mercury = 0,
            Venus = 1,
            Mars = 2,
            Jupiter = 3
        }

        public static int[] ShooterCooldown = { 15, 30, 30, 60 };
    }
}   
