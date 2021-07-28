using System;
using System.Collections.Generic;
using Volunteers.Data.Models;

namespace Volunteers.Models.Comments
{
    public class CommentListingViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public string PublishedOn { get; set; }

        public string ProjectId { get; set; }
    }
}
