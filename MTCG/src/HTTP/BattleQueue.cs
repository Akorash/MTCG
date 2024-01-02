using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.HTTP
{
    public class BattleQueue
    {
        private readonly int _matchFound = 1;
        private readonly int _userWaiting = 0;
        private readonly int _maxInQueue = 2;
        private ConcurrentQueue<int> _battleQueue; // https://dotnetpattern.com/csharp-concurrentqueue
        public bool MatchFound { get; private set; }
        public BattleQueue() 
        {
            _battleQueue = new();
            MatchFound = default(bool);
        }

        public int[] GetMatch()
        {
            if (!(MatchFound && (_battleQueue.Count == 2))) {
            }
            return _battleQueue.ToArray();
        }
        public void addToQueue(int id)
        {
            if (!_battleQueue.IsEmpty)
            {
                if (userAlreadyQueued(id)) {
                    throw new DuplicateWaitObjectException();
                }
                MatchFound = true;
            }
            _battleQueue.Enqueue(id);
        } 
        private bool userAlreadyQueued(int id)
        {
            int[] otherId = _battleQueue.ToArray();
            // https://stackoverflow.com/questions/3566413/ternary-operators-and-return-in-c
            return otherId[0] == id ? true : false;
        }
    }
}
