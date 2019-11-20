
$(document).ready(function () {
    console.log("Siden kjører");
    $.ajax({
        type: 'GET',
        url: '/Home/hentDestinasjon',
        dataType: 'json',
        success: function (avgangerFra) {
            dropdownDestinasjonFra(avgangerFra);
        }

    })

    $.ajax({
        type: 'GET',
        url: '/Home/hentDestinasjon',
        dataType: 'json',
        success: function (avgangerTil) {
            dropdownDestinasjonTil(avgangerTil);
        }

    })

    $("#dropdownFra, #dropdownTil").change(function () {
        if ($("#dropdownFra").val() == null || $("#dropdownTil").val() == null) {
            console.log("De er null");
            $("#avganger").html("");
        }
        else if ($("#dropdownFra").val() == $("#dropdownTil").val()) {
            console.log("De er like");
            $("#avganger").html("");
        }
        else {
            console.log("Ajax kall");
            $.ajax({
                url: '/Home/hentAvganger/',
                type: 'GET',
                dataType: 'json',
                data: 'destFra=' + $("#dropdownFra").val() + '&destTil=' + $("#dropdownTil").val() + '&tid=' + 00,
                success: function (avganger) {
                    listAvganger(avganger);
                }
            });
        }
    });

});


function listAvganger(avganger, i) {
    var utStreng = "";
    utStreng += "<table class='table w-75 mx-auto mt-5 table-hover tabellList'>"
    utStreng += "<thead><tr><th>ID</th><th>Fra</th>"
    utStreng += "<th>Til</th> <th>Avgang</th> <th>Ankomst</th> <th></th> <th></th> </tr></thead>"

    for (var i in avganger) {
        utStreng += "<tr>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='id" + i + "'size='15' value='" + avganger[i].id + "'/></td>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='destinasjonFra" + i + "'size='15' value='" + avganger[i].destinasjonFra + "'/></td>"
        utStreng += "<td><input type='text' readonly class='form-control-plaintext' id='destinasjonTil" + i + "'size='15' value='" + avganger[i].destinasjonTil + "'/></td>"
        utStreng += "<td><input type='text' id='avgang" + i + "'size='15' value='" + avganger[i].tid + "' onchange='validerTid( " + i + "); validerAvgang(" + i + ")' onkeydown='validerTid( " + i + "); validerAvgang(" + i +")' maxlength='5'/></td>"
        utStreng += "<td><input type='text' id='ankomst" + i + "'size='15' value='" + avganger[i].ankomst + "'  onchange='validerAnkomst( " + i + "); validerAvgang(" + i + ")' onkeydown='validerAnkomst( " + i + "); validerAvgang(" + i +")' maxlength='5'/></td>"
        utStreng += "<td><button class='btn btn-dark' id='alertEndre"+i+"' href='#bekrefteModal' data-toggle='modal' onclick='modalEndreAvganger(" + avganger[i].id + ", " + i + ")'>Endre</button></td>"
        utStreng += "<td><button id='slett" + i + "' class='btn btn-danger' href='#bekrefteModal' data-toggle='modal' onclick=' modalSlettAvganger(" + avganger[i].id + ", " + i + ")'>Slett</button></td>"
        utStreng += "</tr>"
    }
    utStreng += "</table>"

    $("#avganger").html(utStreng);
}

function endreAvgang(id, i) {
    var avgang = $("#avgang" + i).val();
    var ankomst = $("#ankomst" + i).val();

    $.ajax({
        type: 'GET',
        url: '/Home/endreAvgang/',
        data: 'id=' + id + '&destFra=' + $("#dropdownFra").val() + '&destTil=' + $("#dropdownTil").val() + '&avgang=' + avgang + '&ankomst=' + ankomst,
        dataType: 'json',
        success: function (avganger) {
            console.log("Fullført endring");
            window.location.href = "/Home/endreAvganger";
        }
    });
}

