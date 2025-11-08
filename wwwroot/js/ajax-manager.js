var AjaxManager = {

    HttpPost: function (serviceUrl, jsonParams, successCallback, errorCallback) {
        $.ajax({
            type: "POST",
            url: serviceUrl,
            contentType: "application/json",
            data: JSON.stringify(jsonParams),
            success: successCallback,
            error: errorCallback
        });
    },

    HttpDelete: function (serviceUrl, jsonParams, successCallback, errorCallback) {
        $.ajax({
            url: serviceUrl + "?" + $.param(jsonParams),
            data: null,

            contentType: 'application/json; charset=utf-8',
            type: "DELETE",
            success: successCallback,
            error: errorCallback
        });
    },

    HttpGet: function (serviceUrl, jsonParams, successCalback, errorCallback) {
        $.ajax({
            cache: false,
            async: true,
            type: "GET",
            url: serviceUrl,
            data: jsonParams,
            success: successCalback,
            error: errorCallback
        });

    },

}