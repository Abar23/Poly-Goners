using System.Collections.Generic;
using UnityEngine;

namespace CSE5912
{
    public class GameConstants
    {
        public static GameConstants Instance { get; private set; }
        private static List<GameConstants> difficultyLevels = new List<GameConstants>(3);
        static GameConstants()
        {
            // Todo: This should really be data-driven from a text file or something.
            // Note: Only to override those that change.
            //Easy
            GameConstants easy = new GameConstants();
            difficultyLevels.Add(easy);
            easy.GasDamageAmount = 10;
            easy.GasDamageDuration = 2;
            GameConstants medium = new GameConstants();
            difficultyLevels.Add(medium);
            GameConstants difficult = new GameConstants();
            difficultyLevels.Add(difficult);
            difficult.GasDamageAmount = 50;
            difficult.GasDamageDuration = 1.5f;
            SetLevel(1);
        }
        private GameConstants()
        {
        }
        public static void SetLevel(int level)
        {
            // if level is within range. Change the instance to the corresponding level.
            if (level < 0 || level >= difficultyLevels.Count)
                return;
            Instance = difficultyLevels[level];
        }
        public float GasDamageAmount = 50; // health per second
        public float GasDamageDuration = 1; // seconds
        public float PlayerHealth = 100;
        public MonoBehaviour timingClass;
    }
}
