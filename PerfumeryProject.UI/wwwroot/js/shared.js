function Post(url, input, success, error) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: input,
        success: function (data, textStatus, jqXHR) {
            success(data, textStatus, jqXHR);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            error(jqXHR, textStatus, errorThrown);
        }
    });
}


function Get(url, input, success, error) {
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: input,
        success: function (data, textStatus, jqXHR) {
            success(data, textStatus, jqXHR);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            error(jqXHR, textStatus, errorThrown);
        }
    });
}