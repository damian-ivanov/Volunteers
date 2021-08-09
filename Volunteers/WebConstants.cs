namespace Volunteers
{
    public class WebConstants
    {
        //Temp data constants start
        public const string GlobalMessageKey = "GlobalMessage";
        public const string CreatedProject = "You project was created and is awaiting for approval!";
        public const string EditedProject = "You project was edited and is awaiting review from an administrator!";
        public const string EditedProjectAdmin = "Successful edit.";
        public const string ApprovedProject = "Project approved!";
        public const string CompletedProject = "Well done! Project is complete!";
        public const string ActivateProject = "Project is now active!";
        public const string HideProject = "Project is now hidden.";
        public const string DeleteProject = "Successfull deletion.";
        public const string JoinedProject = "You have joined! Way to go!";
        public const string LeftProject = "You left the project.";

        public const string PostedComment = "Comment submitted and pending review from a moderator.";
        public const string DeletedComment = "Comment deleted.";
        public const string ApprovedComment = "Comment approved.";

        public const string SetRoles = "Role updated.";
        public const string DeleteUser = "User was deleted.";
        //Temp data constants end

        public static readonly string[] AllowedImageExtensions = new string[] { ".jpg", ".jpeg", ".png" };
        public const string RequiredImage = "Invalid image type. Allowed images are of type .jpg, .jpeg or .png.";
    }
}