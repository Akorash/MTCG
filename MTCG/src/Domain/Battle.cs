using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.Domain.Entities;

namespace MTCG.src.Domain
{
    internal class Battle
    {
        private User? _player1 = null;
        private User? _player2 = null;
        public Battle() { }

        private void StartBattle(User p1, User p2)
        {
            if (_player1 == null || _player2 == null)
            {
                // Error
                return;
            }
            else
            {
                // Battle 
            }
        }
    }
}
