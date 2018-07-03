using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog_Admin.Models;

namespace MyBlog_Admin.Controllers
{
    public class HomeController : Controller
    {
        MyBlog_AdminContext db=new MyBlog_AdminContext();
        public ActionResult Index()
        {
            var article = db.Articles.ToList();
            return View(article.OrderByDescending(x => x.Date));
        }
        
        public ActionResult Login()
        {
            return View();
        }

       
        public ActionResult Control(string name, string password)
        {
            if (name == "muhammed" || password == "1234")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        public ActionResult NewArticle()
        {
            ViewBag.categories = db.Categories.ToList();
            ViewBag.tag = db.Tags.ToList();
            return View();
        }

        public ActionResult AddArticle(List<string> categories, List<string> tags,string title,string content)
        {
            List<Category> categorylist=new List<Category>();
            List<Tag> taglist=new List<Tag>();
            foreach (var item in categories)
            {
                Category cat = new Category {Name = item};
                categorylist.Add(cat);
            }
            foreach (var item in tags)
            {
                Tag tag = new Tag {Name = item};
                taglist.Add(tag);
            }

            Article article = new Article
            {
                Title = title,
                Categories = categorylist,
                Tags = taglist,
                Content_ = content,
                Date = DateTime.Now
            };
            db.Articles.Add(article);
            db.SaveChanges();

            return RedirectToAction("NewArticle", "Home");
        }

        public ActionResult UpdateArticle(List<string> categories, List<string> tags, string title, string content,int ArticleId)
        {
            List<Category> categorylist = new List<Category>();
            List<Tag> taglist = new List<Tag>();
            foreach (var item in categories)
            {
                Category cat = new Category { Name = item };
                categorylist.Add(cat);
            }
            foreach (var item in tags)
            {
                Tag tag = new Tag { Name = item };
                taglist.Add(tag);
            }

         


            Article a = db.Articles.First(x => x.ArticleId == ArticleId);
            a.Categories = categorylist;
            a.Tags = taglist;
            a.Date=DateTime.Now;
            a.Title = title;
            a.Content_ = content;
            db.SaveChanges();


            return RedirectToAction("SinglePost", "Home", new { id = ArticleId });
        }


        public ActionResult SelectedArticle(int Id)
        {
            ViewBag.categories = db.Categories.ToList();
            ViewBag.tag = db.Tags.ToList();
            var article = db.Articles.FirstOrDefault(x => x.ArticleId == Id);

            return View(article);
        }

        //***********************************************
        public ActionResult Category(int category_Id)
        {
            var query = from article in db.Articles
                where article.Categories.Any(c => c.CategoryId == category_Id)
                select article;
            var articles = query.ToList();
            return View(articles.OrderByDescending(x => x.Date));
        }

        public ActionResult Tag(int tag_Id)
        {
            var query = from article in db.Articles
                where article.Tags.Any(c => c.Tag_Id == tag_Id)
                select article;
            var articles = query.ToList();
            return View(articles.OrderByDescending(x => x.Date));

        }

        public ActionResult SinglePost(int Id)
        {
            var article = db.Articles.FirstOrDefault(x => x.ArticleId == Id);

            return View(article);
        }

        public ActionResult CategoryWidget()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }



        public ActionResult TagWidget()
        {
            var tags = db.Tags.ToList();
            return View(tags);
        }
        public ActionResult Search(string text)
        {
            var article = db.Articles.Where(c => c.Title.Contains(text));

            return View(article);
        }

        public ActionResult Fill_Categories()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        public ActionResult Archive()
        {
            var article = db.Articles.ToList();
            return View(article.OrderByDescending(x => x.Date));
        }
        public ActionResult AddComment(string Name, string Email, string Surname, string Message, int ArticleId)
        {
            Comment com = new Comment
            {
                ArticleId = ArticleId,
                Date = DateTime.Now,
                Name = Name,
                Surname = Surname,
                Content_ = Message
            };

            db.Comments.Add(com);
            db.SaveChanges();

            return RedirectToAction("SinglePost", "Home", new { id = ArticleId });
        }


    }
}