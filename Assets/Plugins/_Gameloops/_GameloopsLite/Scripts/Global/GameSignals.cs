namespace Gameloops
{
    public class GameSignals
    {
        public class LevelLoadedSignal{}
        public class LevelStartedSignal
        {
            public int Level;
        }

        public class LevelCompletedSignal
        {
            public int Level;
        }

        public class LevelFailedSignal
        {
            public int Level;
        }

        public class LevelLoadSameSignal{}
        public class LevelLoadNextSignal{}


        public class ProgressUpdatedSignal
        {
            public float Progress;
        }

    }
}