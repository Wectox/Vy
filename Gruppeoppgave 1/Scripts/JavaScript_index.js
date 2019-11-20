var count = [1, 0, 0, 0, 0, 0]; 
var maxBillett = 9;
var sum = 0;
var antallBillett = 1;
var avgangArrayRetur;
var avgangArray;
var returArray;

var riktigBillett = true;
var riktigDestinasjon = false;
var riktigTid = true;
var riktigDato = true;


$(document).ready(function () {

    document.getElementById("antVokse").innerHTML = count[0]; 

    $("#btnBekreft").click(function () {
        var avgangId = $("#getAvgangidText").val();
        var dato = $("#getDatoText").val();
        var antallReisende = antallBillett;
        if (getRadio() == 1) { 
            var totalSum = sum;

            $.ajax({
                url: '/Home/leggBestillingEnvei',
                type: 'GET',
                dataType: 'json',
                data: 'avgangId=' + avgangId + '&dato=' + dato + '&antallReisende=' + antallReisende + '&totalSum=' + totalSum,
                success: function () {
                    console.log("Ok envei");
                }
            });
        }
        else if (getRadio() == 2) { 
            var totalSum = sum * 2;
            var returId = $("#getAvgangidReturText").val();
            var datoRetur = $("#getDatoReturText").val();

            $.ajax({
                url: '/Home/leggBestillingTurRetur',
                type: 'GET',
                dataType: 'json',
                data: 'avgangId=' + avgangId + '&returId=' + returId + '&dato=' + dato + '&datoRetur=' + datoRetur + '&antallReisende=' + antallReisende + '&totalSum=' + totalSum,
                success: function () {
                    console.log("Ok turRetur");
                }
            });
        }
    });
});


$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Home/hentDestinasjon',
        dataType: 'json',
        success: function (jsKunde) {
            dropdownDestinasjonFra(jsKunde);
        }
    });
});



$(document).ready(function () {
    visCurrentDate();
    $("#getSumText").val("0");

    var datoEnVei = $("#dato").val();
    $("#getDatoText").val(datoEnVei);
    var datoRetur = $("#datoRetur").val();
    $("#getDatoReturText").val(datoRetur);
    
    $.ajax({
        type: 'GET',
        url: '/Home/hentDestinasjon',
        dataType: 'json',
        success: function (jsKunde) {
            dropdownDestinasjonTil(jsKunde);
        }
    });

    $(".btnBillett").click(function () {
        settinnAnt(this.id);
    });


    $("#dato, #datoRetur ").change(function () {
        var datoEnVei = $("#dato").val();
        $("#getDatoText").val(datoEnVei);

        var datoRetur = $("#datoRetur").val();
        $("#getDatoReturText").val(datoRetur);

        sjekkDato();
        forskjellTid();
    });

    $("#tid").change(function () {
        console.log("Funker dette?");
        forskjellTid();
    });

    $("#tidRetur").change(function () {
        forskjellTid();
    });


    $("#turReturRadio, #enVeiRadio").change(function () {

        if ($('#turReturRadio').is(':checked')) {
            $('#rowTurRetur').animate({ opacity: 1 });
            $("#rowTurRetur").css('display', 'block');

            $(".h5TurRetur").css('display', 'block');

            $(".billettRetur").css('display', 'block');
            $(".billettRetur").css('opacity', '1');
            $(".billettRetur").css('margin-right', '0');
            $(".billettBoks").css('float', 'left');

            console.log("it's checked");


        } else if ($('#enVeiRadio').is(':checked')) {
            $('#rowTurRetur').animate({ opacity: 0 });
            $("#rowTurRetur").css('display', 'none');

            $(".h5TurRetur").css('display', 'none');

            $(".billettRetur").css('display', 'none');
            $(".billettRetur").css('opacity', '0');

            $(".billettBoks").css('display', 'block');
        }
        console.log("Test");

        forskjellTid();
    });


    $("#sokBillett").click(function () {
        var destFra = $("#dropdownFra").val();
        var destTil = $("#dropdownTil").val();
        var tid = getTid();
        var tidRetur = getTidRetur();

        $.ajax({
            url: '/Home/hentPriser/',
            type: 'GET',
            dataType: 'json',
            success: function (priser) {
                getPris(priser);
            }
        });

        setTimeout(function () { 

            $(".section-avgang").css('display', 'block');
            $("#gaVidere").attr("disabled", true);

            $('html, body').animate({
                scrollTop: $(".avgangOversikt").offset().top
            }, 1000);

            $.ajax({
                url: '/Home/hentAvganger/',
                type: 'GET',
                dataType: 'json',
                data: 'destFra=' + destFra + '&destTil=' + destTil + '&tid=' + tid,
                success: function (avgang) {
                    visAvgangInfo(avgang);
                    visAvgangOverskrift(avgang);
                }
            });

            if (getRadio() == 2) {
                $.ajax({
                    url: '/Home/hentReturAvganger/',
                    type: 'GET',
                    dataType: 'json',
                    data: 'destFra=' + destFra + '&destTil=' + destTil + '&tid=' + tidRetur,
                    success: function (avgang) {
                        visReturInfo(avgang);
                    }
                });
            }
        }, 500);     
    });

    $("#enVeiRadios").click(function () {
        console.log("DU HAR TRYKKET");
    });


    $("#enReturRadio").click(function () {
        var avgang = document.getElementById("enReturRadio").value;
        console.log("Test: " + avgang);
    });


    $("#gaVidere").click(function () {

        $(".section-header").css('display', 'none');
        $(".section-header").css('opacity', '0');

        $(".section-registrering").css('display', 'none');
        $(".section-registrering").css('opacity', '0');

        $(".section-avgang").css('display', 'none');
        $(".section-avgang").css('opacity', '0');

        $(".section-bekreftelse").css('display', 'block');
        $('.section-bekreftelse').animate({ opacity: 1.5 });
    });


    $("#btnTilbake").click(function () {

        $(".section-header").css('display', 'block');
        $('.section-header').animate({ opacity: 1.5 });

        $(".section-registrering").css('display', 'block');
        $('.section-registrering').animate({ opacity: 1.5 });

        $(".section-avgang").css('display', 'block');
        $('.section-avgang').animate({ opacity: 1.5 });

        $(".section-bekreftelse").css('display', 'none');
        $(".section-bekreftelse").css('opacity', '0');
    });
});

