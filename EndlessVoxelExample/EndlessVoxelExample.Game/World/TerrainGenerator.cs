using System;

namespace EndlessVoxelExample.World
{
    internal static class TerrainGenerator
    {
        private const int MaxTerrainHeight = 100;
        private const double LongWaveFrequency = 0.000324563;
        private const double LongWaveAmplitude = 255;
        private const double ShortWaveAmplitude = 68;
        private const double ShortWaveFrequency = 0.08;
        private const double UltraShortWaveFrequency = .3;
        private const double UltraShortWaveAmplitude = 4;

        public static int GetTerrainHeight(int x, int z)
        {
            double longWaveHeightX = LongWaveAmplitude * Math.Sin(LongWaveFrequency * x);
            double shortWaveHeightX = ShortWaveAmplitude * Math.Cos(ShortWaveFrequency * x);
            double ultraShortWaveHeightX = UltraShortWaveAmplitude * Math.Sin(UltraShortWaveFrequency * x);

            double longWaveHeightZ = LongWaveAmplitude * Math.Sin(LongWaveFrequency * z);
            double shortWaveHeightZ = ShortWaveAmplitude * Math.Cos(ShortWaveFrequency * z);
            double ultraShortWaveHeightZ = UltraShortWaveAmplitude * Math.Sin(UltraShortWaveFrequency * z);

            double normalizedHeight = (longWaveHeightX + longWaveHeightZ + shortWaveHeightX + shortWaveHeightZ + ultraShortWaveHeightX + ultraShortWaveHeightZ) /
                                      (2 * LongWaveAmplitude + 2 * ShortWaveAmplitude + 2 * UltraShortWaveAmplitude);

            return (int)(normalizedHeight * MaxTerrainHeight);
        }
    }
}