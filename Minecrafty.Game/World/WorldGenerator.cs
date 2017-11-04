using System;
using System.Collections.Generic;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;

namespace Minecrafty.World
{
    public class WorldGenerator : SyncScript
    {
        public TransformComponent Player;

        private const int ViewDistance = 20;

        private IDictionary<Coordinate, List<Entity>> _existingVoxelModels;
        Prefab _voxelPrefab;

        public override void Start()
        {
            base.Start();

            _existingVoxelModels = new Dictionary<Coordinate, List<Entity>>();
            _voxelPrefab = Content.Load<Prefab>("Prefabs/VoxelPrefab");
        }

        public override void Update()
        {
            Coordinate roundedPlayerPosition = new Coordinate(Player.Position.X, Player.Position.Y, Player.Position.Z);

            CreateMissingVoxels(roundedPlayerPosition);
            DeleteOutOfRangeVoxels(roundedPlayerPosition);
        }

        private void CreateMissingVoxels(Coordinate roundedPlayerPosition)
        {
            int halfDistance = ViewDistance / 2;
            for (int x = -halfDistance; x <= halfDistance; x++)
            {
                for (int z = -halfDistance; z <= halfDistance; z++)
                {
                    Coordinate surroundingCoordinate = new Coordinate(roundedPlayerPosition.X + x, 0, roundedPlayerPosition.Z + z);
                    CreateVoxelIfNotExists(surroundingCoordinate);
                }
            }
        }

        private void DeleteOutOfRangeVoxels(Coordinate roundedPlayerPosition)
        {
            foreach (Coordinate coordinate in _existingVoxelModels.Keys)
            {
                if (_existingVoxelModels.ContainsKey(coordinate) && IsOutOfRange(coordinate, roundedPlayerPosition))
                {
                    foreach (Entity entity in _existingVoxelModels[coordinate])
                    {
                        SceneSystem.SceneInstance.RootScene.Entities.Remove(entity);
                    }
                }
            }
        }

        private bool IsOutOfRange(Coordinate coordinate, Coordinate roundedPlayerPosition)
        {
            return Math.Abs(coordinate.X - roundedPlayerPosition.X) > ViewDistance / 2 ||
                   Math.Abs(coordinate.Z - roundedPlayerPosition.Z) > ViewDistance / 2;
        }

        private void CreateVoxelIfNotExists(Coordinate coordinate)
        {
            if (!_existingVoxelModels.ContainsKey(coordinate))
            {
                Console.WriteLine($"Creating ({coordinate.X}, {coordinate.Z})");

                List<Entity> entities = _voxelPrefab.Instantiate();
                SceneSystem.SceneInstance.RootScene.Entities.AddRange(entities);
                int y = TerrainGenerator.GetTerrainHeight(coordinate.X, coordinate.Z);

                foreach (var entity in entities)
                {
                    entity.Transform.Position = new Vector3(coordinate.X, y, coordinate.Z);
                }
                _existingVoxelModels[coordinate] = entities;
            }
        }
    }

    public struct Coordinate
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Coordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Coordinate(float x, float y, float z)
            :this ((int)x, (int)y, (int)z)
        {
        }
    }
}
