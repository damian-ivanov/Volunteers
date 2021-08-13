using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volunteers.Controllers;
using Volunteers.Models.Projects;
using Volunteers.Services.Projects;
using Volunteers.Services.Projects.Models;
using Xunit;

namespace Volunteers.Test.Routing
{
    public class ProjectsControllerTest
    {
        [Fact]
        public void IndexRouteShouldBeMapped()
            => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithMethod(HttpMethod.Post)
                   .WithLocation("/Projects?sortOrder=Newest")
                   .WithFormFields("Newest"))
               .To<ProjectsController>(c => c.Index("Newest"))
               .AndAlso()
               .ToValidModelState();

        [Fact]
        public void CreateRouteShouldBeMapped()
            => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithMethod(HttpMethod.Get)
                   .WithLocation("/Projects/Create"))
               .To<ProjectsController>(c => c.Create())
               .AndAlso()
               .ToValidModelState();

        [Fact]
        public void ConfirmationRouteShouldBeMapped()
            => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithMethod(HttpMethod.Get)
                   .WithLocation("/Projects/Confirmation"))
               .To<ProjectsController>(c => c.Confirmation())
               .AndAlso()
               .ToValidModelState();

        [Fact]
        public void DetailsRouteShouldBeMapped()
           => MyRouting
              .Configuration()
              .ShouldMap(request => request
                  .WithMethod(HttpMethod.Get)
                  .WithLocation("/Projects/Details/ProjectId"))
              .To<ProjectsController>(c => c.Details("ProjectId"))
              .AndAlso()
              .ToValidModelState();

         [Fact]
        public void EditRouteShouldBeMapped()
           => MyRouting
              .Configuration()
              .ShouldMap(request => request
              .WithUser("admin", new[] { "Administrator" })
              .WithMethod(HttpMethod.Get)
              .WithLocation("/Projects/Edit/ProjectId"))
              .To<ProjectsController>(c => c.Edit("ProjectId"))
              .AndAlso()
              .ToValidModelState();

        [Fact]
        public void CompleteRouteShouldBeMapped()
           => MyRouting
              .Configuration()
              .ShouldMap(request => request
              .WithUser("admin", new[] { "Administrator" })
              .WithMethod(HttpMethod.Get)
              .WithLocation("/Projects/Complete/ProjectId"))
              .To<ProjectsController>(c => c.Complete("ProjectId"))
              .AndAlso()
              .ToValidModelState();


        [Fact]
        public void PendingApprovalRouteShouldBeMapped()
           => MyRouting
              .Configuration()
              .ShouldMap(request => request
                  .WithMethod(HttpMethod.Get)
                  .WithLocation("/Projects/PendingApproval"))
              .To<ProjectsController>(c => c.PendingApproval())
              .AndAlso()
              .ToValidModelState();
    }
}
