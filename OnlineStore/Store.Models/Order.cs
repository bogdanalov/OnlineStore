﻿namespace Store.Models
{
    using System.Collections.Generic;

    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }
    }
}
