using MyTested.AspNetCore.Mvc;
using Volunteers.Controllers;
using Xunit;

namespace Volunteers.Test.Routing
{
    public class BadgesControllerTest
    {
        [Fact]
        public void AllBadgesRouteShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Badges")
               .To<BadgesController>(c => c.Index())
               .AndAlso()
               .ToValidModelState();
    }
}
