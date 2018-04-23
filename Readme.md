# How to use GridView in Batch Edit mode in another grid Edit Form


<p>This example demonstrates how to edit a detail grid with Batch Edit mode enabled in the master grid edit form. The main point is that it is impossible to update both grids simultaneously. So, we need to send two consequent callbacks to update the detail and master grids:</p>


```js
function onUpdateButton (s, e) {
    if (detail.batchEditApi.HasChanges())
        detail.UpdateEdit();
    else master.UpdateEdit();
}

function onDetailEndCallback(s, e) {
    master.UpdateEdit();
}
```


<p>This code describes the basic principle, while the example shows a more complex and thorough implementation. Also, this approach requires additional actions when a new row is created in the master grid. As the detail grid is saved before the master grid, when a new row is created, the detail grid doesn't have the master key which is used to save the details. In this case, you will have to save the detail grid data to a temporary field and then write changes when the master row is created.</p>

<br/>


