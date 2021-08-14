using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using Volunteers.Controllers;
using Volunteers.Models.Projects;
using Xunit;

namespace Volunteers.Test.Pipeline
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
        {
            var query = new AllProjectsQueryModel();

            MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index(query))
                .Which(controller => controller
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllProjectsQueryModel>()));
                    
        }
    }
}
