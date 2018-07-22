using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChoixResto.Models
{
    public class Dal : IDal
    {
        private DatabaseContext db;

        public Dal()
        {
            db = new DatabaseContext();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public List<Restaurant> GetRestaurants()
        {
            return db.Restaurants.ToList();
        }

        public void CreateRestaurant(string name, string phone)
        {
            db.Restaurants.Add(new Restaurant { Name = name, Phone = phone });
            db.SaveChanges();
        }

        public void UpdateRestaurant(int id, string name, string phone)
        {
            Restaurant restaurant = db.Restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                return;
            }
            restaurant.Name = name;
            restaurant.Phone = phone;
            db.SaveChanges();
        }

        public bool IsRestaurant(string name)
        {
            return db.Restaurants.Any(r => string.Compare(r.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public User GetUser(int id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string id)
        {
            if (!int.TryParse(id, out int userId))
            {
                return null;
            }
            return GetUser(userId);
        }

        public int AddUser(string firstname, string password)
        {
            User user = new User { FirstName = firstname, Password = EncodeMD5(password) };
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public User Authenticate(string firstname, string password)
        {
            string encodedPassword = EncodeMD5(password);
            return db.Users.FirstOrDefault(user => user.FirstName == firstname && user.Password == encodedPassword);
        }

        public int CreateSurvey()
        {
            Survey survey = new Survey { Date = DateTime.Now, Votes = new List<Vote>() };
            db.Surveys.Add(survey);
            db.SaveChanges();
            return survey.Id;
        }

        public bool HasVoted(int surveyId, string userId)
        {
            Survey survey = db.Surveys.FirstOrDefault(s => s.Id == surveyId);
            if (survey == null)
            {
                return false;
            }
            if (!int.TryParse(userId, out int id))
            {
                return false;
            }
            return survey.Votes.Any(vote => vote.User.Id == id);
        }

        public void AddVote(int surveyId, int restaurantId, int userId)
        {
            Survey survey = db.Surveys.FirstOrDefault(s => s.Id == surveyId);
            Restaurant restaurant = db.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (survey == null || restaurant == null || user == null)
            {
                return;
            }
            survey.Votes.Add(new Vote { Restaurant = restaurant, User = user });
            db.SaveChanges();
        }

        public List<Results> GetResults(int surveyId)
        {
            Survey survey = db.Surveys.FirstOrDefault(s => s.Id == surveyId);
            if (survey == null)
            {
                return new List<Results>();
            }
            List<Results> results = new List<Results>();
            foreach (IGrouping<int, Vote> group in survey.Votes.GroupBy(vote => vote.Restaurant.Id))
            {
                Restaurant restaurant = db.Restaurants.First(r => r.Id == group.Key);
                results.Add(new Results { Name = restaurant.Name, Phone = restaurant.Phone, NumberOfVotes = group.Count() });
            }
            return results;
        }

        private string EncodeMD5(string password)
        {
            string saltedPassword = "ChoixResto" + password + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(saltedPassword)));
        }
    }
}