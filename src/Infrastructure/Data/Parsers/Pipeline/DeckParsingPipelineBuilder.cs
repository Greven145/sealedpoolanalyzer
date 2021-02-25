using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.Parsers.Pipeline {
    public class DeckParsingPipelineBuilder<T> {
        private readonly List<Func<IDeckParser<T>>> _filters = new();

        public DeckParsingPipelineBuilder<T> Register(Func<IDeckParser<T>> filter) {
            _filters.Add(filter);
            return this;
        }

        public DeckParsingPipelineBuilder<T> Register(IDeckParser<T> filter) {
            _filters.Add(() => filter);
            return this;
        }

        public IDeckParser<T> Build() {
            var root = _filters.First().Invoke();

            foreach (var filter in _filters.Skip(1)) {
                root.Register(filter.Invoke());
            }

            return root;
        }
    }
}
