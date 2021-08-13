using MyTested.AspNetCore.Mvc;
using Volunteers.Controllers;
using Xunit;

namespace Volunteers.Test.Routing
{
    public class UsersControllerTest
    {
        [Fact]
        public void CorrectUserProfileShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Users/Profile/admin")
               .To<UsersController>(c => c.Profile("admin"))
               .AndAlso()
               .ToValidModelState();

    }
}
