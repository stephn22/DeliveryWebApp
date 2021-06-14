using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWebApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public virtual string Id { get; protected set; }
    }
}
