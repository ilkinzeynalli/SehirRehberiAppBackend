using SehirRehberi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{
    public class Value : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
