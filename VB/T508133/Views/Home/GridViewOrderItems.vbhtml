@Html.DevExpress().GridView(Sub(settings)

                                     settings.Name = "GridViewOrderItems"
                                     settings.KeyFieldName = "Id"
                                     settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "GridViewOrderItems", Key .orderId = ViewData("orderId")}
                                     settings.SettingsEditing.BatchUpdateRouteValues = New With {Key .Controller = "Home", Key .Action = "OrderItemsBatchUpdate", Key .orderId = ViewData("orderId")}
                                     settings.CommandColumn.Visible = True
                                     settings.CommandColumn.ShowEditButton = True
                                     settings.CommandColumn.ShowNewButton = True
                                     settings.CommandColumn.ShowDeleteButton = True
                                     settings.CommandColumn.Width = Unit.Percentage(20)
                                     settings.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden
                                     settings.SettingsEditing.Mode = GridViewEditingMode.Batch
                                     settings.Columns.Add(Sub(column)
                                                              column.FieldName = "ProductId"
                                                              column.SortAscending()
                                                              column.EditorProperties().ComboBox(Sub(combo)
                                                                                                     combo.ValueField = "Id"
                                                                                                     combo.TextField = "Name"
                                                                                                     combo.ValueType = GetType(Integer)
                                                                                                     combo.BindList(T508133.Models.DataContext.Products)
                                                                                                     combo.ValidationSettings.Display = Display.Dynamic
                                                                                                 End Sub)
                                                          End Sub)
                                     settings.Columns.Add(Sub(column)
                                                              column.FieldName = "Quantity"
                                                              TryCast(column.PropertiesEdit, TextBoxProperties).ValidationSettings.Display = Display.Dynamic
                                                          End Sub)
                                     settings.Width = Unit.Percentage(100)
                                     settings.ClientSideEvents.EndCallback = "orderItemsHandlers.endCallback"
                                 End Sub).SetEditErrorText(CType(ViewData("EditError"), String)).Bind(Model).GetHtml()
