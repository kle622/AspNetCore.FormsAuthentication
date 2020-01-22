using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.FormsAuthentication.Tests
{
	static internal class FormsAuthenticationTicketExtension
	{
		public static bool IsValid(this FormsAuthenticationTicket ticket)
		{
			return ticket.IssueDate <= DateTime.Now && !ticket.Expired;
		}
	}
}
