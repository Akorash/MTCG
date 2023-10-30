using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.Domain;

namespace MTCG.src.HTTP
{
    internal class DataHandler
    {
        private User _user;
        public DataHandler()
        {
            _user = new();
        }

        public string Login()
        {
            // user -> deserializeobject
            // user.login

            // Login with existing user with psw and uname
            // 200 User login successful
            // 401 Invalid username/password provided
            return "Login OK";
        }

        // _user.RetrieveData()
        // Retrieve user data (string username) --> only admin or matching user
        // 200 Data successfully retrieved$ref: '#/components/schemas/UserData',
        // 401 $ref: '#/components/responses/UnauthorizedError'
        // 404 User not found
        // sercurity mtcAuth

        // _user.ShowCards()
        // Show users cards
        // 200 The user has cards, the response contains these
        // 204 The request was fine, but the user doesn't have any cards
        // 401 #/components/responses/UnauthorizedError

        // _user.ShowDeck()
        // Shows the user's currently configured deck
        // query, plain json
        // 200 The deck has cards, the response contains these
        // array and string deck description
        // 204 deck !have cards
        // 401 $ref: '#/components/responses/UnauthorizedError'

        // _user.RetrieveStats()
        // Retrieves the stats for an individual user
        // 200
        // '401':
        // $ref: '#/components/responses/UnauthorizedError'

        // _user.RetrieveScoreboard
        // Retrieves the user scoreboard ordered by the user's ELO.
        // 200, 401
        // ... 
    }
}
