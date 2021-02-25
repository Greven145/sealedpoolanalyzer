using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Data.Parsers.Pipeline {
    public interface IDeckParser<TContext> {
        void Register(IDeckParser<TContext> filter);
        ValueTask<IEnumerable<CardFromFile>> Execute(TContext context);
    }
}
