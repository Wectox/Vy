//Henter bestillinger og admin når siden lastes
$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/getBestillinger',
        dataType: 'json',
        success: function (bestillinger) {
            listBestillinger(bestillinger);
        }
    });

    $.ajax({
        type: 'GET',
        url: '/Home/getAdmin',
        dataType: 'json',
        success: function (admin) {
            listAdmin(admin);
        }
    });

});

/*----------------------------- LIST DATA -----------------------------*/

//Denne funksjonen lister opp bestillinger på siden
function listBestillinger(bestillinger) { 
    var utStreng = "";
    utStreng += "<table class='table table-hover w-75 tabellList text-center'>"
    utStreng += " <thead> <tr> <th>#</th> <th>Envei ID</th><th>Retur ID</th><th>Dato</th><th>Dato Retur</th>"
    utStreng += "<th>Antall Reisende</th><th>Total Pris</th><th> </th><th> </th></tr> </thead>"

    for (var i in bestillinger) {
        utStreng += "<tr>"
        utStreng += "<th scope='row'>" + (parseFloat(i)+1) + "</th>"
        utStreng += "<td><input type='text' class='form-control-plaintext readOnly' readonly id='enVei" + i + "' value='" + bestillinger[i].enVei + "'/></td>"
        utStreng += "<td><input type='text' class='form-control-plaintext readOnly' readonly id='turRetur" + i + "'  value='" + bestillinger[i].turRetur + "'/></td>"
        utStreng += "<td><input type='text' class='inputText' id='dato" + i + "' value='" + bestillinger[i].dato + "' onchange='validerDato( " + i + "); validerBestilling(" + i + ")' onkeydown='validerDato( " + i + "); validerBestilling(" + i +")' maxlength='10'  /></td>"
        utStreng += "<td><input type='text' class='inputText' id='datoRetur" + i + "' value='" + bestillinger[i].datoRetur + "' onchange='validerReturDato( " + i + "); validerBestilling(" + i + ")' onkeydown='validerReturDato( " + i + "); validerBestilling(" + i +")' maxlength='10'/></td>"
        utStreng += "<td><input type='text' class='inputText' id='antallReisende" + i + "' value='" + bestillinger[i].antallReisende + "' onchange='validerAntallReisende( " + i + "); validerBestilling(" + i + ")' onkeydown='validerAntallReisende( " + i + "); validerBestilling(" + i +")' maxlength='6'/></td>"
        utStreng += "<td><input type='text' class='inputText' id='totalPris" + i + "' value='" + bestillinger[i].totalPris + "' onchange='validerTotalPris( " + i + "); validerBestilling(" + i + ")' onkeydown='validerTotalPris( " + i + "); validerBestilling("+i+")' maxlength='6'/></td>"
        utStreng += "<td><button type='button' id='btnEndreBestilling"+i+"' href='#bekrefteModal' data-toggle='modal' class='btn btn-dark' onclick='modalEndreBestilling(" + bestillinger[i].id + ", " + i + ")'>Endre</a></td>" 
        utStreng += "<td> <button type='button' href='#bekrefteModal' data-toggle='modal' class='btn btn-danger' onclick='modalSlettBestilling(" + bestillinger[i].id + "," + i + ")'> Slett </button></td>" 
        utStreng += "</tr>"
        console.log(i);
    }
    utStreng += "</table>"

    $("#visBestillinger").html(utStreng);
}

//Denne funksjonen lister opp admin-data på siden
function listAdmin(admin) {
    var utStreng = "";
    utStreng += "<table class='table table-hover w-75 tabellList text-center'>"
    utStreng += "<thead> <tr> <th>#</th> <th>Fornavn</th><th>Etternavn</th><th>Telefon</th>"
    utStreng += "<th>Epost</th> <th> </th><th> </th></tr> </thead>"

    for (var i in admin) {
        utStreng += "<tr>"
        utStreng += "<th scope='row'>" + (parseFloat(i) + 1) + "</th>"
        utStreng += "<td><input type='text' class='inputText' id='fornavn" + i + "' value='" + admin[i].Fornavn + "' onchange='validerFornavn( " + i + "); validerAdmin(" + i + ")' onkeydown='validerFornavn( " + i + "); validerAdmin(" + i +")' maxlength='20'/></td>"
        utStreng += "<td><input type='text' class='inputText' id='etternavn" + i + "' value='" + admin[i].Etternavn + "' onchange='validerEtternavn( " + i + "); validerAdmin(" + i + ")' onkeydown='validerEtternavn( " + i + "); validerAdmin(" + i +")' maxlength='20'/></td>"
        utStreng += "<td><input type='text' class='inputText' id='telefon" + i + "' value='" + admin[i].Telefon + "' onchange='validerTelefon( " + i + "); validerAdmin(" + i + ")' onkeydown='validerTelefon( " + i + "); validerAdmin(" + i +")' maxlength='8'/></td>"
        utStreng += "<td><input type='text' class='inputText' id='epost" + i + "' value='" + admin[i].Epost + "' onchange='validerEpost( " + i + "); validerAdmin(" + i + ")' onkeydown='validerEpost( " + i + "); validerAdmin(" + i +")'/></td>"
        utStreng += "<td><button type='button' id='btnEndreAdmin" + i +"' href='#bekrefteModal' data-toggle='modal' class='btn btn-dark' onclick='modalEndreAdmin(" + admin[i].id + ", " + i + ")'>Endre</a></td>"
        utStreng += "<td> <button type='button' href='#bekrefteModal' data-toggle='modal' class='btn btn-danger' onclick='modalSlettAdmin(" + admin[i].id + "," + i + ")'> Slett </button></td>"
        utStreng += "</tr>"
    }

    utStreng += "</table>"

    $("#visAdmin").html(utStreng);
    
}



