@Html.DevExpress().GridView(Sub(settings)
                                     settings.Name = "GridViewOrders"
                                     settings.KeyFieldName = "Id"
                                     settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "GridViewOrders"}
                                     settings.SettingsEditing.AddNewRowRouteValues = New With {Key .Controller = "Home", Key .Action = "InsertOrder"}
                                     settings.SettingsEditing.UpdateRowRouteValues = New With {Key .Controller = "Home", Key .Action = "UpdateOrder"}
                                     settings.SettingsEditing.DeleteRowRouteValues = New With {Key .Controller = "Home", Key .Action = "RemoveOrder"}
                                     settings.CommandColumn.Visible = True
                                     settings.CommandColumn.ShowEditButton = True
                                     settings.CommandColumn.ShowNewButton = True
                                     settings.CommandColumn.ShowDeleteButton = True
                                     settings.CommandColumn.Width = Unit.Percentage(20)
                                     settings.Columns.Add(Sub(column)
                                                              column.FieldName = "Id"
                                                              column.ReadOnly = True
                                                          End Sub)
                                     settings.Columns.Add("Name")
                                     settings.SetEditFormTemplateContent(Sub(c)
                                                                             Html.DevExpress().FormLayout(Sub(form)
                                                                                                              form.Name = "EditForm"
                                                                                                              form.ColCount = 2
                                                                                                              form.Width = Unit.Percentage(100)
                                                                                                              form.Items.Add(Sub(i)
                                                                                                                                 i.Caption = "Name"
                                                                                                                                 i.NestedExtension().TextBox(Sub(name)
                                                                                                                                                                 name.Name = "Name"
                                                                                                                                                                 name.Text = TryCast(DataBinder.Eval(c.DataItem, "Name"), String)
                                                                                                                                                             End Sub)
                                                                                                                             End Sub)
                                                                                                              form.Items.Add(Sub(i)
                                                                                                                                 i.ColSpan = 2
                                                                                                                                 i.ShowCaption = DefaultBoolean.False
                                                                                                                                 i.SetNestedContent(Sub() Html.RenderAction("GridViewOrderItems", New With {Key .orderId = If(c.Grid.IsNewRowEditing, -1, c.KeyValue)}))
                                                                                                                             End Sub)
                                                                                                              form.Items.AddEmptyItem()
                                                                                                              form.Items.Add(Sub(i)
                                                                                                                                 i.ColSpan = 1
                                                                                                                                 i.ShowCaption = DefaultBoolean.False
                                                                                                                                 i.HorizontalAlign = FormLayoutHorizontalAlign.Right
                                                                                                                                 i.SetNestedContent(Sub()
                                                                                                                                                        Html.DevExpress().Button(Sub(button)
                                                                                                                                                                                     button.Name = "Update"
                                                                                                                                                                                     button.Text = "Update"
                                                                                                                                                                                     button.RenderMode = ButtonRenderMode.Link
                                                                                                                                                                                     button.ClientSideEvents.Click = "buttonHandlers.update"
                                                                                                                                                                                     button.Styles.Style.Paddings.PaddingRight = 10
                                                                                                                                                                                 End Sub).Render()
                                                                                                                                                        Html.DevExpress().Button(Sub(button)
                                                                                                                                                                                     button.Name = "Cancel"
                                                                                                                                                                                     button.Text = "Cancel"
                                                                                                                                                                                     button.RenderMode = ButtonRenderMode.Link
                                                                                                                                                                                     button.ClientSideEvents.Click = "buttonHandlers.cancel"
                                                                                                                                                                                 End Sub).Render()
                                                                                                                                                    End Sub)
                                                                                                                             End Sub)
                                                                                                          End Sub).Render()
                                                                         End Sub)
                                     settings.Width = Unit.Percentage(40)
                                 End Sub).SetEditErrorText(CType(ViewData("EditError"), String)).Bind(Model).GetHtml()
