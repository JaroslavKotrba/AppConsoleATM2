using System;

namespace ATM2Console.Domain.Entities
{
	public class InternalTransfer
	{
		public decimal TransferAmount { get; set; } // type prop + TAB + TAB
		public long RecipientBankAccountNumber { get; set; }
		public string RecipientBankAccountName { get; set; }
	}
}

