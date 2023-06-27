function GetOrderList()
{
    Post(baseUrl + "api/Order/get-order-history", {}, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            let response = data.data;
            $('#order-items').html('');
            for (var i = 0; i < response.length; i++)
            {
                $('#order-items').append(`
                    <tr>
                        <td>${response[i].orderName}</td>
                        <td><button class="btn btn-primary" onclick="GotoOrderDetailPage('${response[i].orderId}')">Sipariş Detay</button></td>
                    </tr>
                `);
            }
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GotoOrderDetailPage(orderId) {
    window.location.href = "/Order/Detail?orderId=" + orderId;
}

function GetQueryString() {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
    let orderId = params.orderId; // "some_value"
    return orderId;
}

function GetOrderDetail(orderId) {
    let input = {
        "orderId": orderId
    };

    let sumPrice = 0;

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/Order/get-selected-order", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            let response = data.data;

            $('#order-detail-items').html('');
            for (var i = 0; i < response.length; i++) {
                var row = createCartItemRow(response[i]);
                $('#order-detail-items').append(row);
                sumPrice += response[i].totalPrice;
            }

            var totalPrice = 0;
            totalPrice += sumPrice;
            $('#order-detail-total-price').text(totalPrice.toFixed(2));
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}


function createCartItemRow(item) {
    var row = $('<tr></tr>');
    row.append('<td class="cart-item-fullname">' + item.fullname + '</td>');
    row.append('<td class="cart-item-brand">' + item.brandName + '</td>');
    row.append('<td class="cart-item-name">' + item.perfumeName + '</td>');
    row.append(`<td class="cart-item-quantity">${item.quantity}</td>`);
    row.append(`<td class="cart-item-price">${item.price.toFixed(2)}₺</td>`);
    row.append('<td class="cart-item-total-price">' + item.totalPrice.toFixed(2) + '₺</td>');
    row.append('<td class="cart-item-order-date">' + item.orderDate + '</td>');

    return row;
}

function GotoOrderPage() {
    window.location.href = "/Order/Index";
}