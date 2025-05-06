using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralLabSolutions.Domain.DomainObjects;
using GeneralLabSolutions.Domain.Interfaces;

namespace GeneralLabSolutions.InfraStructure.Repository.Base
{
    public interface IRepository<T> : IDisposable, IAsyncDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
