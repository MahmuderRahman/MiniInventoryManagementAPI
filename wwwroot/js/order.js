var _id = 0;
var dTable = null;

$(document).ready(function () {

    Manager.GetDataForTable(0);

    Manager.GetProductDropdown($("#productDropdown"));

    $('.pick-date-global').datetimepicker({
        timepicker: false,
        format: 'd-m-Y'
    });

    $("#btn-add-order").on('click', function () {
        Manager.Reset();
        $("#order-from-modal").modal("show");
    });    

    $('#order-btn-save').on('click', function () {
        Manager.Save();
    });

    $("#btnAddItem").click(function () {
        var newRow = `
        <tr>
            <td><select id="productDropdown" class="form-select product"></select></td>
            <td><input type="number" class="form-control qty" min="1" value=""></td>
            <td><input type="number" class="form-control price" min="0" value="" readonly></td>
            <td><input type="number" class="form-control subTotal" readonly></td>
            <td class="text-center"><button type="button" class="btn btn-danger btn-sm btnRemove"><i class="fa fa-trash"></i></button></td>
        </tr>`;

        $("#orderItemTable tbody").append(newRow);

        var $dropdown = $("#orderItemTable tbody tr:last").find(".product");

        Manager.GetProductDropdown($dropdown);
    });

});


