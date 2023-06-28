var OrderByParam = "asc";
var pageSize = 0;
var skip = 0;

function GetParfumeList() {
    var url = baseUrl + "odata/Parfume?";
    var searchText = $('#search').val();
    var brandSelection = Number($('#parfume-brands').val());

    sendSearchValue();

    if (OrderByParam == "asc") {
        url += "orderby=price asc"
    }
    else {
        url += "orderby=price desc"
    }

    if (searchText && brandSelection && brandSelection > 0) {
        url += "&filter=brandid eq " + brandSelection + " and contains(tolower(name),tolower('" + searchText + "'))";
    }
    else if (!searchText && brandSelection && brandSelection > 0) {
        url += "&filter=brandid eq " + brandSelection;
    }

    else if (searchText && ((brandSelection && brandSelection <= 0) || !brandSelection)) {
        url += "&filter=contains(tolower(name),tolower('" + searchText + "'))";
    }

    url += "&top=" + pageSize + "&$skip=" + skip + "&count=true";

    Get(url, {}, function (data) {
        var response = data.value;
        $('#parfume-row').html('');
        for (var i = 0; i < response.length; i++) {
            let d = response[i];
            $('#parfume-row').append(`<div class="col-sm-4 my-2">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title">${response[i].BrandName}</h5>
                            <p class="card-text">${response[i].Name}</p>
                            <p class="card-text">Fiyatı : ${response[i].Price}₺</p>
                            <p class="card-text"><input type="number" min="0" name="quantity-${response[i].Id}" class="form-control quantity-${response[i].Id}" placeholder="Sepete eklenecek miktar"/></p>
                            <a href="/Home/ParfumeDetail?parfumeId=${response[i].Id}" class="btn btn-primary">Detayı Görüntüle</a>
                            <button class="btn btn-primary float-end" onclick="AddToCart('${JSON.stringify(d).replaceAll('"', '\\\'')}')">Sepete Ekle</button>
                        </div>
                    </div>
                </div>`);
        }

    }, function (error) {
        toastr.error("Ürün bulunamadı."); //Error
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
            toastr.error("Parfüm detayı getirilemedi.");
        }
    }, function (error) {
        toastr.error("Parfüm detayı getirilemedi.");
    });
}

function updateParfume() {
    let parfumeId = GetQueryString();

    let name = $('#parfumeName').val();
    let price = Number($('#parfumePrice').val());

    if (!parfumeId || !name || !price) {
        toastr.error("Boş alanları doldurun.");
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
            toastr.success("İşlem başarılı.");
            window.location.href = "/";
        }
        else {
            toastr.error("Parfüm güncellenemedi.");
        }
    }, function (error) {
        toastr.error("Parfüm güncellenemedi.");
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
            toastr.success("İşlem başarılı.");
            window.location.href = "/";
        }
        else {
            toastr.error("Parfüm silinemedi.");
        }
    }, function (error) {
        toastr.error("Parfüm silinemedi.");
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
            toastr.error("Markalar getirilemedi.");
        }
    }, function (error) {
        toastr.error("Markalar getirilemedi.");
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
        toastr.error("Boş alanları doldurun.");
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
            toastr.success("İşlem başarılı.");
            window.location.href = "/";
        }
        else {
            toastr.error("Parfüm kaydedilemedi.");
        }
    }, function (error) {
        toastr.error("Parfüm kaydedilemedi.");
    });
}

function AddToCart(requestInput) {
    var inp = JSON.parse(requestInput.replaceAll('\'','"'));
    var userId = "0ba772f5-61a3-475e-9ba7-08db767cf11e";
    var brandName = inp.BrandName;
    var perfumeName = inp.Name;
    var price = Number(inp.Price);
    var newQuantity = Number($(`.quantity-${inp.Id}`).val() || 1);

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

    console.log(input);

    let obj = JSON.stringify(input, null, 2);

    Post(baseUrl + "api/CartItems/save-or-update-cart-item", obj, function (data) {
        if (data.isSuccess) {
            toastr.success("İşlem başarılı.");
            GotoCartPage();
        }
        else {
            toastr.error("Sepete eklenecek miktar girin."); //Error
        }
    }, function (error) {
        toastr.error("Sepete eklenecek miktar girin."); //Error
    });
}

function GotoCartPage() {
    window.location.href = "/Cart/Index";
}

function GetFilterBrands() {
    Post(baseUrl + "api/brand/get-all-brands", {}, function (data) {
        if (data.isSuccess) {
            var response = data.data;
            $('#parfume-brands').html('');
            $('#parfume-brands').append(`<option value="-1">Tüm Markalar</option>`);
            for (var i = 0; i < response.length; i++) {
                $('#parfume-brands').append(`<option value="${response[i].id}">${response[i].name}</option>`);
            }
        }
        else {
            toastr.error("Filtre bölümündeki markalar getirilemedi.");
        }
    }, function (error) {
        toastr.error("Filtre bölümündeki markalar getirilemedi.");
    });
}


function ListOrder(sortType) {
    switch (sortType) {
        case "asc":
            OrderByParam = "asc";
            $('#dropdownMenu2').html("Fiyata göre artan");
            GetParfumeList();
            break;
        case "desc":
            OrderByParam = "desc";
            $('#dropdownMenu2').html("Fiyata göre azalan");
            GetParfumeList();
            break;
        default:
            OrderByParam = "asc";
            $('#dropdownMenu2').html("Fiyata göre artan");
            GetParfumeList();
            break;
    }
}


function sendSearchValue() {
    var searchValue = $('#search').val();
    var brandSelection = Number($('#parfume-brands').val());

    var url = "http://localhost:5025/Home/GetFilterData?searchText=" + searchValue + "&orderBy=" + OrderByParam + "&brandId=" + brandSelection;

    Get(url, {}, function (data) {
        /*toastr.success(data);*/
    }, function (errorThrown) {
        toaster.error("Hatalı");
    });
}