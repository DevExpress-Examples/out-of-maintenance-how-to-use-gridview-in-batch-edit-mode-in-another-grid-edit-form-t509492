using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T508133.Models {
    public class DataContext {
        static string fOrders = "Orders";
        public static List<Order> Orders {
            get {
                List<Order> list = HttpContext.Current.Session[fOrders] as List<Order>;
                if (list == null)
                    HttpContext.Current.Session[fOrders] = list = GenerateOrders();
                return list;
            }
        }

        static string fProducts = "Products";
        public static List<Product> Products {
            get {
                List<Product> list = HttpContext.Current.Session[fProducts] as List<Product>;
                if (list == null)
                    HttpContext.Current.Session[fProducts] = list = GenerateProducts();
                return list;
            }
        }

        public static List<OrderItem> PendingItems = new List<OrderItem>();

        internal static void UpdateOrder(Order order) {
            Order item = Orders.Find(i => i.Id == order.Id);
            if (item != null)
                item.Name = order.Name;
        }

        internal static void InsertOrder(Order order) {
            order.Items.AddRange(PendingItems);
            order.Id = Orders.Count;
            if (Orders.Find(i => i.Id == order.Id) == null) {
                Orders.Add(order);
                PendingItems.Clear();
            }
        }

        internal static void RemoveOrder(int orderId) {
            Order item = Orders.Find(i => i.Id == orderId);
            if (item != null)
                Orders.Remove(item);
        }

        internal static void InsertOrderItems(List<OrderItem> insert, int orderId) {
            if (orderId == -1) PendingItems.AddRange(insert);
            else {
                Order order = Orders.FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                    order.Items.AddRange(insert);
            }
        }

        internal static void RemoveOrderItems(List<Guid> deleteKeys, int orderId) {
            Order order = Orders.FirstOrDefault(o => o.Id == orderId);
            deleteKeys.ForEach(i => {
                OrderItem item = order.Items.Find(oi =>  oi.Id == i);
                if (item != null)
                    order.Items.Remove(item);
            });
        }

        internal static void UpdateOrderItems(List<OrderItem> update, int orderId) {
            Order order = Orders.FirstOrDefault(o => o.Id == orderId);
            update.ForEach(i => {
                OrderItem item = order.Items.Find(oi => oi.Id == i.Id);
                item.Quantity = i.Quantity;
                item.ProductId = i.ProductId;
            });
        }

        private static List<Order> GenerateOrders() {
            List<Order> list = Enumerable.Range(0, 3).Select(i => {
                Order order = new Order { Id = i, Name = "Order" + i };
                order.Items.Add(new OrderItem { ProductId = i, Quantity = i * 2 });
                order.Items.Add(new OrderItem { ProductId = i + 2, Quantity = i * 3 });
                return order;
            }).ToList();
            return list;
        }

        private static List<Product> GenerateProducts() {
            return Enumerable.Range(0, 10).Select(i => new Product { Id = i, Name = "Product" + i }).ToList();
        }
    }

    public class Order {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderItem> Items { get; set; }

        public Order() {
            Items = new List<OrderItem>();
        }
    }

    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderItem {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderItem() {
            Id = Guid.NewGuid();
        }
    }
}