/*----------------------- ENDRE OG SLETTE FUNKSJON -----------------------------*/


function endreBestilling(id, i) {

    //Henter verdiene til tekstboksene
    var dato = $("#dato" + i).val();
    var datoRetur = $("#datoRetur" + i).val();
    var antallReisende = $("#antallReisende" + i).val();
    var totalPris = $("#totalPris" + i).val();

    //Ajaxkall for å kjøre endreBestillinger på HomeController
    $.ajax({
        type: 'GET',
        url: '/Home/endreBestilling',
        data: 'id=' + id + '&dato=' + dato + '&datoRetur=' + datoRetur + '&antallReisende=' + antallReisende + '&totalPris=' + totalPris,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/AdminSide";
            console.log("Bestilling endret!");
        }
    });
}

function slettBestilling(id) {
    $.ajax({
        type: 'GET',
        url: '/Home/slettBestilling',
        data:'id=' + id,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/AdminSide";
            console.log("Bestilling slettet!");
        }
    });
}


function slettAdmin(id) {
    $.ajax({
        type: 'GET',
        url: '/Home/slettAdmin',
        data: 'id=' + id,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/AdminSide";
            console.log('Admin slettet');
        }
    });
}


function endreAdmin(id, i) {
    var fornavn = $("#fornavn" + i).val();
    var etternavn = $("#etternavn" + i).val();
    var telefon = $("#telefon" + i).val();
    var epost = $("#epost" + i).val();

    $.ajax({
        type: 'POST',
        url: '/Home/endreAdmin',
        data: 'id=' + id + '&fornavn=' + fornavn + '&etternavn=' + etternavn + '&telefon=' + telefon + '&epost=' + epost,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/AdminSide";
            console.log(fornavn + ' er nå endret');
        }
    });
}




/*----------------------- BEKREFTELSESMODALER-----------------------------*/

function modalSlettBestilling(id, i) {
    var dato = $("#dato" + i).val();
    var datoRetur = $("#datoRetur" + i).val();
    var antallReisende = $("#antallReisende" + i).val();
    var totalPris = $("#totalPris" + i).val();

    var utStreng = "";
    utStreng += "Ønsker du å slette denne bestillingen </br>";
    utStreng += "<strong>" + "Reise fra: " + dato + "</br> Reise til: " + datoRetur + "</br> antall reiser:" + antallReisende + "</br> total pris: " + totalPris;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        slettBestilling(id);
    });
}


function modalEndreBestilling(id, i) {
    var dato = $("#dato" + i).val();
    var datoRetur = $("#datoRetur" + i).val();
    var antallReisende = $("#antallReisende" + i).val();
    var totalPris = $("#totalPris" + i).val();

    var utStreng = "";
    utStreng += "Ønsker du å endre denne bestillingen til: </br>";
    utStreng += "<strong>" + "Reise fra: " + dato + "</br> Reise til: " + datoRetur + "</br> antall reiser:" + antallReisende + "</br> total pris: " + totalPris;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        endreBestilling(id, i);
    });
}

function modalSlettAdmin(id, i) {
    var fornavn = $("#fornavn" + i).val();
    var etternavn = $("#etternavn" + i).val();
    var telefon = $("#telefon" + i).val();
    var epost = $("#epost" + i).val();

    var utStreng = "";
    utStreng += "Ønsker du å slette denne kontoen? </br>";
    utStreng += "<strong>" + id + ". " + epost + ": "  + fornavn + " " + etternavn + ", " + telefon;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        slettAdmin(id);
    });

}

function modalEndreAdmin(id, i) {
    var fornavn = $("#fornavn" + i).val();
    var etternavn = $("#etternavn" + i).val();
    var telefon = $("#telefon" + i).val();
    var epost = $("#epost" + i).val();

    var utStreng = "";
    utStreng += "Ønsker du å endre denne kontoen til: </br>";
    utStreng += "<strong>" + id + ". " + epost + ": " + fornavn + " " + etternavn + ", " + telefon;
    $(".modalHere").html(utStreng);

    $("#btnBekreftModal").click(function () {
        endreAdmin(id, i);
    });

}


