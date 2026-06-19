using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.Entities
{
    public sealed class Universe
    {
        public Guid Id { get; private set; }
        private readonly List<Body> _bodies = [];

        public IReadOnlyCollection<Body> Bodies
            => _bodies;

        public void AddBody(Body body)
        {
            _bodies.Add(body);
        }
    }
}
