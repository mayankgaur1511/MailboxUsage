using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailboxUsage.Models
{
	public class Usage
	{
		public string UMID { get; set; }

		public string MailboxName { get; set; }

		//public string ValueID { get; set; }

		public bool IUsed { get; set; }

		public bool IDontUse { get; set; }

		public bool IDontKnow { get; set; }

		public int MboxID { get; set; }
	}
}