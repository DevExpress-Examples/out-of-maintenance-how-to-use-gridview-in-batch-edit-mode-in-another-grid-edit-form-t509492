using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T508133.Models;

using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace T508133.Controllers {
    public class HomeController : Controller {
        // GET: Home
        public ActionResult Index() {
            return View();
        }
        #region Orders
        public ActionResult GridViewOrders() {
            //DataContext.PendingItems.Clear();
            var model = DataContext.Orders;
            return PartialView("GridViewOrders", model);
        }

        public ActionResult InsertOrder(Order order) {
            if (ModelState.IsValid)
                DataContext.InsertOrder(order);
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return GridViewOrders();
        }
        public ActionResult UpdateOrder(Order order) {
            if (ModelState.IsValid)
                DataContext.UpdateOrder(order);
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return GridViewOrders();
        }
        public ActionResult RemoveOrder(int id = -1) {
            if (id > -1)
                DataContext.RemoveOrder(id);
            ViewData["EditError"] = "Row is invalid";
            return GridViewOrders();
        }
        #endregion


        #region Items
        public ActionResult GridViewOrderItems(int orderId = -1) {
            ViewData["orderId"] = orderId;
            var order = DataContext.Orders.FirstOrDefault(i => i.Id == orderId);
            var model = order == null ? DataContext.PendingItems.Count > 0 ? DataContext.PendingItems : new List<OrderItem>() : order.Items;
            return PartialView("GridViewOrderItems", model);
        }
        public ActionResult OrderItemsBatchUpdate(MVCxGridViewBatchUpdateValues<OrderItem, Guid> updateValues, int orderId = -1) {
            if (ModelState.IsValid) {
                DataContext.UpdateOrderItems(updateValues.Update, orderId);
                DataContext.InsertOrderItems(updateValues.Insert, orderId);
                DataContext.RemoveOrderItems(updateValues.DeleteKeys, orderId);
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return GridViewOrderItems(orderId);
        }
        #endregion
    }
}