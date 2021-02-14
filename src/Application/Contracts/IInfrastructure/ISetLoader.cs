using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Contracts.IInfrastructure {
    public interface ISetLoader {
        ValueTask<IEnumerable<MagicCard>> GetSetFromId(Guid id);
    }
}
