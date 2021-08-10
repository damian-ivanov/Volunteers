using MyTested.AspNetCore.Mvc;
using System;
using Volunteers.Controllers;
using Volunteers.Models.Projects;
using Xunit;

namespace Volunteers.Test
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
        {
            var query = new AllProjectsQueryModel();
            MyController<HomeController>.Instance()
                .Calling(c => c.Index(query))
                .ShouldReturn()
                .View(view => view.WithModelOfType<AllProjectsQueryModel>());
        }

        [Fact]
        public void PrivacyShouldReturnView()
        {
            MyController<HomeController>.Instance()
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void ErrorShouldReturnView()
        {
            MyController<HomeController>.Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
        }
    }
}
