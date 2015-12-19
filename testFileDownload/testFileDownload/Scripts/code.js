$(document).ready(function () {
    debugger;
    $('#button').on('click', function () {
        $.fileDownload('/Home/Download', {
            successCallback: function (url) {

                alert('You just got a file download dialog or ribbon for this URL :' + url);
            }
        });
    })
})