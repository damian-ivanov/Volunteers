using MyTested.AspNetCore.Mvc;
using Volunteers.Controllers;
using Volunteers.Models.Users;
using Xunit;

namespace Volunteers.Test.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public void ProfileShouldReturnCorrectViewForValidUsername()
        {
            string id = "admin";

            MyController<UsersController>.Instance(instance => instance.WithUser(user => user.WithUsername("admin")))
                .Calling(c => c.Profile(id))
                .ShouldReturn()
                .RedirectToAction("Error", "Home");
        }

        [Fact]
        public void ProfileShouldReturnErrorPageWhenInvalidUsernameIsSelected()
        {
            string id = "admin";

            MyController<UsersController>.Instance()
                .Calling(c => c.Profile(id))
                .ShouldReturn()
                .RedirectToAction("Error", "Home");
        }
    }
}