//------------------------ FUNKSJONER ------------------------------------//


function visAvgangOverskrift(avgang) {
    $("#avgangEnVeiOverskrift").html("Velg avgang fra " + avgang[0].destinasjonFra + " - " + avgang[0].destinasjonTil + ", " + getDato());
    $("#avgangTurReturOverskrift").html("Velg avgang fra " + avgang[0].destinasjonTil + " - " + avgang[0].destinasjonFra + ", " + getDatoRetur());
}


function visAvgangInfo(avgang) {
    var utStreng = "";
    utStreng += "<ul class='list-group w-75 mx-auto'>"
    avgangArrayEnVei = {};
    for (var i in avgang) {
        utStreng += "<li class='list-group-item list-group-item-action listAvganger'>";
        utStreng += "<span class='avgangTid'><strong>Kl:" + avgang[i].tid + "  -  " + avgang[i].ankomst + "</strong></span>";
        utStreng += "<span class='avgangText'>" + avgang[i].destinasjonFra + " - " + avgang[i].destinasjonTil + "</span>";
        utStreng += "<span class='avgangPris'><strong>" + sum + " ,-" + "</strong></span>";
        utStreng += "<span class='float-right pr-3'><input type='radio' class='avgangRadio' onclick='getAvgangEnVeiRadio(); validerAvganger();' id='enVeiRadios' name='avgangEnVei' value='" + i + "'> </span></li>";
        avgangArrayEnVei[i] = avgang[i];
    }
    utStreng += "</ul>"

    $('.avgangOversikt').animate({ opacity: 1 });

    $("#visInfo").html(utStreng);
}



