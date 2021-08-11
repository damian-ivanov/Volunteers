using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volunteers.Areas.Admin.Models;
using Volunteers.Data.Models;
using Volunteers.Models.Projects;
using Volunteers.Services.Projects.Models;

namespace Volunteers.Services.Projects

{
    public interface IProjectService
    {
        IEnumerable<ProjectListingServiceModel> ListProjects(string sortOrder);

        IEnumerable<ProjectListingAdminViewModel> ListProjectsAdmin();

        IEnumerable<ProjectListingViewModel> ListProjectsHomePage(AllProjectsQueryModel query);

        ProjectDetailsServiceModel Details(string id, string userId);

        EditProjectViewModel Edit(string id);

        void Create(AddProjectFormModel project, string secureImageName, string ownerId);

        public Task<bool> Edit(EditProjectViewModel project, string secureImageName, string editorId);

        bool Approve(string id);

        void Complete(CompleteProjectFormModel project, string secureImageName);

        void Activate(string id);

        void Hide(string id);

        void Delete(string id);

        Project Join(string id, string userId);

        Project Leave(string id, string userId);

        string AddImage(IFormFile image, string extension);

        IEnumerable<ProjectCategoryViewModel> GetProjectCategories();

        bool IsValid(string id);

        bool EmptyCategoryCheck(AddProjectFormModel project);

        bool EmptyCategoryCheck(EditProjectViewModel project);

    }
}