/*----------------------- VALIDERING -----------------------------*/


function validerTotalPris(i) {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById('totalPris'+ i ).value);

    if (!OK) {
        document.getElementById('totalPris' + i).style.background = "#ffccc9";
        document.querySelector('.alertBestilling').style.display = "block";
        document.querySelector('.alertBestillingText').innerHTML = "Totalpris kan inneholde kun tall!";
        return false;

    } else {
        document.getElementById('totalPris' + i).style.background = "#ffffff";
        return true;
    }
};

function validerAntallReisende(i) {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById('antallReisende' + i).value);

    if (!OK) {
        document.getElementById('antallReisende' + i).style.background = "#ffccc9";
        document.querySelector('.alertBestilling').style.display = "block";
        document.querySelector('.alertBestillingText').innerHTML = "Antall reisende kan inneholde kun tall!";
        return false;

    } else {
        document.getElementById('antallReisende' + i).style.background = "#ffffff";
        return true;
    }
};


function validerDato(i) {
    var regEx = /^[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}$/;
    OK = regEx.test(document.getElementById('dato' + i).value);

    if (!OK) {
        document.getElementById('dato' + i).style.background = "#ffccc9";
        document.querySelector('.alertBestilling').style.display = "block";
        document.querySelector('.alertBestillingText').innerHTML = "Dato må skrives f.eks slik: yyyy-mm-dd";
        return false;

    } else {
        document.getElementById('dato' + i).style.background = "#ffffff";
        return true;
    }
};


function validerBestilling(i) {
    var totalPrisOK = validerTotalPris(i);
    var antallReisendeOK = validerAntallReisende(i);
    var datoOK = validerDato(i);

    if (totalPrisOK && antallReisendeOK && datoOK) {
        document.getElementById('btnEndreBestilling' + i).disabled = false;
        document.querySelector('.alertBestilling').style.display = "none";
        return true;
    } else {
        document.getElementById('btnEndreBestilling' + i).disabled = true;
        return false;
    }
}



function validerFornavn(i) {
    var regEx = /^[a-zæøåA-ZÆØÅ]{2,30}$/;
    OK = regEx.test(document.getElementById('fornavn' + i).value);

    if (!OK) {
        document.getElementById('fornavn' + i).style.background = "#ffccc9";
        document.querySelector('.alertAdmin').style.display = "block";
        document.querySelector('.alertAdminText').innerHTML = "Fornavnet kan kun inneholde bokstaver";
        return false;

    } else {
        document.getElementById('fornavn' + i).style.background = "#ffffff";
        return true;
    }
};


function validerEtternavn(i) {
    var regEx = /^[a-zæøåA-ZÆØÅ]{2,30}$/;
    OK = regEx.test(document.getElementById('etternavn' + i).value);

    if (!OK) {
        document.getElementById('etternavn' + i).style.background = "#ffccc9";
        document.querySelector('.alertAdmin').style.display = "block";
        document.querySelector('.alertAdminText').innerHTML = "Etternavnet kan kun inneholde bokstaver";
        return false;

    } else {
        document.getElementById('etternavn' + i).style.background = "#ffffff";
        return true;
    }
};

function validerTelefon(i) {
    var regEx = /^[0-9]{8}$/;
    OK = regEx.test(document.getElementById('telefon' + i).value);

    if (!OK) {
        document.getElementById('telefon' + i).style.background = "#ffccc9";
        document.querySelector('.alertAdmin').style.display = "block";
        document.querySelector('.alertAdminText').innerHTML = "Telefonnummer må inneholde 8 siffer";
        return false;

    } else {
        document.getElementById('telefon' + i).style.background = "#ffffff";
        return true;
    }
};


function validerEpost(i) {
    var regEx = /^[a-zæøåA-ZÆØÅ0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,60}$/;
    OK = regEx.test(document.getElementById('epost' + i).value);

    if (!OK) {
        document.getElementById('epost' + i).style.background = "#ffccc9";
        document.querySelector('.alertAdmin').style.display = "block";
        document.querySelector('.alertAdminText').innerHTML = "Email må skrives f.eks slik: test@example.no";
        return false;

    } else {
        document.getElementById('epost' + i).style.background = "#ffffff";
        return true;
    }
};


function validerAdmin(i) {
    var fornavnOK = validerFornavn(i);
    var etternavnOK = validerEtternavn(i);
    var telefonOK = validerTelefon(i);
    var epostOK = validerEpost(i);

    if (fornavnOK && etternavnOK && telefonOK && epostOK) {
        document.getElementById('btnEndreAdmin' + i).disabled = false;
        document.querySelector('.alertAdmin').style.display = "none";
        return true;
    } else {
        document.getElementById('btnEndreAdmin' + i).disabled = true;
        return false;
    }
}





