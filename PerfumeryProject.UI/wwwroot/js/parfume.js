function GetParfumeList() {
    Post(baseUrl + "api/parfume/get-all-parfumes", {}, function (data) {
        if (data.isSuccess) {
            var response = data.data;
            $('#parfume-row').html('');
            for (var i = 0; i < response.length; i++) {
                let d = response[i];
                $('#parfume-row').append(`<div class="col-sm-4 my-2">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title">${response[i].brandName}</h5>
                            <p class="card-text">${response[i].name}</p>
                            <p class="card-text">Fiyatı : ${response[i].price}₺</p>
                            <p class="card-text"><input type="number" min="0" name="quantity-${response[i].id}" class="form-control quantity-${response[i].id}" placeholder="Eklenecek ürün miktarı"/></p>
                            <a href="/Home/ParfumeDetail?parfumeId=${response[i].id}" class="btn btn-primary">Detayı Görüntüle</a>
                            <button class="btn btn-primary float-end" onclick="AddToCart('${JSON.stringify(d).replaceAll('"', '\\\'')}')">Sepete Ekle</button>
                        </div>
                    </div>
                </div>`);
            }
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GetParfumeDetail() {
    let parfumeId = GetQueryString();

    let input = {
        "id": parfumeId
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/parfume/get-parfume", obj, function (data) {
        if (data.isSuccess) {
            var response = data.data;
            $('#parfume-detail-row').html('');
            $('#parfume-detail-row').append(`<div class="col-6 mx-auto my-2">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title">${response.brandName}</h5>
                            <input type="text" name="parfumeName" id="parfumeName" value="${response.name}" class="form-control mt-1" />
                            <input type="number" min="0" name="parfumePrice" id="parfumePrice" value="${response.price}" class="form-control my-2" />
                            <a onclick="updateParfume(${response.id})" class="btn btn-success">Güncelle</a>
                            <a onclick="deleteParfume(${response.id})" class="btn btn-danger">Sil</a>
                            <button onclick="goToHomepage()" type="button" class="btn btn-secondary back-parfume-button">Listeye Geri Dön</button>
                        </div>
                    </div>
                </div>`);
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function updateParfume() {
    let parfumeId = GetQueryString();

    let name = $('#parfumeName').val();
    let price = Number($('#parfumePrice').val());

    if (!parfumeId || !name || !price) {
        console.log("Boş alanlar var.");
        return;
    }

    let input = {
        "id": parfumeId,
        "name": name,
        "price": price
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/parfume/update-parfume", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            window.location.href = "/";
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function deleteParfume() {
    let parfumeId = GetQueryString();

    let input = {
        "id": parfumeId
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/parfume/delete-parfume", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            window.location.href = "/";
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GetQueryString() {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
    let parfumeId = params.parfumeId; // "some_value"
    return parfumeId;
}

function GetAllBrands() {
    Post(baseUrl + "api/brand/get-all-brands", {}, function (data) {
        if (data.isSuccess) {
            var response = data.data;
            $('#brand-select').html('');
            for (var i = 0; i < response.length; i++) {
                $('#brand-select').append(`<option value="${response[i].id}">${response[i].name}</option>`);
            }
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GotoAddParfume() {
    window.location.href = "/Home/AddParfume";
}

$('#back-button').on('click', function () {
    window.location.href = "/";
});

$('#add-button').on('click', function () {
    AddParfume();
});

function goToHomepage() {
    window.location.href = "/";
};

function AddParfume() {
    let brandId = Number($('#brand-select').val());
    let name = $('#name-input').val();
    let price = $('#price-input').val();

    if (!brandId || !name || !price) {
        console.log("Boş alanlar var.");
        return;
    }

    let input = {
        "brandId": brandId,
        "name": name,
        "price": price
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/parfume/create-parfume", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            window.location.href = "/";
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function AddToCart(requestInput) {
    var inp = JSON.parse(requestInput.replaceAll('\'','"'));
    console.log(inp);
    console.log(`.quantity-${inp.id}`);
    var userId = "0ba772f5-61a3-475e-9ba7-08db767cf11e";
    var brandName = inp.brandName;
    var perfumeName = inp.name;
    var price = Number(inp.price);
    var newQuantity = Number($(`.quantity-${inp.id}`).val());

    var input = {
        "userId": userId,
        "brandName": brandName,
        "price": price,
        "quantity": newQuantity,
        "isOrdered": false,
        "cartNumber": 0,
        "createdDate": new Date(),
        "name": perfumeName
    };

    let obj = JSON.stringify(input, null, 2);
    console.log(input);

    Post(baseUrl + "api/CartItems/save-or-update-cart-item", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            GotoCartPage();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GotoCartPage() {
    window.location.href = "/Cart/Index";
}