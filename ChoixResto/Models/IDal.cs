using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoixResto.Models
{
    public interface IDal : IDisposable
    {
        List<Restaurant> GetRestaurants();
        void CreateRestaurant(string name, string phone);
        void UpdateRestaurant(int id, string name, string phone);
        bool IsRestaurant(string name);

        User GetUser(int id);
        User GetUser(string id);
        int AddUser(string firstname, string password);
        User Authenticate(string firstname, string password);

        int CreateSurvey();
        bool HasVoted(int surveyId, string userId);
        void AddVote(int surveyId, int restaurantId, int userId);

        List<Results> GetResults(int surveyId);
    }
}
