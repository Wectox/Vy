$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/hentLogg',
        dataType: 'json',
        success: function (logg) {
            listLogg(logg);
        }
    });
});

function listLogg(logg, i) {
    var utStreng = "";
    utStreng += "<table class='table w-75 mx-auto mt-5 table-hover tabellList'>"
    utStreng += "<thead><tr><th>ID</th><th>Tabell</th>"
    utStreng += "<th>Endret</th> <th>Beskrivelse</th> <th></th> <th></th> </tr></thead>"

    for (var i in logg) {
        utStreng += "<tr>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='id" + i + "'size='1' value='" + logg[i].id + "'/></td>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='id" + i + "'size='5' value='" + logg[i].tabell + "'/></td>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='id" + i + "'size='5' value='" + logg[i].sisteEndret + "'/></td>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='beskrivelse" + i + "'size='15' value='" + logg[i].beskrivelse + "'/></td>"
        utStreng += "<td><button  class='btn btn-dark' href='#detaljeModal' data-toggle='modal' onclick='modalDetaljeLogg(" + logg[i].id + ", " + i + ")'>Detaljer</button></td>"
        utStreng += "<td><button id='slett" + i + "' class='btn btn-danger' href='#bekrefteModal' data-toggle='modal' onclick='modalSlettLogg(" + logg[i].id + ", " + i + ")'>Slett</button></td>"
        utStreng += "</tr>"
    }
    utStreng += "</table>"

    $("#visLogg").html(utStreng);
}

function modalSlettLogg(id, i) {
    var utStreng = "Er du sikker på at du vil slette loggen med id: " + id;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        slettLogg(id, i);
    });
}


function modalDetaljeLogg(id, i) {
    var beskrivelse = $("#beskrivelse" + i).val();
    $(".modalHere").html(beskrivelse);

    $("#btnBekreftModal").click(function () {
        slettLogg(id, i);
    });
}

function slettLogg(id, i) {

    $("#slett" + i).attr("disabled", true);
    $.ajax({
        type: 'GET',
        url: '/Home/slettLogg',
        data: 'id=' + id,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/loggSide";
        }
    });
}