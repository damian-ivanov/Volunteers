namespace Volunteers.Data
{
    public class DataConstants
    {
        public const int ProjectTitleMinLength = 5;
        public const int ProjectTitleMaxLength = 100;
        public const int ProjectDescriptionMinLength = 5;
        public const int ProjectDescriptionMaxLength = 1000;

        public const int BadgesTitleMaxLength = 50;
        public const int BadgesDescriptionMaxLength = 200;

        public const int CommentMinLength = 3;
        public const int CommentMaxLength = 1000;

        //Data seed settings/
        //Administrator account
        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "admin@admin.com";
        public const string AdministratorUsername = "admin";
        public const string AdministratorPassword = "111111";

        //Badges
        public const string FirstBadgeTitle = "First project";
        public const string FirstBadgeDesctiption = "This badge is awarded for your first submitted and approved project";
        public const string FirstBadgeImage = "1.png";

        public const string SecondBadgeTitle = "Enthusiast";
        public const string SecondBadgeDesctiption = "Awarded when you have 3 or more approved projects. Keep up the good work!";
        public const string SecondBadgeImage = "2.png";

        public const string ThirdBadgeTitle = "Advanced user";
        public const string ThirdBadgeDesctiption = "For users with 5 or more approved projects. Way to go!";
        public const string ThirdBadgeImage = "3.png";

        public const string ForthBadgeTitle = "Master improver";
        public const string ForthBadgeDesctiption = "Awarded to heavy improvers only - with 10+ projects.";
        public const string ForthBadgeImage = "4.png";

    }
}