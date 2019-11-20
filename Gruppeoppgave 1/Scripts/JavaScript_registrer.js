function validerFornavn() {
    var regEx = /^[a-zæøåA-ZÆØÅ]{2,30}$/;
    OK = regEx.test(document.getElementById('fornavn').value);

    if (!OK) {
        document.getElementById('feilFornavn').innerHTML = "Fornavnet må ha minst 2 bokstaver";
        return false;

    } else {
        document.getElementById('feilFornavn').innerHTML = "";
        return true;
    }
};


function validerEtternavn() {
    var regEx = /^[a-zæøåA-ZÆØÅ]{2,30}$/;
    OK = regEx.test(document.getElementById('etternavn').value);

    if (!OK) {
        document.getElementById('feilEtternavn').innerHTML = "Etternavnet må ha minst 2 bokstaver";
        return false;

    } else {
        document.getElementById('feilEtternavn').innerHTML = "";
        return true;
    }
};


function validerTelefon() {
    var regEx = /^[0-9]{8}$/;
    OK = regEx.test(document.getElementById('telefon').value);

    if (!OK) {
        document.getElementById('feilTelefon').innerHTML = "Telefon må inneholde 8 tall";
        return false;

    } else {
        document.getElementById('feilTelefon').innerHTML = "";
        return true;
    }
};

function validerEpost() {
    var regEx = /^[a-zæøåA-ZÆØÅ0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,60}$/;
    OK = regEx.test(document.getElementById('epost').value);

    if (!OK) {
        document.getElementById('feilEpost').innerHTML = "Eposten må være f.eks: eksempel@test.no";
        return false;

    } else {
        document.getElementById('feilEpost').innerHTML = "";
        return true;
    }
};

function validerPassord() {
    var regEx = /^[a-zæøåA-ZÆØÅ0-9._-]{5,30}$/;
    OK = regEx.test(document.getElementById('passord').value);

    if (!OK) {
        document.getElementById('feilPassord').innerHTML = "Passordet må innholde minst 5 tegn";
        return false;

    } else {
        document.getElementById('feilPassord').innerHTML = "";
        return true;
    }
};

function verifiserPassord() {
    var passord = document.getElementById('passord').value;
    var sjekkpassord = document.getElementById('passordsjekk').value;

    if (passord !== sjekkpassord) {
       document.getElementById('feilPassordSjekk').innerHTML = "Passordene må være like";
        $("#passord").css("background", "#ffccc9");
        $("#passordsjekk").css("background", "#ffccc9");
        validerPassord();
        return false;

    } else if (sjekkpassord == "") {
        document.getElementById('feilPassordSjekk').innerHTML = "Passordene må være like";
        $("#passord").css("background", "#ffccc9");
        $("#passordsjekk").css("background", "#ffccc9");
        return false;

    } else if (passord == "") {
        document.getElementById('feilPassordSjekk').innerHTML = "Passordene må være like";
        $("#passord").css("background", "#ffccc9");
        $("#passordsjekk").css("background", "#ffccc9");
        return false;

    } else {
        document.getElementById('feilPassordSjekk').innerHTML = "";
        $("#passord").css("background", "#daf5c9");
        $("#passordsjekk").css("background", "#daf5c9");
        validerPassord();
        return true;
    }
}


function validerAdmin() {
    var fornavnOK = validerFornavn();
    var etternavnOK = validerEtternavn();
    var telefonOK = validerTelefon();
    var EpostOK = validerEpost();
    var passordOK = validerPassord();
    var passordMATCH = verifiserPassord();

    if (fornavnOK && etternavnOK && telefonOK && EpostOK && passordOK && passordMATCH) {
        return true;
    } else {
        return false;
    }
}












