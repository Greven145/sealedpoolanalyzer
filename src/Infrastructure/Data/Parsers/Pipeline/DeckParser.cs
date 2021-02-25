using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Data.Parsers.Pipeline {
    public abstract class DeckParser<TContext> : IDeckParser<TContext> {
        private IDeckParser<TContext> _next;

        public void Register(IDeckParser<TContext> filter) {
            if (_next == null) {
                _next = filter;
            } else {
                _next.Register(filter);
            }
        }

        async ValueTask<IEnumerable<CardFromFile>> IDeckParser<TContext>.Execute(TContext context) {
            try {
                return await Execute(context, ctx => _next?.Execute(ctx) ?? ValueTask.FromResult(Enumerable.Empty<CardFromFile>()));
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        protected abstract ValueTask<IEnumerable<CardFromFile>> Execute(TContext context, Func<TContext, ValueTask<IEnumerable<CardFromFile>>> next);
    }
}
