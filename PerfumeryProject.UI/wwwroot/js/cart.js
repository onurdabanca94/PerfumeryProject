//function addToCart(item) {
//    var existingItem = findCartItem(item);
//    if (existingItem) {
//        existingItem.quantity++;
//        existingItem.totalPrice = existingItem.price * existingItem.quantity;
//        updateCartItem(existingItem);
//    } else {
//        item.totalPrice = item.price * item.quantity;
//        var row = createCartItemRow(item);
//        $('#cart-items').append(row);
//    }

//    var totalPrice = parseFloat($('#total-price').text());
//    totalPrice += item.price;
//    $('#total-price').text(totalPrice.toFixed(2));
//}

function findCartItem(item) {
    var existingItem = null;
    $('#cart-items tr').each(function () {
        var brand = $(this).find('.cart-item-brand').text();
        var name = $(this).find('.cart-item-name').text();
        if (brand === item.brand && name === item.name) {
            existingItem = {
                row: $(this),
                quantity: parseInt($(this).find('.cart-item-quantity').text()),
                price: parseFloat($(this).find('.cart-item-price').text()),
                totalPrice: parseFloat($(this).find('.cart-item-total-price').text())
            };
            return false;
        }
    });
    return existingItem;
}

function updateCartItem(item) {
    item.row.find('.cart-item-quantity').text(item.quantity);
    item.row.find('.cart-item-total-price').text(item.totalPrice.toFixed(2));
}

function createCartItemRow(item) {
    var row = $('<tr></tr>');
    row.append('<td class="cart-item-brand">' + item.brandName + '</td>');
    row.append('<td class="cart-item-name">' + item.name + '</td>');
    row.append('<td class="cart-item-price">' + item.price.toFixed(2) + '₺</td>');
    row.append(`<td class="cart-item-quantity"><input type="number" class="form-control" id="cart-quantity-${item.id}" min="0" value="${item.quantity}" /></td>`);
    row.append('<td class="cart-item-total-price">' + item.totalPrice.toFixed(2) + '₺</td>');
    row.append('<td><button type="button" class="btn btn-info btn-xs cart-item-update mx-2">Güncelle</button><button type="button" class="btn btn-danger btn-xs cart-item-remove">Sil</button></td>');

    row.find('.cart-item-remove').click(function () {
        removeFromCart(item);
    });

    row.find('.cart-item-update').click(function () {
        updateFromCart(item);
    });

    return row;
}

function removeFromCart(item) {
    let input = {
        "id": item.id
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/CartItems/delete-cart-item", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");

            var totalPrice = parseFloat($('#total-price').text());
            totalPrice -= item.totalPrice;
            $('#total-price').text(totalPrice.toFixed(2));

            GetCartList();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function updateFromCart(item) {
    let input = {
        "id": item.id,
        "userId": "0ba772f5-61a3-475e-9ba7-08db767cf11e",
        "brandName": item.brandName,
        "price": item.price,
        "quantity": Number($(`#cart-quantity-${item.id}`).val()),
        "isOrdered": false,
        "cartNumber": 0,
        "createdDate": new Date(),
        "name": item.name
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/CartItems/update-cart-item", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            GetCartList();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GetCartList() {
    let input = {
        "userId": "0ba772f5-61a3-475e-9ba7-08db767cf11e"
    };

    let sumPrice = 0;

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/CartItems/get-items", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");

            let response = data.data;

            $('#cart-items').html('');
            for (var i = 0; i < response.length; i++) {
                console.log(response[i]);
                var row = createCartItemRow(response[i]);
                $('#cart-items').append(row);
                sumPrice += response[i].totalPrice;
            }

            var totalPrice = 0;
            totalPrice += sumPrice;
            $('#total-price').text(totalPrice.toFixed(2));
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}


$('#order-button').click(function () {
    let input = {
        "userId": "0ba772f5-61a3-475e-9ba7-08db767cf11e"
    };

    let sumPrice = 0;

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/Order/create-order", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            GotoOrderPage();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
});

function GotoOrderPage() {
    window.location.href = "/Order/Index";
}