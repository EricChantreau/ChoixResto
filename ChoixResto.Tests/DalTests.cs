using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ChoixResto.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChoixResto.Tests
{
    [TestClass]
    public class DalTests
    {
        private IDal dal;

        [TestInitialize]
        public void Init_BeforeEachTest()
        {
            IDatabaseInitializer<DatabaseContext> init = new DropCreateDatabaseAlways<DatabaseContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DatabaseContext());

            dal = new Dal();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            dal.Dispose();
        }

        [TestMethod]
        public void CreateRestaurant_WithNewRestaurant_GetRestaurantAndCheckRestaurantExists()
        {
            dal.CreateRestaurant("La bonne fourchette", "01 02 03 04 05");
            List<Restaurant> restaurants = dal.GetRestaurants();

            Assert.IsNotNull(restaurants);
            Assert.AreEqual(1, restaurants.Count);
            Assert.AreEqual("La bonne fourchette", restaurants[0].Name);
            Assert.AreEqual("01 02 03 04 05", restaurants[0].Phone);
        }

        [TestMethod]
        public void UpdateRestaurant_WithNewRestaurant_RestaurantUpdated()
        {
            dal.CreateRestaurant("La bonne fourchette", "01 02 03 04 05");
            dal.UpdateRestaurant(1, "La bonne cuillère", null);
            List<Restaurant> restaurants = dal.GetRestaurants();

            Assert.IsNotNull(restaurants);
            Assert.AreEqual(1, restaurants.Count);
            Assert.AreEqual("La bonne cuillère", restaurants[0].Name);
            Assert.IsNull(restaurants[0].Phone);
        }

        [TestMethod]
        public void IsRestaurant_WithNewRestaurant_RestaurantExists()
        {
            dal.CreateRestaurant("La bonne fourchette", "01 02 03 04 05");
            bool exists = dal.IsRestaurant("La bonne fourchette");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void IsRestaurant_WithNoRestaurant_RestaurantExists()
        {
            bool exists = dal.IsRestaurant("La bonne fourchette");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void GetUser_WithNoUser_ReturnsNull()
        {
            User user = dal.GetUser(1);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUser_WithNonNumeric_ReturnsNull()
        {
            User user = dal.GetUser("abc");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void AddUser_WithNewUser_GetUser()
        {
            dal.AddUser("New user", "12345");

            User user = dal.GetUser(1);

            Assert.IsNotNull(user);
            Assert.AreEqual("New user", user.FirstName);

            user = dal.GetUser("1");

            Assert.IsNotNull(user);
            Assert.AreEqual("New user", user.FirstName);
        }

        [TestMethod]
        public void Authenticate_LoginPasswordOk_AuthenticationOk()
        {
            dal.AddUser("New user", "12345");

            User user = dal.Authenticate("new user", "12345");

            Assert.IsNotNull(user);
            Assert.AreEqual("New user", user.FirstName);
        }

        [TestMethod]
        public void Authenticate_LoginPasswordKo_AuthenticationKo()
        {
            dal.AddUser("New user", "12345");

            User user = dal.Authenticate("new user", "0");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void HasVoted_UserHasNotVoted_ReturnsFalse()
        {
            int surveyId = dal.CreateSurvey();
            int userId = dal.AddUser("New user", "12345");

            bool hasVoted = dal.HasVoted(surveyId, userId.ToString());

            Assert.IsFalse(hasVoted);
        }

        [TestMethod]
        public void HasVoted_UserHasVoted_ReturnsTrue()
        {
            int surveyId = dal.CreateSurvey();
            int userId = dal.AddUser("New user", "12345");
            dal.CreateRestaurant("La bonne fourchette", "01 02 03 04 05");
            dal.AddVote(surveyId, 1, userId);

            bool hasVoted = dal.HasVoted(surveyId, userId.ToString());

            Assert.IsTrue(hasVoted);
        }

        [TestMethod]
        public void GetResults_WithSomeChoices_ReturnsResults()
        {
            int surveyId = dal.CreateSurvey();
            int userId1 = dal.AddUser("New user 1", "12345");
            int userId2 = dal.AddUser("New user 2", "12345");
            int userId3 = dal.AddUser("New user 3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(surveyId, 1, userId1);
            dal.AddVote(surveyId, 3, userId1);
            dal.AddVote(surveyId, 4, userId1);
            dal.AddVote(surveyId, 1, userId2);
            dal.AddVote(surveyId, 1, userId3);
            dal.AddVote(surveyId, 3, userId3);

            List<Results> results = dal.GetResults(surveyId);

            Assert.AreEqual(3, results[0].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results[0].Name);
            Assert.AreEqual("0102030405", results[0].Phone);
            Assert.AreEqual(2, results[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results[1].Name);
            Assert.AreEqual("0102030405", results[1].Phone);
            Assert.AreEqual(1, results[2].NumberOfVotes);
            Assert.AreEqual("Resto ride", results[2].Name);
            Assert.AreEqual("0102030405", results[2].Phone);
        }

        [TestMethod]
        public void GetResults_WithTwoSurveys_ReturnsResults()
        {
            int surveyId1 = dal.CreateSurvey();
            int userId1 = dal.AddUser("New user 1", "12345");
            int userId2 = dal.AddUser("New user 2", "12345");
            int userId3 = dal.AddUser("New user 3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(surveyId1, 1, userId1);
            dal.AddVote(surveyId1, 3, userId1);
            dal.AddVote(surveyId1, 4, userId1);
            dal.AddVote(surveyId1, 1, userId2);
            dal.AddVote(surveyId1, 1, userId3);
            dal.AddVote(surveyId1, 3, userId3);

            int surveyId2 = dal.CreateSurvey();
            dal.AddVote(surveyId2, 2, userId1);
            dal.AddVote(surveyId2, 3, userId1);
            dal.AddVote(surveyId2, 1, userId2);
            dal.AddVote(surveyId2, 4, userId3);
            dal.AddVote(surveyId2, 3, userId3);

            List<Results> results1 = dal.GetResults(surveyId1);
            List<Results> results2 = dal.GetResults(surveyId2);

            Assert.AreEqual(3, results1[0].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results1[0].Name);
            Assert.AreEqual("0102030405", results1[0].Phone);
            Assert.AreEqual(2, results1[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results1[1].Name);
            Assert.AreEqual("0102030405", results1[1].Phone);
            Assert.AreEqual(1, results1[2].NumberOfVotes);
            Assert.AreEqual("Resto ride", results1[2].Name);
            Assert.AreEqual("0102030405", results1[2].Phone);

            Assert.AreEqual(1, results2[0].NumberOfVotes);
            Assert.AreEqual("Resto pinambour", results2[0].Name);
            Assert.AreEqual("0102030405", results2[0].Phone);
            Assert.AreEqual(2, results2[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results2[1].Name);
            Assert.AreEqual("0102030405", results2[1].Phone);
            Assert.AreEqual(1, results2[2].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results2[2].Name);
            Assert.AreEqual("0102030405", results2[2].Phone);
        }
    }
}
