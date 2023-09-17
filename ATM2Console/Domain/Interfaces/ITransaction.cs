using System;
using ATM2Console.Domain.Enums; // to load TransactionType

namespace ATM2Console.Domain.Interfaces
{
	public interface ITransaction
	{
		void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc);
		void ViewTransaction();
	}
}

