using System;
using System.Collections.Generic;
using Volunteers.Data.Models;

namespace Volunteers.Models.Badges
{
    public class BadgesListingViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int Users { get; set; }
    }
}
