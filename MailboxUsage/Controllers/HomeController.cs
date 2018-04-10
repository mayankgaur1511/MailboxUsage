using MailboxUsage.HelperClasses;
using MailboxUsage.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace MailboxUsage.Controllers
{
    public class HomeController : Controller
    {

	public ActionResult Index()
	{
	    List<Usage> lstUsage = new List<Usage>();
	    UsageHelper helper = new UsageHelper();
	    string UUID = Request.QueryString["UUID"] != null ? Request.QueryString["UUID"].ToString() : string.Empty;
	    TempData["UserName"] = helper.GetUserName(UUID);
	    Session["UserID"] = UUID;
	    lstUsage = helper.GetListofMailbox(UUID);
	    ViewBag.Title = "Mailbox Usage Survey";
	    return View(lstUsage);
	}

	[HttpPost]
	public async Task<ActionResult> Savedata(List<Usage> lstUsage)
	{
	    UsageHelper helper = new UsageHelper();
	    try
	    {
		bool res = await helper.SaveData(lstUsage);
		if (res)
		{
		    ViewBag.Messages = "Thanks for submitting data.Your data has been successfully saved.";
		    return Json(new { redirectTo = Url.Action("Result", "Home") }, JsonRequestBehavior.AllowGet);
		}
		else
		{
		    ViewBag.Messages = "Not able to update the data now. Please try again after some time.";
		    return Json(new { redirectTo = Url.Action("Result", "Home") }, JsonRequestBehavior.AllowGet);
		}

	    }
	    catch (Exception ex)
	    {
		ViewBag.Messages = "Some error occurred. Please try again. " + ex.Message;
		return Json(new { redirectTo = Url.Action("Result", "Home") }, JsonRequestBehavior.AllowGet);
	    }
	}

	public async Task<ActionResult> ViewUserDetails()
	{
	    UsageHelper helper = new UsageHelper();
	    List<User> user = await helper.GetListOfUsers();
	    if (user != null && user.Count > 0)
	    {
		SelectList selectList = new SelectList(user, "UserID", "FullName");
		ViewBag.USERS = selectList;
	    }
	    ViewBag.Title = "User Details";
	    return View();
	}

	public ActionResult _DisplayUserDetails(string UserID)
	{
	    List<Usage> lstUsage = new List<Usage>();
	    UsageHelper helper = new UsageHelper();
	    lstUsage = helper.GetListofMailbox(UserID);
	    return PartialView(lstUsage);
	}

	public ActionResult Result()
	{
	    ViewBag.Title = "Thank You!";
	    return View();
	}

	public ActionResult GoBack()
	{
	    return RedirectToAction("Index", new { UUID = Session["UserID"] != null ? Session["UserID"].ToString() : string.Empty});
	}
    }

}
