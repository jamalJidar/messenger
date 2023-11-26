

function progressBarFunction(val) {
    if (val === 0) return
    const CircularProgressBlack1 = document.getElementById("CircularProgressBlack");
    const progressValue = CircularProgressBlack1.querySelector(".percentage");
    const innerCircle = CircularProgressBlack1.querySelector(".inner-circle");

    let startValue = 0,
        endValue = val,
        speed = 10,
        progressColor = CircularProgressBlack1.getAttribute("data-progress-color");
    progressValue.textContent = `${val}%`;
    progressValue.style.color = `${progressColor}`;
    innerCircle.style.backgroundColor = `${CircularProgressBlack1.getAttribute(
        "data-inner-circle-color"
    )}`;

    CircularProgressBlack1.style.background = `conic-gradient(${progressColor} ${val * 3.6
        }deg,${CircularProgressBlack1.getAttribute("data-bg-color")} 0deg)`;
    if (val >= 100) {
        clearInterval(intervalId);
    }
}
progressBarFunction(0)
function uploadFiles(inputId) {

    var input = document.getElementById(inputId);

    let Caption = document.getElementById('Caption').value;
    var files = input.files;
    var formData = new FormData();
    formData.append("caption", Caption);
    formData.append("recvId", CurrentUser);
    for (var i = 0; i != files.length; i++) {
        GetFileName(files[i])
        formData.append("files", files[i]);
    }

  

    startUpdatingProgressIndicator();
    $.ajax(
        {
            url: "/uploader",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
                stopUpdatingProgressIndicator();

            }
        }
    );
}

var intervalId;
function startUpdatingProgressIndicator() {
    $("#progress").show();

    intervalId = setInterval(
        function () {
            // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
            $.post(
                "/uploader/progress", function (progress) {
                    progressBarFunction(progress)
                });
        }, 10);
}

function stopUpdatingProgressIndicator() {
    clearInterval(intervalId);
}

function GetFileName(file) {
    console.log(file.name);
    //return file.filename.split('.').pop();
}
document.getElementById("btnCanselSendFile").addEventListener('click', function () {


    $.post(
        "/uploader/CanselSendFile", function (progress) {
            progressBarFunction(progress)
        });

})