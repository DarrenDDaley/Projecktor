﻿using System;

namespace Projecktor.Domain.Abstract
{
    public interface IContext : IDisposable
    {
        IUserRepository Users { get; }
        ITextPostRepository TextPosts { get; }
        ILikeRepository Likes { get; }

        int SaveChanges();
    }
}
