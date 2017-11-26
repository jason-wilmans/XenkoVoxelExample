using System;
using SiliconStudio.Core.Mathematics;

namespace EndlessVoxelExample.World
{
    internal static class TerrainGenerator
    {

        private const int MaxTerrainHeight = 20;

        private const int NoiseScale = 30;

        private static readonly Random Random = new Random();
        private static readonly OpenSimplexNoise SimplexNoise = new OpenSimplexNoise(Random.Next());

        public static int GetTerrainHeight(int worldX, int worldZ)
        {
            double lowerNoiseX = (double)worldX / NoiseScale;
            double lowerNoiseY = (double)worldZ / NoiseScale;
            
            double lowerLeft = SimplexNoise.Evaluate(lowerNoiseX, lowerNoiseY);
            double upperLeft = SimplexNoise.Evaluate(lowerNoiseX, lowerNoiseY + 1);
            double lowerRight = SimplexNoise.Evaluate(lowerNoiseX + 1, lowerNoiseY);
            double upperRight = SimplexNoise.Evaluate(lowerNoiseX + 1, lowerNoiseY + 1);

            double rightAmount = (worldX - lowerNoiseX * NoiseScale) / NoiseScale;
            double forwardAmount = (worldZ - lowerNoiseY * NoiseScale) / NoiseScale;
            double totalAmount = (rightAmount + forwardAmount) / 2;

            double firstDiagonal = MathUtil.Lerp(lowerLeft, upperRight, totalAmount);
            double secondDiagonal = MathUtil.Lerp(upperLeft, lowerRight, totalAmount);
            double value = (firstDiagonal + secondDiagonal) / 2;

            return (int) (MaxTerrainHeight * value);
        }
    }
}