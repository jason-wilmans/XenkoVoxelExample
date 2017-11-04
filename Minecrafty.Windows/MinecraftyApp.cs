using SiliconStudio.Xenko.Engine;

namespace Minecrafty
{
    class MinecraftyApp
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
