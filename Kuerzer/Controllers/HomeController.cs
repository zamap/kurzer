﻿using System;
using System.Web;
using System.Web.Mvc;
using KuerzerRepositories;
using KuerzerRepositories.Interfaces;


namespace Kuerzer.Controllers
{

	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
			
			//MvcApplication.AppId = Guid.NewGuid();
			
			//var g = HttpContext.Application["mykey"] as string;
			
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
