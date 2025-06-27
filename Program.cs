using System;

namespace CustomProject
{
    /// <summary>
    /// Entry point for the game.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            /// <summary>
            /// Setup game.
            /// </summary>
            GameManager.GetInstance().InitialiseGame();

            /// <summary>
            /// Continuous game loop calling each game phase sequentially.
            /// </summary>
            while (true)
            {
                GameManager.GetInstance().RunCurrentPhase();
            }
        }
    }
}