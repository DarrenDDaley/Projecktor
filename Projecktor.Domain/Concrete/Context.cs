﻿using Projecktor.Domain.Abstract;
using System.Data.Entity;


namespace Projecktor.Domain.Concrete
{
    public class Context : IContext
    {
        private readonly DbContext db;


        public Context(DbContext context = null, IUserRepository users = null, 
                       IPostRepository posts = null, ILikeRepository likes = null, 
                       ITextRepository texts = null, IHashtagRepository hashtags = null, 
                       IFollowRepository follow = null, IPasswordResetRepository passwordReset = null)
        {
            db = context ?? new ProjecktorDatabase();
            Users = users ?? new UserRepository(db, true);
            Posts = posts ?? new PostRepository(db, true);
            Texts = texts ?? new TextRepository(db, true);
            Likes = likes ?? new LikeRepository(db, true);
            Hashtags = hashtags ?? new HashtagRepository(db, true);
            Follow = follow ?? new FollowRepository(db, true);
            PasswordReset = passwordReset ?? new PasswordResetRepository(db, true);
        }

        public IUserRepository Users
        {
            get;
            private set;
        }

        public IPostRepository Posts
        {
            get;
            private set;
        }

        public ITextRepository Texts
        {
            get;
            private set;
        }

        public ILikeRepository Likes
        {
            get;
            private set;
        }

        public IHashtagRepository Hashtags
        {
            get;
            private set;
        }

        public IFollowRepository Follow
        {
            get;
            private set;
        }

        public IPasswordResetRepository PasswordReset
        {
            get;
            private set;
        }

        public int SaveChanges() {
            return db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
            {
                try {
                    db.Dispose();
                }
                catch{ }
            }
        }
    }
}
