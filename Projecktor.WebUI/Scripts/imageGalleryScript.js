﻿function GalleryOne(imagestring, vignetteString)
{
    $("<img>", { src: vignetteString, id: "vignette" }).prependTo("#projecktor_lightbox");
    $("<img>", { src: imagestring, id: "centreImage" }).prependTo("#displayImage");
    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({
        "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647",
        "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)"
    });

    $("body").css({ "overflow": "hidden" })
    ImageCall();
}

function GalleryShowMany(images, vignetteString) {
    $("<img>", { src: vignetteString, id: "vignette" }).prependTo("#projecktor_lightbox");
    $("<img>", { src: images[2], id: "leftImage" }).prependTo("#displayImage");
    $("<img>", { src: images[0], id: "centreImage" }).prependTo("#displayImage");
    $("<img>", { src: images[1], id: "rightImage" }).prependTo("#displayImage");

    $("#vignette").css({ "position": "absolute", "width": "100%", "height": "100%", "left": "0px", "top": "0px" });
    $("#projecktor_lightbox").css({
        "position": "fixed", "top": "0px", "bottom": "0px", "left": "0px", "right": "0px", "z-index": "2147483647",
        "overflow": "hidden", "background-color": "rgba(17, 17, 17, 0.921569)"
    });

    $("body").css({ "overflow": "hidden" })
    ImageCall();
}


function GalleryMany(postNumber, vignetteString) {

    var postSplit = postNumber.split("-");
    $.ajax
   ({
       type: 'GET',
       url: 'gallery',
       data: { 'id': postSplit[0] },
       dataType: 'json',
       success: function (dataId) {
           GalleryShowMany(dataId, vignetteString)
       }
   })
}

$(document).ready(function() {
    $(window).bind("resize", function() {
        ImageFunc();
    });

    $('body').on('click', '#projecktor_lightbox', function () {
        $("#vignette").removeAttr("style");
        $("#projecktor_lightbox").removeAttr("style");
        $("#displayImage").removeAttr("style");
        $("body").removeAttr("style");
        $("#centreImage").remove();
        $("#leftImage").remove();
        $("#rightImage").remove();
        $('#vignette').remove();
    });
})


function ImageCall()
{
    var img = document.getElementById("centreImage");

    img.onload = function () {
        ImageFunc();
    }
}

function ImageFunc()
{
    var img = document.getElementById("centreImage");
    var imgLeft = document.getElementById("leftImage");
    var imgRight = document.getElementById("rightImage");

    var boxRatio = $(window).width() / $(window).height();

    $("#displayImage").css({ "position": "absolute", "left": "50%", "top": "50%" });

    if (imgLeft != null)
    {
        var aspectRatioLeft = imgLeft.width / imgLeft.height;
        var scaleFactorLeft;

        if (boxRatio > aspectRatioLeft) {
            scaleFactorLeft = $(window).height() / imgLeft.height;
        }
        else {
            scaleFactorLeft = $(window).width() / imgLeft.width;
        }

        var reduceHeightByLeft = (imgLeft.height * scaleFactorLeft / 100) * 15;
        var reduceWidthByLeft = (imgLeft.width * scaleFactorLeft / 100) * 15;

        var newHeightLeft = Math.min(imgLeft.height * scaleFactorLeft - reduceHeightByLeft, imgLeft.naturalHeight);
        var newWidthLeft = Math.min(imgLeft.width * scaleFactorLeft - reduceWidthByLeft, imgLeft.naturalWidth);

        var leftLeft = 0.42 * $(window).width();

        $("#leftImage").css({ "position": "absolute", "display": "inline-block", "height": newHeightLeft, "width": newWidthLeft, "left": leftLeft, "top": -newHeightLeft / 2 });
    }

    var aspectRatio = img.width / img.height;
    var scaleFactor;

    if (boxRatio > aspectRatio) {
        scaleFactor = $(window).height() / img.height;
    }
    else {
        scaleFactor = $(window).width() / img.width;
    }

    var reduceHeightBy = (img.height * scaleFactor / 100) * 15;
    var reduceWidthBy = (img.width * scaleFactor / 100) * 15;

    var newHeightCentre = Math.min(img.height * scaleFactor - reduceHeightBy, img.naturalHeight);
    var newWidthCentre = Math.min(img.width * scaleFactor - reduceWidthBy, img.naturalWidth);

    var leftCentre = -newWidthCentre / 2;


    $("#centreImage").css({ "position": "absolute", "display": "inline-block", "height": newHeightCentre, "width": newWidthCentre, "left": leftCentre, "top": -newHeightCentre / 2 });

    if (imgRight != null)
    {
        var aspectRatioRight = imgRight.width / imgRight.height;
        var scaleFactorRight;

        if (boxRatio > aspectRatioRight) {
            scaleFactorRight = $(window).height() / imgRight.height;
        }
        else {
            scaleFactorRight = $(window).width() / imgRight.width;
        }

        var reduceHeightByRight = (imgRight.height * scaleFactorRight / 100) * 15;
        var reduceWidthByRight = (imgRight.width * scaleFactorRight / 100) * 15;

        var newHeightRight = Math.min(imgRight.height * scaleFactorRight - reduceHeightByRight, img.naturalHeight);
        var newWidthRight = Math.min(imgRight.width * scaleFactorRight - reduceWidthByRight, img.naturalWidth);

        var leftRight = 0 - newWidthRight - 0.42 * $(window).width();

        $("#rightImage").css({ "position": "absolute", "display": "inline-block", "height": newHeightRight, "width": newWidthRight, "left": leftRight, "top": -newHeightRight / 2 });
    }


}