using MyTested.AspNetCore.Mvc;
using System;
using Volunteers.Controllers;
using Volunteers.Data.Models;
using Volunteers.Models.Projects;
using Xunit;

namespace Volunteers.Test
{
    public class CommentsControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
        {
            var user = new User { Id = "9cf1f6a2-2c94-42e2-b896-839faf685c33", NormalizedUserName = "admin" };
            var project = new Project { Id = "9cf1f6a2-2c94-42e2-b896-839faf685c25" };
            var comment = "sample comment";
            string id = "9cf1f6a2-2c94-42e2-b896-839faf685c25";
            
            MyController<CommentsController>.Instance(instance => instance.WithUser(user => user.WithUsername("admin")))
                .Calling(c => c.Add(comment, id))
                .ShouldReturn()
                .View(view => view.WithModelOfType<dynamic>());
        }
    }
}
