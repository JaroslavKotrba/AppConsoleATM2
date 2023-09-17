using System;

namespace ATM2Console.Domain.Enums
{
	public enum AppMenu
	{
		CheckBalance = 1, // it will start from 1 and not from 0
		PlaceDeposit,
		MakeWithdrawal,
		InternalTransfer,
		ViewTransaction,
		Logout
	}
}

