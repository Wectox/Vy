$(document).ready(function () {

    $.ajax({
        type: 'GET',
        url: '/Home/getPris',
        dataType: 'json',
        success: function (priser) {
            listPriser(priser);
        }
    });

});


function listPriser(priser) {
    var utStreng = "";
    utStreng += "<table class='table table-hover w-75 tabellList'>"
    utStreng += " <thead> <tr> <th>Type billett</th> <th>Pris</th> <th></th> </thead> "
    utStreng += "<tr> <td><strong> Voksenpris </strong></td>  <td><input type='text' id='voksenPris' value='" + priser[0].Voksenpris + "' onkeydown='validerVoksen(); validerAllePriser()' onchange='validerVoksen(); validerAllePriser()' maxlength='6'/> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='voksenprisen' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "voksenPris" + "\")'>Endre</button></td> </tr>"
    utStreng += "<tr> <td><strong> Studentpris </strong></td>  <td><input type='text' id='studentPris' value='" + priser[0].Studentpris + "' onkeydown='validerStudent(); validerAllePriser()' onchange='validerStudent(); validerAllePriser()' /> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='studentprisen' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "studentPris" + "\")'>Endre</button></td> </tr>"
    utStreng += "<tr> <td><strong> Barnepris </strong></td>  <td><input type='text' id='barnePris' value='" + priser[0].Barnepris + "' onkeydown='validerBarn(); validerAllePriser()' onchange='validerBarn(); validerAllePriser()'/> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='barneprisen' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "barnePris" + "\")'>Endre</button></td> </tr>"
    utStreng += "<tr> <td><strong> Ungdomspris </strong></td>  <td><input type='text' id='ungdomPris' value='" + priser[0].Ungdompris + "' onkeydown='validerUngdom(); validerAllePriser()' onchange='validerUngdom(); validerAllePriser()'/> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='ungdomsprisen' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "ungdomPris" + "\")'>Endre</button></td> </tr>"
    utStreng += "<tr> <td><strong> Honnørpris </strong></td>  <td><input type='text' id='honnorPris' value='" + priser[0].Honnorpris + "'  onkeydown='validerHonnor(); validerAllePriser()' onchange='validerHonnor(); validerAllePriser()' /> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='honnørprisen' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "honnorPris" + "\")'>Endre</button></td> </tr>"
    utStreng += "<tr> <td><strong> Verneplikt </strong></td>  <td><input type='text' id='vernepliktPris' value='" + priser[0].Vernepliktpris + "'  onkeydown='validerVerneplikt(); validerAllePriser()' onchange='validerVerneplikt(); validerAllePriser()'/> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='vernepliktprisen' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "vernepliktPris" + "\")'>Endre</button></td> </tr>"
    utStreng += "<tr> <td><strong> Pris per sone </strong></td>  <td><input type='text' id='prisPerSone' value='" + priser[0].PrisPerSone + "' onkeydown='validerPerSone(); validerAllePriser()' onchange='validerPerSone(); validerAllePriser()'/> kr </td>"
    utStreng += "<td><button type='button' class='btn btn-dark btnEndrePris' href='#bekrefteModal' value='prisen per sone' data-toggle='modal' onclick='modalEndrePris(" + priser[0].id + ", this, \"" + "prisPerSone" + "\")'>Endre</button></td> </tr>"
    utStreng += "</table>"

    $("#visPriser").html(utStreng);
}
    


function endrePris(id) {
    var prisVoks = $("#voksenPris").val();
    var prisStud = $("#studentPris").val();
    var prisBarn = $("#barnePris").val();
    var prisUng = $("#ungdomPris").val();
    var prisHonn = $("#honnorPris").val();
    var prisVern = $("#vernepliktPris").val();
    var prisPrSone = $("#prisPerSone").val();


    $.ajax({
        type: 'POST',
        url: '/Home/endrePriser',
        data: 'id=' + id + '&prisVoksen=' + prisVoks + '&prisStudent=' + prisStud + '&prisBarn=' + prisBarn + '&prisUngdom=' + prisUng + '&prisHonnor=' + prisHonn + '&prisVerneplikt=' + prisVern + '&prisPerSone=' + prisPrSone,
        dataType: 'json',
        success: function () {
            window.location.href = "/Home/endrePris";
            console.log("Pris er endret!"); 
        }

    });
}


function modalEndrePris(id, typeBillett, pris) {
    var nyPris = $("#" + pris).val();
    $(".modalHere").html("Ønsker du å endre <strong>" + typeBillett.value + "</strong> til <strong>" + nyPris + "kr ?</strong>");

    console.log(typeBillett.value + " " + nyPris);

    $("#btnBekreftModal").click(function () {
        endrePris(id);
    });
}

/*----------------------- VALIDERING -----------------------------*/

function validerVoksen() {
    var regEx = /^[0-9]{1,6}$/;
    OK= regEx.test(document.getElementById("voksenPris").value);
   
    if (!OK) {
        document.getElementById('voksenPris').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("voksenPris").style.background = "#ffffff";
        return true;
    }   
};


function validerStudent() {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById("studentPris").value);

    if (!OK) {
        document.getElementById('studentPris').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("studentPris").style.background = "#ffffff";
        return true;
    }
};

function validerBarn() {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById("barnePris").value);

    if (!OK) {
        document.getElementById('barnePris').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("barnePris").style.background = "#ffffff";
        return true;
    }
};

function validerUngdom() {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById("ungdomPris").value);

    if (!OK) {
        document.getElementById('ungdomPris').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("ungdomPris").style.background = "#ffffff";
        return true;
    }
};

function validerHonnor() {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById("honnorPris").value);

    if (!OK) {
        document.getElementById('honnorPris').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("honnorPris").style.background = "#ffffff";
        return true;
    }
};

function validerVerneplikt() {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById("vernepliktPris").value);

    if (!OK) {
        document.getElementById('vernepliktPris').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("vernepliktPris").style.background = "#ffffff";
        return true;
    }
};


function validerPerSone() {
    var regEx = /^[0-9]{1,6}$/;
    OK = regEx.test(document.getElementById("prisPerSone").value);

    if (!OK) {
        document.getElementById('prisPerSone').style.background = "#ffccc9";
        document.querySelector('.alertPris').style.display = "block";
        document.querySelector('.alertPrisText').innerHTML = "Pris kan bare inneholde siffer!";
        return false;

    } else {
        document.getElementById("prisPerSone").style.background = "#ffffff";
        return true;
    }
};


function validerAllePriser() {
    var voksenOK = validerVoksen();
    var studentOK = validerStudent();
    var barnOK = validerBarn();
    var ungdomOK = validerUngdom();
    var honnorOK = validerHonnor();
    var vernepliktOK = validerVerneplikt();
    var perSoneOK = validerPerSone();


    if (voksenOK && studentOK && barnOK && ungdomOK && honnorOK && vernepliktOK && perSoneOK) {
        $(".btnEndrePris").attr("disabled", false);
        document.querySelector('.alertPris').style.display = "none";
        return true;
    } else {
        $(".btnEndrePris").attr("disabled", true);
        return false;
    }
}












