using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Data.Parsers.Pipeline {
    public class DeckParsingPipeline<T> {
        private IDeckParser<T> _root;

        public DeckParsingPipeline<T> Register(IDeckParser<T> filter) {
            if (_root == null) {
                _root = filter;
            }
            else {
                _root.Register(filter);
            }

            return this;
        }

        public ValueTask<IEnumerable<CardFromFile>> Execute(T context) {
            return _root.Execute(context);
        }
    }
}
