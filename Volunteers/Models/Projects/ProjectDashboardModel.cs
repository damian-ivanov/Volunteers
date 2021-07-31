using System;
using System.Collections.Generic;
using Volunteers.Data.Models;

namespace Volunteers.Models.Projects
{
    public class ProjectDashboardModel
    {
        
        public int ParticipantsCount { get; set; }

        public int ProjectsCount { get; set; }

        public int TownsCount { get; set; }
    }
}
