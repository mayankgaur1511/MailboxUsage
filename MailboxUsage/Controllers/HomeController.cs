using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailboxUsage.Models;
namespace MailboxUsage.Controllers
{
	public class HomeController : Controller
	{
		private MailboxUsageModelContainer _mailboxes = new MailboxUsageModelContainer();
		public ActionResult Index()
		{
			List<Usage> lstUsage = new List<Usage>();
			if (Request.QueryString["UUID"] != null)
			{
				string UUID = Request.QueryString["UUID"].ToString();
				ViewBag.UserName = _mailboxes.Users.Where(x => x.UUID == UUID).FirstOrDefault().FullName;

				var usages = (from mb in _mailboxes.Mailboxes
							  join sh in _mailboxes.SharedMailboxes on mb.UMID equals sh.UMID
							  join va in _mailboxes.Values on mb.ValueID equals va.ValueID
							  where mb.UUID.Equals(UUID)
							  orderby sh.Id
							  select new
							  {
								  mb.Id,
								  sh.MailboxName,
								  mb.ValueID,
								  mb.UMID
							  }).ToList();

				foreach (var item in usages)
				{
					Usage usage = new Usage();
					usage.MboxID = item.Id;
					usage.MailboxName = item.MailboxName;
					usage.UMID = item.UMID;
					if(item.ValueID == "1")
					{
						usage.IUsed = true;
						usage.IDontUse = usage.IDontKnow = false;
					}
					else if (item.ValueID == "2")
					{
						usage.IDontUse = true;
						usage.IUsed = usage.IDontKnow = false;
					}
					else if (item.ValueID == "3")
					{
						usage.IDontKnow = true;
						usage.IUsed = usage.IDontUse = false;
					}
					//usage.ValueID = item.ValueID;
					lstUsage.Add(usage);
				}
			}



			ViewBag.Title = "Mailbox Usgae Survey";

			return View(lstUsage);
		}

		[HttpPost]
		public ActionResult Savedata(List<Usage> lstUsage, FormCollection form)
		{
			try
			{
				foreach (var item in lstUsage)
				{
					Mailbox mBox = _mailboxes.Mailboxes.Find(item.MboxID);
					if (item.IUsed)
					{
						mBox.ValueID = "1";
					}
					else if (item.IDontUse)
					{
						mBox.ValueID = "2";
					}
					else if (item.IDontKnow)
					{
						mBox.ValueID = "3";
					}
					mBox.LastUpdated = DateTime.Now;

				}
				_mailboxes.SaveChanges();
				ViewBag.Messages = "Thanks for submitting data.Your data has been successfully saved.";
			}
			catch (Exception)
			{
				ViewBag.Messages = "Some error occurred. Please try again.";
			}
			
			return View("Result");
		}
	}

}
