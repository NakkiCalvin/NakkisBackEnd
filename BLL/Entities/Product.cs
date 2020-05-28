using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BLL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ImagePath { get;set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        //public string Color { get; set; }
        //public string Size { get; set; }
        //public int Quantity { get; set; }

        //public string Department { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IEnumerable<string> VariantSizes { get; set; }
        //public string Category { get; set; }

        //public ICollection<CartItem> CartItems { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<Variant> Variants { get; set; }
    }
}
