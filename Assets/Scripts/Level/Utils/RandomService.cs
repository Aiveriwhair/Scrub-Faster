using System;


    public static class RandomService
    {
        static Random _rnd;
        public static int Seed { get; private set; }

        static RandomService()
        {
            _rnd = new Random();
            Seed = _rnd.Next(Int32.MinValue, Int32.MaxValue);
            
            _rnd = new Random(Seed);
        }

        public static void SetSeed(int seed)
        {
            Seed = seed;
            _rnd = new Random(Seed);
        }

        public static bool RollD100(int chance) => _rnd.Next(1, 101) <= chance;

        public static int GetRandom(int min, int max) => _rnd.Next(min, max);
    }