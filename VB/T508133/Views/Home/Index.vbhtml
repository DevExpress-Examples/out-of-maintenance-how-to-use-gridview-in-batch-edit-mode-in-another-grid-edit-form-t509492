﻿
<h2>Index</h2>

<script>
    var buttonHandlers = {
        update: function(s, e) {
            GridViewOrderItems.batchEditApi.HasChanges() ? GridViewOrderItems.UpdateEdit() : GridViewOrders.UpdateEdit();
        },
        cancel: function(s, e) { GridViewOrders.CancelEdit(); }
    };

    var orderItemsHandlers = {
        endCallback: function(s, e) {
            if (GridViewOrderItems.batchEditApi.HasChanges()) return;
            GridViewOrders.UpdateEdit();
        }
    }

</script>

@Html.Action("GridViewOrders")