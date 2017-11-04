using SiliconStudio.Xenko.Engine;

namespace EndlessVoxelExample
{
    class EndlessVoxelExampleApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