function visReturInfo(avgang) {
    var utStreng = "";
    avgangArrayRetur = {};
    utStreng += "<ul class='list-group w-75 mx-auto'>"
    for (var i in avgang) {
        utStreng += "<li class='list-group-item list-group-item-action listAvganger'>";
        utStreng += "<span class='avgangTid'><strong>Kl:" + avgang[i].tid + "  -  " + avgang[i].ankomst + "</strong></span>";
        utStreng += "<span class='avgangText'>" + avgang[i].destinasjonFra + " - " + avgang[i].destinasjonTil + "</span>";
        utStreng += "<span class=' avgangPris'><strong>" + sum + " ,-" + "</strong></span>";
        utStreng += "<span class='float-right pr-3'><input type='radio' class='avgangRadio' onclick='getAvgangReturRadio(); validerAvganger();' id='enReturRadio' name='avgangReturVei' value='" + i + "'> </span></li>";
        avgangArrayRetur[i] = avgang[i];
    }
    utStreng += "</ul>"

    $("#returInfo").html(utStreng);
}

function getAvgangEnVeiRadio() {
    var id = $("#enVeiRadios:checked").val();
    var avgang = avgangArrayEnVei[id];

    $("#getAvgangidText").val(avgangArrayEnVei[id].id);
    $("#getDestFraText").val(avgangArrayEnVei[id].destinasjonFra);
    $("#getDestTilText").val(avgangArrayEnVei[id].destinasjonTil);
    $("#getTidText").val(avgangArrayEnVei[id].tid);
    $("#getAnkomstText").val(avgangArrayEnVei[id].ankomst);
    $("#getDatoText").val(getDato());
    $("#getPrisText").val(sum+"kr");
    
    console.log("Fra: " + avgangArrayEnVei[id].destinasjonFra + " Til: " + avgangArrayEnVei[id].destinasjonTil + " Tid: " + avgangArrayEnVei[id].tid + " id: " + avgangArrayEnVei[id].id);

    if (getRadio() == 1) {
        $("#getSumText").val(sum + "kr");
    }
    else if (getRadio() == 2) {
        $("#getSumText").val(sum * 2 + "kr");
    }

    return avgang;
}


function getAvgangReturRadio() {
    var id = $("#enReturRadio:checked").val();
    var avgang = avgangArrayRetur[id];

    $("#getAvgangidReturText").val(avgangArrayRetur[id].id);
    $("#getDestFraReturText").val(avgangArrayRetur[id].destinasjonFra);
    $("#getDestTilReturText").val(avgangArrayRetur[id].destinasjonTil);
    $("#getTidReturText").val(avgangArrayRetur[id].tid);
    $("#getAnkomstReturText").val(avgangArrayRetur[id].ankomst);
    $("#getDatoReturText").val(getDatoRetur());
    $("#getPrisReturText").val(sum + "kr");

    console.log("Fra: " + avgangArrayRetur[id].destinasjonFra + " Til: " + avgangArrayRetur[id].destinasjonTil + " Tid: " + avgangArrayRetur[id].tid + " id: " + avgangArrayRetur[id].id);
    return avgang;
}

function validerAvganger() {
    if (getRadio() == 1) {
        $("#gaVidere").removeAttr("disabled");

    } else if (getRadio() == 2) {

        if ($('[name="avgangEnVei"]').is(':checked') && $('[name="avgangReturVei"]').is(':checked')) {
            $("#gaVidere").removeAttr("disabled");

        } else {
            $("#gaVidere").attr("disabled", true);
        }
    }
}


function dropdownDestinasjonFra(jsKunde) {
    var utstreng = '<option disabled selected>Hvor ønsker du å reise fra?</option>';
    for (var i in jsKunde) {
        utstreng += '<option value="' + jsKunde[i].id + '">' + jsKunde[i].sted + '</option>';
    }
    $("#dropdownFra").html(utstreng);
}


function dropdownDestinasjonTil(jsKunde) {
    var utstreng = '<option disabled selected>Hvor ønsker du å reise til?</option>';
    for (var i in jsKunde) {
        utstreng += '<option value="' + jsKunde[i].id + '">' + jsKunde[i].sted + '</option>';
    }
    $("#dropdownTil").html(utstreng);
}




$(function () {
    $("#sokBillett").attr("disabled", true);
    
    $("#dropdownFra").change(function () {
        validerDestinasjon();
    });

    $("#dropdownTil").change(function () {
        validerDestinasjon();
    }); 

    function validerDestinasjon() {
        var sjekkFra = $("#dropdownFra").val();
        var sjekkTil = $("#dropdownTil").val();

        if (sjekkFra == sjekkTil) {
            $("#feil").html("Du har valgt samme stasjon fra og til. Vennligst endre på det for å gå videre.");
            $(".destinasjon").css("border-bottom", "2px solid red");
            $("#feil").css("color", "red");
            riktigDestinasjon = false;


        } else if (sjekkFra > 0 && sjekkTil == null || sjekkFra == null && sjekkTil > 0) {
            riktigDestinasjon = false;

        } else {
            $("#feil").html("");
            $(".destinasjon").css("border-bottom", "2px solid #7d7d7d");
            riktigDestinasjon = true;
        }

        sjekkValidering();
    }
});



