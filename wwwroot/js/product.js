var _id = 0;
var dTable = null;

$(document).ready(function () {

    Manager.GetDataForTable(0);

    $("#btn-add-product").on('click', function () {
        Manager.Reset();
        $("#product-from-modal").modal("show");
    });    

    $('#product-btn-save').on('click', function () {
        if (_id == 0) {
            Manager.Save();
        }
        else {
            Manager.Edit();
        }
    });
});


var Manager = {

    Reset: function () {
        $("#product-id")[0].reset();
        _id = "0";
    },

    Save: function () {
        var price = parseFloat($('#Price').val());
        var stockQty = parseInt($('#StockQuantity').val());
        if (!$('#Name').val()) {
            alert("Name is required");
            return;
        }
        if (!$('#Price').val()) {
            alert("Price is required");
            return;
        }
        if (price <= 0) {
            alert("Price must be greater than 0");
            return;
        }

        if (!$('#StockQuantity').val()) {
            alert("Stock quantity is required");
            return;
        }
        if (stockQty < 0) {
            alert("Stock Quantity cannot be negative");
            return;
        }
        if (confirm("Do you want to save this product?")) {
            var serviceURL = "/api/product/create";
    
            var formData = {
                Name: $("#product-id [name='Name']").val(),
                Price: price,
                StockQuantity: stockQty,
                Description: $("#product-id [name='Description']").val()
            };

            AjaxManager.HttpPost(serviceURL, formData, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData.payload == "0") {
                alert("Failed to save.")
            } else {
                alert("Successfully saved.");
                Manager.GetDataForTable(1);
                Manager.Reset();
            }
        }
        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }


    },

    Edit: function () {
        var price = parseFloat($('#Price').val());
        var stockQty = parseInt($('#StockQuantity').val());
        if (!$('#Name').val()) {
            alert("Name is required");
            return;
        }
        if (!$('#Price').val()) {
            alert("Price is required");
            return;
        }
        if (price <= 0) {
            alert("Price must be greater than 0");
            return;
        }
        if (!$('#StockQuantity').val()) {
            alert("Stock quantity is required");
            return;
        }
        if (stockQty < 0) {
            alert("Stock Quantity cannot be negative");
            return;
        }

        if (confirm("Do you want to update this product?")) {
            var serviceURL = "/api/product/update"; 
            var formData = {
                ProductId: _id,
                Name: $("#product-id [name='Name']").val(),
                Price: price,
                StockQuantity: stockQty,
                Description: $("#product-id [name='Description']").val()
            };

            AjaxManager.HttpPost(serviceURL, formData, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData.payload == "0") {
                alert("Failed to update.");
            } else {
                alert("Successfully updated.");
                Manager.GetDataForTable(1); 
                Manager.Reset();
                $("#product-from-modal").modal("hide");
            }
        }

        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }
    },

    Delete: function (id) {
        if (confirm("Do you want to delete this product?")) {
            var jsonParam = { id: id };
            var serviceURL = "/api/product/delete";
            AjaxManager.HttpDelete(serviceURL, jsonParam, onSuccess, onFailed);
        }
        function onSuccess(jsonData) {
            if (jsonData.payload == "0") {
                alert("Failed to delete.");
            }
            else {
                alert("Successfully deleted.");
                Manager.GetDataForTable(1);
                Manager.Reset();
            }
        }
        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }
    },


    GetDataForTable: function (refresh) {
        var jsonParam = '';
        var serviceURL = "/api/product/get-all-products";
        AjaxManager.HttpGet(serviceURL, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            Manager.LoadDataTable(jsonData.payload, refresh);
        }

        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }
    },

    LoadDataTable: function (data, refresh) {

        if (refresh == "0") {
            dTable = $('#product-table-element').DataTable({
                lengthMenu: [[5, 10, 15, 20], [5, 10, 15, 20, "All"]],
                lengthChange: false,
                paging: true,
                searching: false,
                columnDefs: [
                    { visible: false, targets: [] },
                    { className: "dt-center", targets: [] }
                ],
                columns: [

                    {
                        data: 'name',
                        name: 'name',
                        title: 'Name'
                    },

                    {
                        data: 'price',
                        name: 'price',
                        title: 'Price'
                    },
                    {
                        data: 'stockQuantity',
                        name: 'stockQuantity',
                        title: 'Stock Quantity'
                    },

                    {
                        data: 'description',
                        name: 'description',
                        title: 'Description'
                    },
                    {
                        name: 'Option',
                        title: 'Option',
                        orderable: false,
                        width: 100,
                        render: function (data, type, row) {
                            return `
                                    <div class="dt-edit" title="Edit" style="display:inline-block; cursor:pointer; margin-right:5px;">
                                        <i class="fa fa-pencil"></i>
                                    </div>
                                    <div class="dt-delete" title="Delete" style="display:inline-block; cursor:pointer; color:red;">
                                        <i class="fa fa-trash"></i>
                                    </div>
                                `;
                        }
                    }
                ],
                data: data
            });         

        } else {
            dTable.clear().rows.add(data).draw();
        }
    },

};

$(document).on('click', '.dt-edit', function () {
    var tr = $(this).closest('tr');
    var rowData = dTable.row(tr).data();
    _id = rowData.productId;
    $('#Name').val(rowData.name);
    $('#Price').val(rowData.price);
    $('#StockQuantity').val(rowData.stockQuantity);
    $('#Description').val(rowData.description);
    $('#product-from-modal').modal('show');
});

$(document).on('click', '.dt-delete', function () {
    var row = $(this).closest('tr');
    var id = dTable.row(row).data().productId;
    Manager.Delete(id);
});