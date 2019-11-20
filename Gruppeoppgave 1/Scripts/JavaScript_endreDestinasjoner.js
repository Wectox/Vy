$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/hentDestinasjon',
        dataType: 'json',
        success: function (destinasjon) {
            listDestinasjoner(destinasjon);
        }
    });

    $("#leggTilKnapp").attr("disabled", true);
});

function listDestinasjoner(destinasjon, i) {
    var utStreng = "";
    utStreng += "<table class='table w-75 mx-auto mt-5 table-hover tabellList'>"
    utStreng += "<thead><tr><th>ID</th><th>Sted</th>"
    utStreng += "<th>Sone</th> <th></th> <th></th> </tr></thead>"

    for (var i in destinasjon) {
        utStreng += "<tr>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='id" + i + "'size='15' value='" + destinasjon[i].id + "'/></td>"
        utStreng += "<td><input type='text' id='sted" + i + "' size='15' value='" + destinasjon[i].sted + "' onchange='validerSted( " + i + "); validerListDest(" + i + ")' onkeydown='validerSted( " + i + "); validerListDest(" + i +")' maxlength='20'/></td>"
        utStreng += "<td><input type='text' id='sone" + i + "' size='15' value='" + destinasjon[i].sone + "'  onchange='validerSone( " + i + "); validerListDest(" + i + ")' onkeydown='validerSone( " + i + "); validerListDest(" + i +")' maxlength='2'/></td>"
        utStreng += "<td><button class='btn btn-dark' id='alertEndre" + i + "' href='#bekrefteModal' data-toggle='modal' onclick='modalEndreDestinasjon(" + destinasjon[i].id + ", " + i + ")'>Endre</button></td>"
        utStreng += "<td><button id='slett" + i + "' class='btn btn-danger' href='#bekrefteModal' data-toggle='modal' onclick='modalSlettDestinasjon(" + destinasjon[i].id + ", " + i + ")'>Slett</button></td>"
        utStreng += "</tr>"
    }
    utStreng += "</table>"

    $("#leggTilKnapp").attr("disabled", false);
    if (i != 0) $("#slett" + i).attr("disabled", false);
    $("#visDestinasjoner").html(utStreng);
}

function endreDestinasjoner(id, i) {
    var sted = $("#sted" + i).val();
    var sone = $("#sone" + i).val();

    $.ajax({
        type: 'GET',
        url: '/Home/endreDestinasjoner',
        data: 'id=' + id + '&sted=' + sted + '&sone=' + sone,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/endreDestinasjon";
        }
    });

}

function slettDestinasjon(id, i) {

    $("#slett"+i).attr("disabled", true);
    $.ajax({
        type: 'GET',
        url: '/Home/slettDestinasjon',
        data: 'id=' + id,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/endreDestinasjon";
        }
    });

}

function modalEndreDestinasjon(id, i) {
    var sted = $("#sted" + i).val();
    var sone = $("#sone" + i).val();

    var utStreng = "";
    utStreng += "Ønsker du å endre denne destinasjonen til: </br>";
    utStreng += sted + ", " + sone;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        endreDestinasjoner(id, i);
    });
}

function modalSlettDestinasjon(id, i) {
    var sted = $("#sted" + i).val();
    var sone = $("#sone" + i).val();

    var utStreng = "";
    utStreng += "Ønsker du å slette denne destinasjonen: </br>";
    utStreng += sted + ", " + sone;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        slettDestinasjon(id, i);
    });
}

function nyAvganger() {
    var nySone = $("#nySone").val();
    var nySted = $("#nySted").val();
    var i = 0;
    console.log("Nysted: " + nySted);
    console.log("Nysone: " + nySone);
    $("#leggTilKnapp").attr("disabled", true);

    $.ajax({
        type: 'GET',
        url: '/Home/nyDestinasjon',
        data: 'nySted=' + nySted + '&nySone=' + nySone,
        dataType: 'json',
        success: function (destinasjon) {
            $("#melding").html("Ny destinasjon lagt til!");
            listDestinasjoner(destinasjon, i);
        }
    });
}



/*----------------------- VALIDERING -----------------------------*/

function validerNySted() {
    var regEx = /^[a-zæøåA-ZÆØÅ]{2,30}$/;
    OK = regEx.test(document.getElementById('nySted').value);

    if (!OK) {
        document.getElementById('nySted').style.background = "#ffccc9";
        document.querySelector('.alertSettInnDestinasjon').style.display = "block";
        document.querySelector('.alertSettInnDestinasjonText').innerHTML = "Sted kan bare inneholde bokstaver!";
        return false;

    } else {
        document.getElementById('nySted').style.background = "#ffffff";
        return true;
    }
};

function validerNySone() {
    var regEx = /^[0-9]{1,2}$/;
    OK = regEx.test(document.getElementById('nySone').value);

    if (!OK) {
        document.getElementById('nySone').style.background = "#ffccc9";
        document.querySelector('.alertSettInnDestinasjon').style.display = "block";
        document.querySelector('.alertSettInnDestinasjonText').innerHTML = "Sone kan kun inneholde maks 2 siffer!";
        return false;

    } else {
        document.getElementById('nySone').style.background = "#ffffff";
        return true;
    }
};



function validerSettInnDest() {
    var nyStedOK = validerNySted();
    var nySoneOK = validerNySone();

    if (nyStedOK && nySoneOK) {
        document.getElementById('leggTilKnapp').disabled = false;
        document.querySelector('.alertSettInnDestinasjon').style.display = "none";
        return true;
    } else {
        document.getElementById('leggTilKnapp').disabled = true;
        return false;
    }
}


function validerSted(i) {
    var regEx = /^[a-zæøåA-ZÆØÅ]{2,30}$/;
    OK = regEx.test(document.getElementById('sted' + i).value);

    if (!OK) {
        document.getElementById('sted'+i).style.background = "#ffccc9";
        document.querySelector('.alertListDestinasjon').style.display = "block";
        document.querySelector('.alertListDestinasjonText').innerHTML = "Sted kan bare inneholde bokstaver!";
        return false;

    } else {
        document.getElementById('sted'+i).style.background = "#ffffff";
        return true;
    }
};

function validerSone(i) {
    var regEx = /^[0-9]{1,2}$/;
    OK = regEx.test(document.getElementById('sone' + i).value);

    if (!OK) {
        document.getElementById('sone' + i).style.background = "#ffccc9";
        document.querySelector('.alertListDestinasjon').style.display = "block";
        document.querySelector('.alertListDestinasjonText').innerHTML = "Sone kan kun inneholde maks 2 siffer!";
        return false;

    } else {
        document.getElementById('sone' + i).style.background = "#ffffff";
        return true;
    }
};




function validerListDest(i) {
    var stedOK = validerSted(i);
    var soneOK = validerSone(i);

    if (stedOK && soneOK) {
        document.getElementById('alertEndre'+i).disabled = false;
        document.querySelector('.alertListDestinasjon').style.display = "none";
        return true;
    } else {
        document.getElementById('alertEndre' + i).disabled = true;
        return false;
    }
}