function getRadio() {
    if ($('#enVeiRadio').is(':checked')) {
        $("#avgangTurReturOverskrift").css('display', 'none');
        $("#returInfo").css('display', 'none');
        $("#returInfo").css('opacity', '0');
        console.log("EnVei markert");
        return 1;
    }
    else if ($('#turReturRadio').is(':checked')) {
        $("#avgangTurReturOverskrift").css('display', 'block');
        $("#returInfo").css('display', 'block');
        $("#returInfo").css('opacity', '1');
        console.log("TurRetur markert");
        return 2;
    }
    else {
        console.log("Ingenting markert");
        return 0;
    }
}

function getTid() {
    var tid = document.getElementById("tid").value;
    var tidInt = parseInt(tid.substring(0, 2));
    return tidInt;
}

function getTidRetur() {
    var tid = document.getElementById("tidRetur").value;
    var tidInt = parseInt(tid.substring(0, 2));
    return tidInt;
}

function getDato() {
    var dato = document.getElementById("dato").value;
    return dato;
}

function getDatoRetur() {
    var dato = document.getElementById("datoRetur").value;
    return dato;
}


function sjekkBillett() {
    antallBillett = 0;
    for (var i = 0; i < count.length; i++) {
        antallBillett += count[i];
    }
    console.log(antallBillett);

    if (antallBillett == 0) {
        riktigBillett = false;
        sjekkValidering();
    } else if (antallBillett >= 1 && antallBillett < 9) {
        $(".feilReisende").html("");
        $("#ungdoIncBtn").attr("disabled", false);
        $("#vokseIncBtn").attr("disabled", false);
        $("#studeIncBtn").attr("disabled", false);
        $("#barneIncBtn").attr("disabled", false);
        $("#honnoIncBtn").attr("disabled", false);
        $("#verneIncBtn").attr("disabled", false);
        riktigBillett = true;
    } else if (antallBillett >= 9) {
        $(function () {
            $("#vokseIncBtn").attr("disabled", true);
            $("#studeIncBtn").attr("disabled", true);
            $("#ungdoIncBtn").attr("disabled", true);
            $("#barneIncBtn").attr("disabled", true);
            $("#honnoIncBtn").attr("disabled", true);
            $("#verneIncBtn").attr("disabled", true);
            $(".feilReisende").html("Du kan bare velge 9 reisende");
        });
        riktigBillett = true;
        sjekkValidering();
    }
    sjekkValidering();
}

function settinnAnt(typeId) {
    var id = typeId.substring(0, 5);
    var type = typeId.substring(5, 8);

    switch (id) {
        case 'vokse':
            type == "Inc" ? count[0]++ : count[0]--;
            if (count[0] <= 0) count[0] = 0;
            document.getElementById("antVokse").innerHTML = count[0];
            sjekkBillett();
            break;
        case 'stude':
            type == "Inc" ? count[1]++ : count[1]--;
            if (count[1] <= 0) count[1] = 0;
            document.getElementById("antStude").innerHTML = count[1];
            sjekkBillett();
            break;
        case 'ungdo':
            type == "Inc" ? count[2]++ : count[2]--;
            if (count[2] <= 0) count[2] = 0;
            document.getElementById("antUngdo").innerHTML = count[2];
            sjekkBillett();
            break;
        case 'barne':
            type == "Inc" ? count[3]++ : count[3]--;
            if (count[3] <= 0) count[3] = 0;
            document.getElementById("antBarne").innerHTML = count[3];
            sjekkBillett();
            break;
        case 'honno':
            type == "Inc" ? count[4]++ : count[4]--;
            if (count[4] <= 0) count[4] = 0;
            document.getElementById("antHonno").innerHTML = count[4];
            sjekkBillett();
            break;
        case 'verne':
            type == "Inc" ? count[5]++ : count[5]--;
            if (count[5] <= 0) count[5] = 0;
            document.getElementById("antVerne").innerHTML = count[5];
            sjekkBillett();
            break;
    }
}

