using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Contracts.IInfrastructure {
    public interface ISetLoader {
        ValueTask<Set> GetSetFromId(Guid id);
    }
}
