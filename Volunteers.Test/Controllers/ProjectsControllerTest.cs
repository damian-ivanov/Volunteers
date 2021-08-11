using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Volunteers.Controllers;
using Volunteers.Data.Models;
using Volunteers.Models.Projects;
using Volunteers.Services.Projects.Models;
using Xunit;

namespace Volunteers.Test.Controllers
{
    public class ProjectsControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
        {
            var query = "Newest";

            MyController<ProjectsController>.Instance()
                .Calling(c => c.Index(query))
                .ShouldReturn()
                .View(view => view.WithModelOfType<List<ProjectListingServiceModel>>());
        }

        [Fact]
        public void CreateShouldReturnCorrectViewWithModel()
        {
            MyController<ProjectsController>.Instance()
                .Calling(c => c.Create())
                .ShouldReturn()
                .View(view => view.WithModelOfType<AddProjectFormModel>());
        }

        [Fact]
        public void ConfirmationShouldReturnCorrectViewWithModel()
        {
            MyController<ProjectsController>.Instance()
                .Calling(c => c.Confirmation())
                .ShouldReturn()
                .View();
        }


        [Fact]
        public void EditShouldRedirectToProjectIndexPageIfTheUserIsNotTheOwnerOrAdmin()
        {
            var project = new AddProjectFormModel { Address = "Sample address", CategoryId = 1, City = "Sample city", Description = "Description", Image = "sampleImage", StartDate = new DateTime(), Title = "Sample title" };

            MyController<ProjectsController>.Instance(instance => instance.WithUser(user => user.WithUsername("admin")))
                .Calling(c => c.Edit("sampleId"))
                .ShouldReturn()
                .RedirectToAction("Index", "Projects");
        }

        [Fact]
        public void EditShouldRedirectToProjectIndexPageIfTheUserIsNotAuthenticated()
        {
            MyController<ProjectsController>.Instance()
                .Calling(c => c.Edit("sampleId"))
                .ShouldReturn()
                .RedirectToAction("Index", "Projects");
        }

        [Fact]
        public void EditShouldRedirectToErrorPageIfTheProjectIsNull()
        {
            MyController<ProjectsController>.Instance(instance => instance.WithUser(user => user.InRole("Administrator")))
                .Calling(c => c.Edit("sampleId"))
                .ShouldReturn()
                .RedirectToAction("Error", "Home");
        }

        [Fact]
        public void ApproveShouldReturnCorrectView()
        {
            var project = new Project { Address = "Sample address", CategoryId = 1, City = "Sample city", Description = "Description", Image = "sampleImage", StartDate = new DateTime(), Title = "Sample title", Id = "9cf1f6a2-2c94-42e2-b896-839faf685c25", IsPublic = true, OwnerId = "myUserId", PublishedOn = new DateTime() };
            var user = new User { Id = "myUserId", UserName = "admin" };
            var badge = new Badge { Description = "Sample badge", Id = 1, Title = "First project", Users = new List<User>() };
            string id = "9cf1f6a2-2c94-42e2-b896-839faf685c25";

            MyController<ProjectsController>.Instance(instance => instance.WithUser("admin", new[] { "Administrator" }).WithData(project, user, badge))
                .Calling(c => c.Approve(id))
                .ShouldReturn()
                .RedirectToAction("Admin", "Projects");
        }

        //[Fact]
        //public void EditShouldReturnCorrectViewIfUserIsOwnerOrAdminAnTheProjectIsValid()
        //{
        //    var project = new Project { Address = "Sample address", CategoryId = 1, City = "Sample city", Description = "Description", Image = "sampleImage", StartDate = new DateTime(), Title = "Sample title", Id = "9cf1f6a2-2c94-42e2-b896-839faf685c25", IsPublic = true, OwnerId = "myUserId", PublishedOn = new DateTime()};
        //    string id = "9cf1f6a2-2c94-42e2-b896-839faf685c25";

        //    MyController<ProjectsController>.Instance(instance => instance.WithUser(user => user.InRole("Administrator")).WithData(project))
        //        .Calling(c => c.Edit(id))
        //        .ShouldReturn()
        //        .View(view => view.WithModelOfType<EditProjectViewModel>());
        //}



        //[Fact]
        //public void DetailsShouldReturnCorrectViewWithModel()
        //{
        //    string id = "sampleId";
        //    var user = new User { Id = "myUserId", UserName = "admin" };
        //    var project = new Project { Address = "Sample address", CategoryId = 1, City = "Sample city", Description = "Description", Image = "sampleImage", StartDate = new DateTime(), Title = "Sample title", Id="sampleId", IsPublic=true, OwnerId= "myUserId" };


        //    MyController<ProjectsController>.Instance(instance => instance.WithUser("admin").WithData(project))
        //        .Calling(c => c.Details(id))
        //        .ShouldReturn()
        //        .View(view => view.WithModelOfType<dynamic>());
        //}

        //[Fact]
        //public void CreatePostActionShouldReturnCorrectViewWithModel()
        //{
        //    var project = new AddProjectFormModel { Address = "Sample address", CategoryId = 1, City = "Sample city", Description = "Description", Image = "sampleImage", StartDate = new DateTime(), Title = "Sample title" };
        //    iformfile image = new mock

        //    MyController<ProjectsController>.Instance(instance => instance.WithUser(user => user.WithIdentifier("9cf1f6a2-2c94-42e2-b896-839faf685c33")))
        //        .Calling(c => c.Create(project, ))
        //        .ShouldReturn()
        //        .View(view => view.WithModelOfType<AddProjectFormModel>());
        //}


    }
}
