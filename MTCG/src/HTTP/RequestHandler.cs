using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.Repositories;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.Domain;
using MTCG.src.Domain.Entities;
using System.Net;

namespace MTCG.src.HTTP
{
    public class RequestHandler
    {
        public PostFunctions Post { get; set; }
        public GetFunctions Get { get; set; }
        public RequestHandler()
        {
            Post = new PostFunctions();
            Get = new GetFunctions();
        }
    }

    public class PostFunctions
    {
        public PostFunctions() { }
        public string Register(string body)
        {
            // Create User Domain Entity
            var user = new User(null, GetUsername(body), GetPassword(body));

            try
            {
                user.Register();
                return HttpStatusCode.Created.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return HttpStatusCode.Conflict.ToString();
            }
        }
        public void LogIn(string body)
        {
            // Deserialize object
            var user = new User();
      
            try
            {
                user.LogIn();
            }
            catch (Exception e)
            {

            }
        }

        public string NewPackage()
        {
            // Create new card package from an array of cards, only admin
            // 201 Package and cards successfully created
            // 401 $ref: '#/components/responses/UnauthorizedError'
            // 403 Provided user is not "admin"
            // 409 At least one card in the packages already exists
            return "";
        }

        public string BuyPackage(string body)
        {
            // Aquire a card package with user money
            var user = new User(null, GetUsername(body), GetPassword(body));
            // 200 A package has been successfully bought
            // 401 $ref: '#/components/responses/UnauthorizedError'
            // 403 Not enough money for buying a card package
            // 404 No card package available for buying
            return "";
        }
        public string postBattles()
        {
            return "";
        }

        public string postTradings()
        {
            return "";
        }

        public string postTradingsWithId()
        {
            return "";
        }
        public string RetrieveUserData()
        {
            return "";
        }

        public string ShowCards()
        {
            var user = new User();
            return "";
        }
        public string ShowDeck()
        {
            var user = new User();
            return "";
        }
        public string RetrieveStats()
        {
            return "";
        }
        public string RetrieveScoreBoard()
        {
            return "";
        }
        private string GetUsername(string body)
        {
            // TODO: Parse
            return "test1";
        }

        private string GetPassword(string body)
        {
            // TODO: Parse
            return "password1";
        }

    }

    public class GetFunctions
    { 
        public GetFunctions() { }

        public string RetrieveUserData()
        {
            // _user.RetrieveData()
            // Retrieve user data (string username) --> only admin or matching user
            // 200 Data successfully retrieved$ref: '#/components/schemas/UserData',
            // 401 $ref: '#/components/responses/UnauthorizedError'
            // 404 User not found
            // sercurity mtcAuth
            return "";
        }
        public string ShowCards()
        {
            var user = new User();
            List<Card> cards = user.ShowCards();
            // _user.ShowCards()
            // Show users cards
            // 200 The user has cards, the response contains these
            // 204 The request was fine, but the user doesn't have any cards
            // 401 #/components/responses/UnauthorizedError
            return "";
        }
        public string ShowDeck()
        {
            var user = new User();
            return "";
        }
        public string RetrieveStats()
        {
            // _user.RetrieveStats()
            // Retrieves the stats for an individual user
            // 200
            // '401':
            // $ref: '#/components/responses/UnauthorizedError'
            return "";
        }
        public string RetrieveScoreBoard()
        {
            // _user.RetrieveScoreboard
            // Retrieves the user scoreboard ordered by the user's ELO.
            // 200, 401
            // ... 
            return "";
        }
        public string ShowTrades()
        {
            return "";
        }
        
        private string GetUsername(string body)
        {
            // TODO: Parse
            return "test1";
        }
        private string GetPassword(string body)
        {
            // TODO: Parse
            return "password1";
        }
    }
}
