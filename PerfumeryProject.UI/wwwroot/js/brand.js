﻿function GetBrandList() {
    ShowTableBrand();
    Post(baseUrl + "api/brand/get-all-brands", {}, function (data) {
        if (data.isSuccess) {
            var response = data.data;
            $('#brand-list').html('');
            for (var i = 0; i < response.length; i++) {
                $('#brand-list').append(`
                <tr>
                <td>${response[i].name}</td>
                <td><button class="btn btn-primary mx-2" onclick="GetBrandInfo(${response[i].id})">Güncelle</button> <button class="btn btn-primary mx-2" onclick="DeleteBrand(${response[i].id})">Sil</button> </td>
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

function ShowAddBrand() {
    $('#table-row').hide();
    $('#update-row').hide();
    $('#insert-row').show();
}

function ShowTableBrand() {
    $('#table-row').show();
    $('#insert-row').hide();
    $('#update-row').hide();
}

function ShowUpdateBrand() {
    $('#table-row').hide();
    $('#insert-row').hide();
    $('#update-row').show();
}

$('#add-brand-button').on('click', function () {
    InsertBrand();
});


$('.back-brand-button').on('click', function () {
    ShowTableBrand();
});


function InsertBrand() {
    let input = {
        "name": $('#brand-name').val()
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/brand/add-brand", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            GetBrandList();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function DeleteBrand(id) {
    let input = {
        "id": id
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/brand/delete-brand", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            GetBrandList();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function GetBrandInfo(id) {
    let input = {
        "id": id
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/brand/get-brand-by-id", obj, function (data) {
        if (data.isSuccess) {
            var response = data.data;
            $('#update-container').html('');
            $('#update-container').append(`
                    <div class="form-group">
                        <label class="control-label col-sm-3">Marka Adı:</label>
                        <div class="col-sm-9">
                            <input id="update-brand-name" type="text" class="form-control" value="${response.name}" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-3 col-sm-9 my-2">
                            <button id="update-brand-button" type="button" class="btn btn-primary" onclick="UpdateBrand(${response.id})">Güncelle</button>
                            <button type="button" class="btn btn-secondary back-brand-button" onclick="ShowTableBrand()">Listeye Geri Dön</button>
                        </div>
                    </div>
                `);

            ShowUpdateBrand();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}

function UpdateBrand(id) {
    let input = {
        "id": id,
        "name": $('#update-brand-name').val()
    };

    let obj = JSON.stringify(input, null, 2);
    Post(baseUrl + "api/brand/update-brand", obj, function (data) {
        if (data.isSuccess) {
            console.log("İşlem başarılı.");
            GetBrandList();
        }
        else {
            console.log("error");
        }
    }, function (error) {
        console.log(error); //Error
    });
}