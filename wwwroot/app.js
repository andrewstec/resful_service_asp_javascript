//This is the connection URL address:
var serviceURL = "http://restfulservice.funkyprogrammer.com/api/Products";

//Pat included a link to learning more about enabling cross-origin requests on a server so that a web browser allows them.
// http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api

//Create a send request:

function sendRequest() {
    var tableHeader = "<table id='list_of_products'><th> Product ID </th> <th> Name </th> <th> Manufacturer </th><th> Vendor </th><th> Price </th></table>";
    $("#list_of_products").replaceWith(tableHeader);
    $.ajax({
        url: serviceURL
    }).done(function (data) {
        data.forEach(function (value) {
            callback(value)
        });
    }).error(function (jqXHR, textStatus, errorThrown) {
        $("#list_of_products").text(jqXHR.responseText || textStatus);
    });
}

function callback(value) {
   
    var parsed_values = "<tr><td>" + value.productID + "</td>" + "<td>" + value.name + "</td>" + "<td>" + value.mfg + "</td>" + "<td>" + value.vendor + "</td>" + "<td>" + value.price + "</td></tr>"
    $(parsed_values).appendTo($("#list_of_products"));
   
}

function find() {
    // the id from the search bar is stored in a variable
    var id = $("#IDtoFind").val();
    $.getJSON(serviceURL + "/" + id, function (query) {
        if (query == null) {
            $("#IDSearchResults").text("No results were found.");
        }
        else {
            var results = query.productID + " | " + query.name + " | " + query.mfg + " | " + query.vendor + " | " + query.price;
            $("#IDSearchResults").text(results);
        }
    }).fail(
    function (jqueryHeaderRequest, textStatus, err) {
        $("#IDSearchResults").text("There was an error searching: " + err);
    })
}

function update() {
    jQuery.support.cors = true;

    var product = {
        productID: $("#product_id").val(),
        name: $('#product_name').val(),
        mfg: $('select[name="product_manufacturer_list"]').val(),
        vendor: $('select[name="product_vendor_list"]').val(),
        price: $('#product_price').val()
    }

    var JSONproduct = JSON.stringify(product);

    //ajax request begins here
    $.ajax({
        url: serviceURL + "/" + $('#product_id').val(),
        type: 'PUT',
        data: JSONproduct,
        contentType: "application/json;charset=utf-8",
        success: function (dataPlaceholder) {
            $('#UpdateStatus').text('The product was successfully updated');
            sendRequest();
        },
        error: function (_httpRequest, _status, _httpError) {
            $('#UpdateStatus').text("There was an error updating the product: " + _httpRequest + _status + "http_Error: " + _httpError);
        }
    })
}

function del() {
    var id = $("#product_delete_id").val();

    $.ajax({
        url: serviceURL + "/" + id,
        type: "DELETE",
        dataType: "json",
        success: function(dataPlaceholder) {
            $("#UpdateDeleteStatus").text("You have successfully removed Product #" + id + ".");
            sendRequest();
        }
    }).fail(
    function (jqueryHeaderRequest, textStatus, err) {
        $("#UpdateDeleteStatus").text("There was an error removing the product: " + jqueryHeaderRequest + textStatus + err);
    })
}