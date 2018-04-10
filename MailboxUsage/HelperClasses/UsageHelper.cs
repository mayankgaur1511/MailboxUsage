using MailboxUsage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MailboxUsage.HelperClasses
{
	public class UsageHelper
	{
		private MailboxUsageEntities2 db = new MailboxUsageEntities2();
		public List<Usage> GetListofMailbox(string UserID)
		{
			List<Usage> lstUsage = new List<Usage>();

			if (string.IsNullOrEmpty(UserID))
			{
				User user = GetUserByWindowsIdentity();
				UserID = user != null?  user.UserID : string.Empty;
			}
			if(string.IsNullOrEmpty(UserID))
			{
				return lstUsage;
			}
			var usages = (from user in db.Users
						  join usings in db.Usings on user.UserID equals usings.UserID
						  join mb in db.Mailboxes on usings.MailboxID equals mb.MailboxID
						  join status in db.UsageStatus on usings.StatusID equals status.StatusID
						  where user.UserID.Equals(UserID)
						  orderby usings.ID
						  select new
						  {
							  usings.ID,
							  mb.MailboxID,
							  mb.MailboxName,
							  mb.MailboxEmail,
							  status.StatusID,
							  status.StatusName,

						  }).ToList();

			foreach (var item in usages)
			{
				Usage usage = new Usage();
				usage.MboxID = item.ID;
				usage.MailboxName = item.MailboxName;
				usage.MailboxEmail = item.MailboxEmail;
				usage.MailBoxID = item.MailboxID;
				if (item.StatusID == 1)
				{
					usage.IUsed = true;
					usage.IDontUse = usage.IDontKnow = false;
				}
				else if (item.StatusID == 2)
				{
					usage.IDontUse = true;
					usage.IUsed = usage.IDontKnow = false;
				}
				else if (item.StatusID == 3)
				{
					usage.IDontKnow = true;
					usage.IUsed = usage.IDontUse = false;
				}
				lstUsage.Add(usage);
			}
			return lstUsage;
		}

		public string GetUserName(string UserID)
		{
			if (!string.IsNullOrEmpty(UserID))
			{
				var user = db.Users.Where(x => x.UserID == UserID);
				if (user != null && user.Count() > 0)
					return user.FirstOrDefault().FullName;
				else
					return string.Empty;
			}
			else
			{
				User user = GetUserByWindowsIdentity();
				if (user != null)
				{
					return user.FullName;
				}
				else
				{
					return string.Empty;
				}

			}
		}

		public User GetUserByWindowsIdentity()
		{
			WindowsIdentity identity = System.Web.HttpContext.Current.Request.LogonUserIdentity;
			User user = new User();
			string name = identity.Name;
			if (identity.Name.Contains(@"\"))
			{
				string[] userArr = new string[2];
				string[] delimiters = new string[] { @"\" };
				userArr = name.Split(delimiters, StringSplitOptions.None);
				if (userArr != null && userArr.Length == 2)
				{
					string userName = userArr[1].Insert(1, ".") + "@" + userArr[0] + ".com";
					var users = db.Users.Where(x => x.Email == userName);
					if (users != null)
					{
						user = users.FirstOrDefault();
					}
				}
			}
			return user;
		}

		public async Task<bool> SaveData(List<Usage> lstUsage)
		{

			int inUseID = db.UsageStatus.Where(m => m.StatusName.Equals("In use")).FirstOrDefault().StatusID;
			int notInUseID = db.UsageStatus.Where(m => m.StatusName.Equals("Not used")).FirstOrDefault().StatusID;
			int dontknowID = db.UsageStatus.Where(m => m.StatusName.Equals("Don't know")).FirstOrDefault().StatusID;
			foreach (var item in lstUsage)
			{
				var @using = await db.Usings.FindAsync(item.MboxID);
				if (item.IUsed)
				{
					@using.StatusID = inUseID;
				}
				else if (item.IDontUse)
				{
					@using.StatusID = notInUseID;
				}
				else if (item.IDontKnow)
				{
					@using.StatusID = dontknowID;
				}
				@using.LastUpdated = DateTime.Now;
				db.Entry(@using).State = EntityState.Modified;
				await db.SaveChangesAsync();
			}
			return true;
		}

		public async Task<List<User>> GetListOfUsers()
		{
			List<User> lstUser = new List<User>();
			lstUser = await db.Users.OrderBy(x => x.FullName).ToListAsync();
			return lstUser;
		}
	}
}