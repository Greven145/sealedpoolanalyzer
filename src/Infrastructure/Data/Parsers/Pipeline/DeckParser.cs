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

        ValueTask<IEnumerable<CardFromFile>> IDeckParser<TContext>.Execute(TContext context) {
            return Execute(context, ctx => _next?.Execute(ctx) ?? ValueTask.FromResult(Enumerable.Empty<CardFromFile>()));
        }

        protected abstract ValueTask<IEnumerable<CardFromFile>> Execute(TContext context, Func<TContext, ValueTask<IEnumerable<CardFromFile>>> next);
    }
}
