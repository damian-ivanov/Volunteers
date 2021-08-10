using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Volunteers.Controllers;
using Volunteers.Models.Projects;
using Xunit;

namespace Volunteers.Test
{
    public class BadgesControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
        {
            MyController<BadgesController>.Instance()
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view.WithModelOfType<IEnumerable<Volunteers.Models.Badges.BadgesListingViewModel>>());
        }
    }
}
