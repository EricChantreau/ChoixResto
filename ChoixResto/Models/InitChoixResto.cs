using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class InitChoixResto : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            context.Restaurants.Add(new Restaurant { Id = 1, Name = "Resto pinambour", Phone = "0102030405" });
            context.Restaurants.Add(new Restaurant { Id = 2, Name = "Resto pinière", Phone = "0102030405" });
            context.Restaurants.Add(new Restaurant { Id = 3, Name = "Resto toro", Phone = "0102030405" });

            base.Seed(context);
        }
    }
}