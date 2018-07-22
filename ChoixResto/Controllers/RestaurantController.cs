using ChoixResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController() : this(new Dal())
        {

        }

        public RestaurantController(IDal dal)
        {
            this.dal = dal;
        }

        public ActionResult Index()
        {
            List<Restaurant> restaurants = dal.GetRestaurants();
            return View(restaurants);
        }

        public ActionResult UpdateRestaurant(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }
            Restaurant restaurant = dal.GetRestaurants().FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                return View("Error");
            }
            return View(restaurant);
        }

        [HttpPost]
        public ActionResult UpdateRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return View(restaurant);
            }
            dal.UpdateRestaurant(restaurant.Id, restaurant.Name, restaurant.Phone);
            return RedirectToAction("Index");
        }

        public ActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRestaurant(Restaurant restaurant)
        {
            if (dal.IsRestaurant(restaurant.Name))
            {
                ModelState.AddModelError("Name", "This name already exists");
                return View(restaurant);
            }
            if (!ModelState.IsValid)
            {
                return View(restaurant);
            }
            dal.CreateRestaurant(restaurant.Name, restaurant.Phone);
            return RedirectToAction("Index");
        }
    }
}