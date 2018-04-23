Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports T508133.Models

Imports DevExpress.Web
Imports DevExpress.Web.Mvc

Namespace T508133.Controllers
	Public Class HomeController
		Inherits Controller

		' GET: Home
		Public Function Index() As ActionResult
			Return View()
		End Function
		#Region "Orders"
		Public Function GridViewOrders() As ActionResult
			'DataContext.PendingItems.Clear();
			Dim model = DataContext.Orders
			Return PartialView("GridViewOrders", model)
		End Function

		Public Function InsertOrder(ByVal order As Order) As ActionResult
			If ModelState.IsValid Then
				DataContext.InsertOrder(order)
			Else
				ViewData("EditError") = "Please, correct all errors."
			End If
			Return GridViewOrders()
		End Function
		Public Function UpdateOrder(ByVal order As Order) As ActionResult
			If ModelState.IsValid Then
				DataContext.UpdateOrder(order)
			Else
				ViewData("EditError") = "Please, correct all errors."
			End If
			Return GridViewOrders()
		End Function
		Public Function RemoveOrder(Optional ByVal id As Integer = -1) As ActionResult
			If id > -1 Then
				DataContext.RemoveOrder(id)
			End If
			ViewData("EditError") = "Row is invalid"
			Return GridViewOrders()
		End Function
		#End Region


		#Region "Items"
		Public Function GridViewOrderItems(Optional ByVal orderId As Integer = -1) As ActionResult
			ViewData("orderId") = orderId
			Dim order = DataContext.Orders.FirstOrDefault(Function(i) i.Id = orderId)
			Dim model = If(order Is Nothing, If(DataContext.PendingItems.Count > 0, DataContext.PendingItems, New List(Of OrderItem)()), order.Items)
			Return PartialView("GridViewOrderItems", model)
		End Function
		Public Function OrderItemsBatchUpdate(ByVal updateValues As MVCxGridViewBatchUpdateValues(Of OrderItem, Guid), Optional ByVal orderId As Integer = -1) As ActionResult
			If ModelState.IsValid Then
				DataContext.UpdateOrderItems(updateValues.Update, orderId)
				DataContext.InsertOrderItems(updateValues.Insert, orderId)
				DataContext.RemoveOrderItems(updateValues.DeleteKeys, orderId)
			Else
				ViewData("EditError") = "Please, correct all errors."
			End If

			Return GridViewOrderItems(orderId)
		End Function
		#End Region
	End Class
End Namespace