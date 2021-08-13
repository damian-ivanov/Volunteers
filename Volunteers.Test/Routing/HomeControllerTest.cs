using MyTested.AspNetCore.Mvc;
using Volunteers.Controllers;
using Volunteers.Models.Projects;
using Xunit;

namespace Volunteers.Test.Routing
{
    public class HomeControllerTest
    {

        [Fact]
        public void IndexRouteShouldBeMapped()
            => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithMethod(HttpMethod.Post)
                   .WithLocation("/")
                   .WithFormFields(new AllProjectsQueryModel
                   {
               
                   }))
               .To<HomeController>(c => c.Index(new AllProjectsQueryModel
               {}))
               .AndAlso()
               .ToValidModelState();

        [Fact]
        public void ErrorRouteShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Home/Error")
               .To<HomeController>(c => c.Error())
               .AndAlso()
               .ToValidModelState();

        [Fact]
        public void PrivacyRouteShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Home/Privacy")
               .To<HomeController>(c => c.Privacy())
               .AndAlso()
               .ToValidModelState();


    }
}