function visCurrentDate() {
    var field = document.getElementById("dato");
    var field2 = document.getElementById("datoRetur");
    var date = new Date();

    // Set the date
    field.value = date.getFullYear().toString() + '-' + (date.getMonth() + 1).toString().padStart(2, 0) +
        '-' + date.getDate().toString().padStart(2, 0);

    field2.value = date.getFullYear().toString() + '-' + (date.getMonth() + 1).toString().padStart(2, 0) +
        '-' + date.getDate().toString().padStart(2, 0);
}

function getPris(priser) {
    var destFra = $("#dropdownFra").val();
    var destTil = $("#dropdownTil").val();
    var antVoksen = count[0];
    var antStudent = count[1];
    var antUngdom = count[2];
    var antBarn = count[3];
    var antHonnor = count[4];
    var antVerneplikt = count[5];
    var prisVoksen = priser.prisVoksen;
    var prisStudent = priser.prisStudent;
    var prisUngdom = priser.prisUngdom;
    var prisBarn = priser.prisBarn;
    var prisHonnor = priser.prisHonnor;
    var prisVerneplikt = priser.prisVerneplikt;
    var sonePris = Math.abs(destFra - destTil) * priser.prisPerSone;

    sum = (prisVoksen + sonePris) * antVoksen + (prisStudent + sonePris) * antStudent + (prisUngdom + sonePris) * antUngdom + (prisBarn + sonePris) * antBarn + (prisHonnor + sonePris) * antHonnor + (prisVerneplikt + sonePris) * antVerneplikt;
}


function forskjellTid() {

    var forsteTid = getTid();
    var andreTid = getTidRetur();
    var forsteDato = getDato();
    var andreDato = getDatoRetur();

    if (forsteDato == andreDato && andreTid <= forsteTid) {
        riktigTid = false;
        console.log("RiktigTid? :" + riktigTid);
    }
    else {
        riktigTid = true;
        console.log("RiktigTid? :" + riktigTid);
    }
    sjekkValidering();
}


function sjekkDato() {
    var forsteDato = getDato();
    var andreDato = getDatoRetur();
    var forsteAr = parseInt(forsteDato.substring(0, 4));
    var forsteManed = parseInt(forsteDato.substring(5, 7));
    var forsteDag = parseInt(forsteDato.substring(8, 10));
    var andreAr = parseInt(andreDato.substring(0, 4));
    var andreManed = parseInt(andreDato.substring(5, 7));
    var andreDag = parseInt(andreDato.substring(8, 10));

    if (forsteAr <= andreAr && forsteManed <= andreManed && forsteDag <= andreDag) {
        riktigDato = true;
    }
    else {
        riktigDato = false;
    }

    sjekkValidering();
    console.log(forsteAr + " " + forsteManed + " " + forsteDag + "  " + andreAr + " " + andreManed + " " + andreDag);   
}


function sjekkValidering() {
    console.log("RiktigBillett: " + riktigBillett + " RiktigDestinasjon: " + riktigDestinasjon + " RiktigTid: " + riktigTid);
    if (getRadio() == 1) riktigTid = true;

    if (riktigBillett && riktigDestinasjon && riktigTid && riktigDato) {
        $("#sokBillett").attr("disabled", false);
        $(".feilMeldingText").html("");
    }
    else if (!riktigBillett) {
        $("#sokBillett").attr("disabled", true);
        if (antallBillett == 0) $(".feilMeldingText").html("Du må velge minst 1 billett!");
    }
    else if (!riktigDestinasjon) {
        $("#sokBillett").attr("disabled", true);
        $(".feilMeldingText").html("Du har valgt ugyldig destinasjoner");
    }
    else if (!riktigTid) {
        $("#sokBillett").attr("disabled", true);
        $(".feilMeldingText").html("Du har valgt ugydlig tidspunkt");
    }
    else if (!riktigDato) {
        $("#sokBillett").attr("disabled", true);
        $(".feilMeldingText").html("Du har valgt ugyldig dato");
    }
    else {
        $("#sokBillett").attr("disabled", true);
    } 
}
