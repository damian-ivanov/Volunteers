using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Volunteers.Models.Projects;
using Volunteers.Services.Projects.Models;

namespace Volunteers.Services.Projects

{
    public interface IProjectService
    {
        IEnumerable<ProjectListingServiceModel> ListProjects(string sortOrder);

        ProjectDetailsServiceModel Details(string id, string userId);

        EditProjectViewModel Edit(string id);

        void Create(AddProjectFormModel project, string secureImageName, string ownerId);

        void Edit(EditProjectViewModel project, string secureImageName);

        bool Approve(string id);

        void Complete(CompleteProjectFormModel project, string secureImageName);

        void Activate(string id);

        void Hide(string id);

        void Delete(string id);

        string AddImage(IFormFile image, string extension);

    }
}