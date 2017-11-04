using System;

namespace Minecrafty.World
{
    internal static class TerrainGenerator
    {
        private const int MaxTerrainHeight = 20;
        private const double LongWaveFrequency = 0.0324563;
        private const double LongWaveAmplitude = 255;
        private const double ShortWaveAmplitude = 10;
        private const int ShortWaveFrequency = 5;

        public static int GetTerrainHeight(int x, int z)
        {
            double longWaveHeightX =  LongWaveAmplitude * Math.Sin(LongWaveFrequency * x);
            double shortWaveHeightX = ShortWaveAmplitude * Math.Cos(ShortWaveFrequency * x);

            double longWaveHeightZ = LongWaveAmplitude * Math.Sin(LongWaveFrequency * z);
            double shortWaveHeightZ = ShortWaveAmplitude * Math.Cos(ShortWaveFrequency * z);

            double normalizedHeight = (longWaveHeightX + longWaveHeightZ + shortWaveHeightX + shortWaveHeightZ) /
                    (2 * LongWaveAmplitude + 2 * ShortWaveAmplitude);

            Console.WriteLine("Normalized height: " + normalizedHeight);

            return (int)(normalizedHeight * MaxTerrainHeight);
        }
    }
}