function slettAvgang(id, i) {
    var avgang = $("#avgang" + i).val();
    var ankomst = $("#ankomst" + i).val();

    $.ajax({
        type: 'GET',
        url: '/Home/slettAvgang',
        data: 'id=' + id + '&destFra=' + $("#dropdownFra").val() + '&destTil=' + $("#dropdownTil").val(),
        dataType: 'json',
        success: function (avganger) {
            console.log("Fullført slett");
            window.location.href = "/Home/endreAvganger"
        }
    });
}

function dropdownDestinasjonFra(avgangerFra)   {
    var utstreng = '<option disabled selected>Avgang fra</option>';
    for (var i in avgangerFra) {
        utstreng += '<option value="' + avgangerFra[i].id + '">' + avgangerFra[i].sted + '</option>';
    }
    $("#dropdownFra").html(utstreng);

}

function dropdownDestinasjonTil(avgangerTil) {
    var utstreng = '<option disabled selected>Avgang til</option>';
    for (var i in avgangerTil) {
        utstreng += '<option value="' + avgangerTil[i].id + '">' + avgangerTil[i].sted + '</option>';
    }
    $("#dropdownTil").html(utstreng);

}

function modalEndreAvganger(id, i) {
    var destinasjonFra = $("#destinasjonFra" + i).val();
    var destinasjonTil = $("#destinasjonTil" + i).val();
    var tid = $("#avgang" + i).val();
    var ankomst = $("#ankomst" + i).val();

    var utStreng = "Ønsker du å endre avgangen <strong>" + destinasjonFra + " - " + destinasjonTil + "</strong> <br/> til den nye tiden: <strong>Kl:" + tid + " - " + ankomst + "</strong>";

    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        endreAvgang(id, i);
    });
}

function modalSlettAvganger(id, i) {
    var destinasjonFra = $("#destinasjonFra" + i).val();
    var destinasjonTil = $("#destinasjonTil" + i).val();
    var tid = $("#avgang" + i).val();
    var ankomst = $("#ankomst" + i).val();

    var utStreng = "Ønsker du å slette avgangen <strong>" + destinasjonFra + " - " + destinasjonTil + ", kl:" + tid + " - " + ankomst + "</strong>";

    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        slettAvgang(id, i);
    });
}



/*----------------------- VALIDERING -----------------------------*/


function validerVelgDest() {
    var sjekkFra = $("#dropdownFra").val();
    var sjekkTil = $("#dropdownTil").val();

    if (sjekkFra == sjekkTil || sjekkFra == null || sjekkTil == null) {
        $(".alertVelgAvgang").css("display", "block");
        $(".alertVelgAvgangText").html("Du har valgt ugyldig destinasjoner!");
    } else {
        $(".alertVelgAvgang").css("display", "none");
    }
}


function validerTid(i) {
    var regEx = /^[0-9]{2}:[0-9]{2}$/;
    OK = regEx.test(document.getElementById('avgang' + i).value);

    if (!OK) {
        document.getElementById('avgang' + i).style.background = "#ffccc9";
        document.querySelector('.alertListAvgang').style.display = "block";
        document.querySelector('.alertListAvgangText').innerHTML = "Tid må skrive f.eks slik: TT:MM!";
        return false;

    } else {
        document.getElementById('avgang' + i).style.background = "#ffffff";
        return true;
    }
};

function validerAnkomst(i) {
    var regEx = /^[0-9]{2}:[0-9]{2}$/;
    OK = regEx.test(document.getElementById('ankomst' + i).value);

    if (!OK) {
        document.getElementById('ankomst' + i).style.background = "#ffccc9";
        document.querySelector('.alertListAvgang').style.display = "block";
        document.querySelector('.alertListAvgangText').innerHTML = "Ankomst må skrive f.eks slik: TT:MM!";
        return false;

    } else {
        document.getElementById('ankomst' + i).style.background = "#ffffff";
        return true;
    }
};


function validerAvgang(i) {
    var tidOK = validerTid(i);
    var ankomstOK = validerAnkomst(i);

    if (tidOK && ankomstOK) {
        document.getElementById('alertEndre' + i).disabled = false;
        document.querySelector('.alertListAvgang').style.display = "none";
        return true;
    } else {
        document.getElementById('alertEndre' + i).disabled = true;
        return false;
    }
}














