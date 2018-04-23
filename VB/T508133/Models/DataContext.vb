Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace T508133.Models
	Public Class DataContext
		Private Shared fOrders As String = "Orders"
		Public Shared ReadOnly Property Orders() As List(Of Order)
			Get
				Dim list As List(Of Order) = TryCast(HttpContext.Current.Session(fOrders), List(Of Order))
				If list Is Nothing Then
					list = GenerateOrders()
					HttpContext.Current.Session(fOrders) = list
				End If
				Return list
			End Get
		End Property

		Private Shared fProducts As String = "Products"
		Public Shared ReadOnly Property Products() As List(Of Product)
			Get
				Dim list As List(Of Product) = TryCast(HttpContext.Current.Session(fProducts), List(Of Product))
				If list Is Nothing Then
					list = GenerateProducts()
					HttpContext.Current.Session(fProducts) = list
				End If
				Return list
			End Get
		End Property

		Public Shared PendingItems As New List(Of OrderItem)()

		Friend Shared Sub UpdateOrder(ByVal order As Order)
			Dim item As Order = Orders.Find(Function(i) i.Id = order.Id)
			If item IsNot Nothing Then
				item.Name = order.Name
			End If
		End Sub

		Friend Shared Sub InsertOrder(ByVal order As Order)
			order.Items.AddRange(PendingItems)
			order.Id = Orders.Count
			If Orders.Find(Function(i) i.Id = order.Id) Is Nothing Then
				Orders.Add(order)
				PendingItems.Clear()
			End If
		End Sub

		Friend Shared Sub RemoveOrder(ByVal orderId As Integer)
			Dim item As Order = Orders.Find(Function(i) i.Id = orderId)
			If item IsNot Nothing Then
				Orders.Remove(item)
			End If
		End Sub

		Friend Shared Sub InsertOrderItems(ByVal insert As List(Of OrderItem), ByVal orderId As Integer)
			If orderId = -1 Then
				PendingItems.AddRange(insert)
			Else
				Dim order As Order = Orders.FirstOrDefault(Function(o) o.Id = orderId)
				If order IsNot Nothing Then
					order.Items.AddRange(insert)
				End If
			End If
		End Sub

		Friend Shared Sub RemoveOrderItems(ByVal deleteKeys As List(Of Guid), ByVal orderId As Integer)
			Dim order As Order = Orders.FirstOrDefault(Function(o) o.Id = orderId)
			deleteKeys.ForEach(Sub(i)
				Dim item As OrderItem = order.Items.Find(Function(oi) oi.Id = i)
				If item IsNot Nothing Then
					order.Items.Remove(item)
				End If
			End Sub)
		End Sub

		Friend Shared Sub UpdateOrderItems(ByVal update As List(Of OrderItem), ByVal orderId As Integer)
			Dim order As Order = Orders.FirstOrDefault(Function(o) o.Id = orderId)
			update.ForEach(Sub(i)
				Dim item As OrderItem = order.Items.Find(Function(oi) oi.Id = i.Id)
				item.Quantity = i.Quantity
				item.ProductId = i.ProductId
			End Sub)
		End Sub

		Private Shared Function GenerateOrders() As List(Of Order)
			Dim list As List(Of Order) = Enumerable.Range(0, 3).Select(Function(i)
				Dim order As Order = New Order With {.Id = i, .Name = "Order" & i}
				order.Items.Add(New OrderItem With {.ProductId = i, .Quantity = i * 2})
				order.Items.Add(New OrderItem With {.ProductId = i + 2, .Quantity = i * 3})
				Return order
			End Function).ToList()
			Return list
		End Function

		Private Shared Function GenerateProducts() As List(Of Product)
			Return Enumerable.Range(0, 10).Select(Function(i) New Product With {.Id = i, .Name = "Product" & i}).ToList()
		End Function
	End Class

	Public Class Order
		Public Property Id() As Integer
		Public Property Name() As String
		Public Property Items() As List(Of OrderItem)

		Public Sub New()
			Items = New List(Of OrderItem)()
		End Sub
	End Class

	Public Class Product
		Public Property Id() As Integer
		Public Property Name() As String
	End Class

	Public Class OrderItem
		Public Property Id() As Guid
		Public Property ProductId() As Integer
		Public Property Quantity() As Integer

		Public Sub New()
			Id = Guid.NewGuid()
		End Sub
	End Class
End Namespace