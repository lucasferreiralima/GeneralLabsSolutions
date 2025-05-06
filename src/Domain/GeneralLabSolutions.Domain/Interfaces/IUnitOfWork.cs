﻿namespace GeneralLabSolutions.Domain.Interfaces
{
	public interface IUnitOfWork
	{
		Task<bool> CommitAsync();
	}
}
