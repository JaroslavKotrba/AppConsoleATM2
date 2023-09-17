using System;

namespace ATM2Console.Domain.Interfaces
{
	public interface IUserAccountActions
	{
		void CheckBalance();
		void PlaceDeposit();
		void MakeWithDrawal();

	}
}

