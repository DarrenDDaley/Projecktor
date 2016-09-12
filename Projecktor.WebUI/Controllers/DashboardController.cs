﻿using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;

using Projecktor.WebUI.Models;
using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Controllers
{
    public class DashboardController : ProjecktorControllerBase
    {
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["login"];

            if (Security.IsAuthenticated == false && cookie == null) {
                return RedirectToAction("Index", "Home");
            }

            if(Security.IsAuthenticated == false && cookie != null) {
                Security.Login(cookie.Value);
            }

            IEnumerable<PostViewModel> timeline = Posts.GetTimeLineFor(Security.UserId).Take(10).ToArray();
            return View("Dashboard", timeline);
        }

        public ActionResult Likes()
        {
            if(Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(Security.UserId).Take(10).ToArray();
            return View("Likes", likeLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(string username)
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            Users.Follow(username, Security.GetCurrentUser());
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public JsonResult Reblog(int textId, int reblogId, int sourceId)
        {
            Post madeReblog = Posts.Reblog(CurrentUser.Id, textId, reblogId, sourceId);
            Hashtag[] tags = Hashtags.GetHashTagsFor(sourceId).ToArray();

            if (tags != null) {
                Hashtags.Create(madeReblog.Id, Security.UserId ,tags);
            }

            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult DeleteReblog(int postId)
        {
            Posts.DeleteReblog(postId);
            Hashtags.Delete(postId);
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnFollow(string username)
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            Users.Unfollow(username, Security.GetCurrentUser());
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Profiles()
        {
            IEnumerable<User> users = Users.AllUsers();
            return View(users);
        }

        [HttpGet]
        public JsonResult GetPosts(int pageIndex, int pageSize)
        {
            List<int> postIds = new List<int>();
            var timeline = Posts.GetTimeLineFor(Security.UserId).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in timeline) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLikes(int pageIndex, int pageSize)
        {
            List<int> postIds = new List<int>();
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(Security.UserId).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in likeLine) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPost(List<int> data)
        {
            List<PostViewModel> models = new List<PostViewModel>();

            foreach (var number in data) {
                models.Add(Posts.GetPost(number));
            }

            return PartialView("_MultiPosts", models);
        }


        public ActionResult Followers()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<User> followers = Users.GetAllFor(Security.UserId).Followers;

            return View("Followers", new FollowViewModel() {
                FollowData = followers
            });
        }

        public ActionResult Following()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<User> followers = Users.GetAllFor(Security.UserId).Following;

            return View("Following", new FollowViewModel() {
                FollowData = followers
            });
        }
  
        [HttpGet]
        public ActionResult TextPost() {
            return View("TextPost", new CreateTextPostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TextPost(CreateTextPostViewModel model)
        {
            if(ModelState.IsValid == true)
            {
                Post made = Posts.CreateTextPost(Security.UserId, model.TextPost);

                if(model.Hashtags != null) {
                    Hashtags.Create(made.Id, Security.UserId, model.Hashtags);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public ActionResult ImagePost() {
            return View("ImagePost", new CreateImagePostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImagePost(CreateImagePostViewModel model)
        {
            string[] imagePath = new string[6];
            HttpPostedFileBase[] images = model.Images.ToArray();

            for (int i = 0; i < model.Images.Count(); i++)
            {
                string imageName = Path.GetFileName(images[i].FileName);
                string physicalPath = Server.MapPath("~/images/" + imageName);

                images[i].SaveAs(physicalPath);
                imagePath[i] = physicalPath;
            }

            Post made = Posts.CreateImagePost(Security.UserId, model.Text, imagePath);

            if(model.Hashtags != null) {
                Hashtags.Create(made.Id, Security.UserId, model.Hashtags);
            }

            return RedirectToAction("Index", "Dashboard");
        }


        [HttpPost]
        public JsonResult DeletePost(int postId)
        {
            Post post = Posts.Getby(postId);

            if(post.Image1 != null) {
                System.IO.File.Delete(post.Image1);
            }
            if (post.Image2 != null) {
                System.IO.File.Delete(post.Image2);
            }
            if (post.Image3 != null) {
                System.IO.File.Delete(post.Image3);
            }
            if (post.Image4 != null) {
                System.IO.File.Delete(post.Image4);
            }
            if (post.Image5 != null) {
                System.IO.File.Delete(post.Image5);
            }
            if (post.Image6 != null) {
                System.IO.File.Delete(post.Image6);
            }

            Posts.Delete(postId);
            Hashtags.Delete(postId);
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult Like(int postId, int sourceId)
        {
            if(ModelState.IsValid == true) {
                UserLikes.Like(Security.UserId, postId, sourceId);
            }

            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult Unlike(int postId)
        {
            if (ModelState.IsValid == true) {
                UserLikes.Unlike(CurrentUser.Likes.FirstOrDefault(u => u.PostId == postId));
            }

            return Json(new { msg = "Successful" });
        }

        [HttpGet]
        public ActionResult Settings()
        {
            User user = Users.GetBy(Security.UserId);
            SettingsViewModel settings = new SettingsViewModel()
            {
                Username = user.Username,
                Email = user.Email
            };

            return View("Settings", settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel settings)
        {

            if (Security.Authenticate(CurrentUser.Username, settings.CurrentPassword) == false)
            {
                ModelState.AddModelError("Username", "The password was incorrect");
                return View("Settings", settings);
            }

            Users.Settings(settings.Username, settings.NewPassword, settings.Email, Security.UserId);
            return View("Settings", settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            string name = Request.Cookies["loggedIn"].Name;
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Domain = ".projecktor.com";
            Response.Cookies.Add(cookie);

            Security.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}