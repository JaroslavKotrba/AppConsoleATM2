using System;
using ATM2Console.Domain.Enums; // to load TransactionType

namespace ATM2Console.Domain.Entities
{
	public class Transaction
	{
		public long TransactionId { get; set; } // type prop + TAB + TAB
        public long UserBankAccountId { get; set; }
		public DateTime TransactionDate { get; set; }
		public TransactionType TransactionType { get; set; }
		public string Description { get; set; }
		public Decimal TransactionAmount { get; set; }
	}
}

