﻿using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class SearchModel
    {
        public User SubdomainUser { get; set; }
        public IEnumerable<User> FoundUsers { get; set; }
        public IEnumerable<PostViewModel> FoundPosts { get; set; }
    }
}