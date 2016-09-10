﻿var pageSize = 10;
var pageIndex = 1;

function GetPosts(url, showpost) {
    $.ajax
    ({
        type: 'GET',
        url: url,
        data: { 'pageIndex': JSON.stringify(pageIndex), 'pageSize': JSON.stringify(pageSize) },
        dataType: 'json',
        async: 'false',
        success: function (dataId) {
            $.post(showpost, $.param({ data: dataId }, true), function (data) {
                $('#container').append(data);
            })
            pageIndex++;
        },
        beforeSend: function () {
            $("#progress").show();
        },
        complete: function () {
            $("#progress").hide();
        },
    })
}