using CentiroHomeAssignment.DataLayer;
using CentiroHomeAssignment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CentiroHomeAssignment.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public OrderDbContext _orderDbContext = new OrderDbContext();


        public int Id { get; set; }

        [Display(Name = "Order number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Order line number")]
        public int OrderLineNumber { get; set; }

        [Display(Name = "Product number")]
        public string ProductNumber { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Product group")]
        public string ProductGroup { get; set; }

        [Display(Name = "Order date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer number")]
        public string CustomerNumber { get; set; }


        public void BringOrderFromFile()
        {

            string order1Path = Directory.GetCurrentDirectory() + "/App_Data/Order1.txt";
            string order2Path = Directory.GetCurrentDirectory() + "/App_Data/Order2.txt";
            string order3Path = Directory.GetCurrentDirectory() + "/App_Data/Order3.txt";


            string[] order1 = File.ReadAllLines(order1Path);
            string[] order2 = File.ReadAllLines(order2Path);
            string[] order3 = File.ReadAllLines(order3Path);


            var add = order1.Skip(1).Concat(order2.Skip(1));
            var add2 = add.Concat(order3.Skip(1));

            var list = new List<Order>();


            foreach (var line in add2)
            {
                var values = line.Split('|');

                if (values[0] == string.Empty)
                {
                    var order = new Order
                    {
                        OrderNumber = int.Parse(values[1]),
                        OrderLineNumber = int.Parse(values[2]),
                        ProductNumber = values[3],
                        Quantity = int.Parse(values[4]),
                        Name = values[5],
                        Description = values[6],
                        Price = double.Parse(values[7], CultureInfo.InvariantCulture),
                        ProductGroup = values[8],
                        OrderDate = DateTime.Parse(values[9]),
                        CustomerName = values[10],
                        CustomerNumber = values[11]
                    };

                    _orderDbContext.Orders.Add(order);
                }

                else
                {
                    var order = new Order
                    {
                        OrderNumber = int.Parse(values[0]),
                        OrderLineNumber = int.Parse(values[1]),
                        ProductNumber = values[2],
                        Quantity = int.Parse(values[3]),
                        Name = values[4],
                        Description = values[5],
                        Price = double.Parse(values[6]),
                        ProductGroup = values[7],
                        OrderDate = DateTime.Parse(values[8]),
                        CustomerName = values[9],
                        CustomerNumber = values[10]
                    };

                    _orderDbContext.Orders.Add(order);
                }

                
            }

            _orderDbContext.SaveChanges();
        }


        public OrderViewModel()
        {

        }

        public OrderViewModel(Order order)
        {
            Id = order.Id;
            OrderNumber = order.OrderNumber;
            OrderLineNumber = order.OrderLineNumber;
            ProductNumber = order.ProductNumber;
            Quantity = order.Quantity;
            Name = order.Name;
            Description = order.Description;
            Price = order.Price;
            ProductGroup = order.ProductGroup;
            OrderDate = order.OrderDate;
            CustomerName = order.CustomerName;
            CustomerNumber = order.CustomerNumber;
        }
    }
}
