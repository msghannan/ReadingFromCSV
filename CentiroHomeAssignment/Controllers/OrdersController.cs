using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CentiroHomeAssignment.DataLayer;
using CentiroHomeAssignment.Models;
using CentiroHomeAssignment.ViewModels;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace CentiroHomeAssignment.Controllers
{
    public class OrdersController : Controller
    {
        OrderViewModel orderViewModel = new OrderViewModel();
        OrderDbContext orderDbContext = new OrderDbContext();

        public OrdersController()
        {

        }

        public IActionResult GetAll()
        {
            // TODO: Return all orders to a view


            var orders = orderDbContext.Orders.ToList();

            return View(orders);
        }

        public IActionResult NewOrder()
        {
            var viewModel = new OrderViewModel
            {
                Order = new Order()
            };

            return View("OrderForm", viewModel);
        }

        public IActionResult Edit(int id, int orderNumber)
        {

            var order = orderDbContext.Orders.SingleOrDefault(o => o.Id == id && o.OrderNumber == orderNumber);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel(order);

            return View("OrderForm", viewModel);
        }

        public IActionResult Delete(int id, int orderNumber)
        {
            var order = orderDbContext.Orders.SingleOrDefault(o => o.Id == id && o.OrderNumber == orderNumber);

            if (order == null)
            {
                return NotFound();
            }

            orderDbContext.Remove(order);
            orderDbContext.SaveChanges();

            return RedirectToAction("GetAll", "Orders");
        }

        public IActionResult Save(Order order)
        {
            var orders = orderDbContext.Orders.ToList();

            if (orders.Count() > 0 && order.Id == 0)
            {
                int maxOrderNumber = orders.Max(o => o.OrderNumber);
                int latestOrderNumber = maxOrderNumber + 1;

                int listLenght = orders.Count();
                int latestListLenght = listLenght + 1;

                if (order.Id == 0)
                {
                    order.OrderNumber = latestOrderNumber;
                    order.OrderLineNumber = latestListLenght;
                    order.OrderDate = DateTime.Now;
                    orderDbContext.Orders.Add(order);
                }
            }


            else if (orders.Count() <= 0 )
            {
                Random rd = new Random();

                int orderNumber = rd.Next(1, 9999);

                int listLenght = orders.Count();
                int latestListLenght = listLenght + 1;

                if (order.Id == 0)
                {
                    order.OrderNumber = orderNumber;
                    order.OrderLineNumber = latestListLenght;
                    order.OrderDate = DateTime.Now;
                    orderDbContext.Orders.Add(order);
                }
            }

            else
            {
                var orderInDb = orderDbContext.Orders.SingleOrDefault(o => o.Id == order.Id && o.OrderNumber == order.OrderNumber);

                orderInDb.ProductNumber = order.ProductNumber;
                orderInDb.Quantity = order.Quantity;
                orderInDb.Name = order.Name;
                orderInDb.Description = order.Description;
                orderInDb.Price = order.Price;
                orderInDb.ProductGroup = order.ProductGroup;
                orderInDb.OrderDate = order.OrderDate;
                orderInDb.CustomerName = order.CustomerName;
                orderInDb.CustomerNumber = order.CustomerNumber;
            }

            orderDbContext.SaveChanges();

            return RedirectToAction("GetAll", "Orders");
        }

        public IActionResult InsertOrdersFromFile()
        {
            orderViewModel.BringOrderFromFile();

            return RedirectToAction("GetAll");
        }
    }
}
