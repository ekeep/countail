using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GifGenerator.Data
{
    public abstract class IEntity
    {
        public long Id { get; set; }
    }
}