var Manager = {

    Save: function () {
        if (!$('#CustomerName').val()) {
            alert("Customer Name is required");
            return;
        }
        if (!$('#OrderDate').val()) {
            alert("Order Date is required");
            return;
        }
        if (!$('#Status').val()) {
            alert("Status is required");
            return;
        }

        var orderObj = {
                    CustomerName: $("#order-id [name='CustomerName']").val(),
                    TotalAmount: parseFloat($("#order-id [name='TotalAmount']").val()),
                    OrderDate: new Date($("#order-id [name='OrderDate']").val().split('-').reverse().join('-')).toISOString(),
                    Status: parseInt($("#order-id [name='Status']").val()),
                    OrderItems: []
        };        

        var hasError = false;
        $("#orderItemTable tbody tr").each(function () {
            var productId = $(this).find(".product").val();
            var qty = $(this).find(".qty").val();
            var price = $(this).find(".price").val();
            var subTotal = $(this).find(".subTotal").val();

            if (!productId || productId === "0") {
                alert("Product is required");
                hasError = true;
                return false;
            }
            if (!qty || qty <= 0) {
                alert("Quantity must be greater than 0");
                hasError = true;
                return false;
            }
            if (!price || price <= 0) {
                alert("Unit Price is required");
                hasError = true;
                return false;
            }

            orderObj.OrderItems.push({
                ProductId: parseInt(productId),
                Quantity: parseInt(qty),
                UnitPrice: parseFloat(price),
                SubTotal: parseFloat(subTotal)
            });
        });

        if (hasError) return;
        if (orderObj.OrderItems.length === 0) {
            alert("At least one order item is required before saving.");
            return;
        }

        if (confirm("Do you want to save this order?")) {

            var serviceURL = "/api/order/create";

            $.ajax({
                type: "POST",
                url: serviceURL,
                data: JSON.stringify(orderObj),
                contentType: "application/json; charset=utf-8",

                success: function (jsonData) {
                    if (jsonData.payload == "0") {
                        alert("Failed to save.");
                    } else {
                        alert("Successfully saved.");
                        Manager.GetDataForTable(1);
                        Manager.Reset();
                    }
                },
                error: function (xhr) {
                    var msg = "Error occurred while saving.";
                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        msg = xhr.responseJSON.message;
                    }
                    alert(msg);
                }

            });
        }
    },

    GetProductDropdown: function ($dropdown) {
        if (!$dropdown || $dropdown.length === 0) return;

        var serviceURL = "/api/product/get-product-dropdown";
        AjaxManager.HttpGet(serviceURL, "", onSuccess, onFailed);

        function onSuccess(jsonData) {
            var productList = jsonData.payload;
            $dropdown.empty();
            $dropdown.append(`<option value="0">Select Product</option>`);
            $.each(productList, function (i, item) {
                var id = item.id || 0;
                var name = item.name || "";
                var price = item.price || "";
                $dropdown.append(`<option value="${id}" data-price="${price}">${name}</option>`);
            });
        }

        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }
    },

    Reset: function () {
        $("#order-id")[0].reset();
        _id = "0";

        $("#orderItemTable tbody").empty();

        var newRow = `
        <tr>
            <td>
                <select class="form-select product"></select>
            </td>
            <td><input type="number" class="form-control qty" min="1" value=""></td>
            <td><input type="number" class="form-control price" min="0" value="" readonly></td>
            <td><input type="number" class="form-control subTotal" readonly></td>
            <td class="text-center">
                <button type="button" class="btn btn-danger btn-sm btnRemove"><i class="fa fa-trash"></i></button>
            </td>
        </tr>`;
        $("#orderItemTable tbody").append(newRow);
        var $dropdown = $("#orderItemTable tbody tr:last").find(".product");
        Manager.GetProductDropdown($dropdown);
    },

    GetDataForTable: function (refresh) {
        var jsonParam = '';
        var serviceURL = "/api/order/get-all-order";
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
            dTable = $('#order-table-element').DataTable({
                lengthMenu: [[5, 10, 15, 20], [5, 10, 15, 20, "All"]],
                lengthChange: false,
                paging: true,
                searching: false,
                columnDefs: [
                    { visible: false, targets: [] },
                    { className: "dt-center", targets: [1, 3] }
                ],
                columns: [

                    {
                        data: 'customerName',
                        name: 'customerName',
                        title: 'Customer Name'
                    },
                    {
                        data: 'orderDate',
                        name: 'orderDate',
                        title: 'Order Date',
                        render: function (data) {
                            if (!data) return '';
                            return data.split('T')[0];
                        }
                    },
                    {
                        data: 'orderItems',
                        name: 'orderItems',
                        title: 'Products',
                        render: function (data, type, row) {
                            if (!data || data.length === 0) return '';
                            return data.map(i => `Name: ${i.productName}, Qty: ${i.quantity}, UnitPrice: ${i.unitPrice}`).join('<br/>');
                        }
                    },
                    {
                        data: 'totalAmount',
                        name: 'totalAmount',
                        title: 'Total Amount'
                    },
                    {
                        data: 'status',
                        title: 'Status',
                        render: function (data) {
                            switch (data) {
                                case 0: return '<span class="badge bg-warning text-dark">Pending</span>';
                                case 1: return '<span class="badge bg-primary">Completed</span>';
                                case 2: return '<span class="badge bg-danger">Cancelled</span>';
                                default: return '<span class="badge bg-secondary"></span>';
                            }
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

$(document).on("input", ".qty, .price", function () {
    var row = $(this).closest("tr");
    var qty = parseFloat(row.find(".qty").val()) || 0;
    var price = parseFloat(row.find(".price").val()) || 0;
    var subTotal = qty * price;
    row.find(".subTotal").val(subTotal.toFixed(2));
    calculateTotal();
});

function calculateTotal() {
    var total = 0;
    $("#orderItemTable tbody tr").each(function () {
        var sub = parseFloat($(this).find(".subTotal").val()) || 0;
        total += sub;
    });
    $("#TotalAmount").val(total.toFixed(2));
}
$(document).on("change", ".product", function () {
    var price = $(this).find("option:selected").data("price") || 0;
    var $row = $(this).closest("tr");
    $row.find(".price").val(price);
    $row.find(".qty").val('');
    $row.find(".subTotal").val('');
});

$("#orderItemTable").on("click", ".btnRemove", function () {
    $(this).closest("tr").remove();
    calculateTotal();
});