using System;

namespace ATM2Console.Domain.Entities
{
	public class UserAccount
	{
		public int Id { get; set; } // type prop + TAB + TAB
        public long CardNumber { get; set; }
		public int CardPin { get; set; }
		public long AcountNumber { get; set; }
		public string FullName { get; set; }
		public decimal AccountBalance { get; set; }
		public int TotalLogin { get; set; }
		public bool IsLocked { get; set; }
	}	
}

