const USER = JSON.parse(sessionStorage.getItem('currentUser'));


const APPLYVALIDATION = _ => {

    let validation = true;
    $('.Date , .Terminal , .Time ').removeClass("is-invalid");

    
    let Date = document.getElementById('DateDrop').value;
    let Terminal = document.getElementById('TerminalDrop').value;
    let Time = document.getElementById('TimeDrop').value;
   

     

    if (Date == "") {
        $('.Date').addClass("is-invalid")
        $('#DateValidator').html('ادخل  التاريخ');
        validation = false;
    }

    if (Terminal == "0") {
        $('.Terminal').addClass("is-invalid")
        $('#TerminalValidator').html('ادخل  تقطه الانطلاق');
        validation = false;
    }
    if (Time == "0") {
        $('.Time').addClass("is-invalid")
        $('#DateValidator').html('ادخل  الوقت');
        validation = false;
    }
    return validation;

}

const LOADSERVICES = _ => {

    axios.get(`${BASE_URL}/MobileService/Services`)
        .then(res => {
            let items = res.data;

            var dropdown = $('#serviceDrop');
            var service = "";
            var optGroup;

            for (var i = 0; i < items.length; i++) {
                if (items[i].service != service) {
                    optGroup = $("<optgroup/>");
                    optGroup.attr('label', items[i].service);
                }
                service = items[i].service;
                optGroup.append(
                    $('<option></option>').val(items[i].id).html(items[i].name)
                );

                dropdown.append(optGroup);
            };

        });
};

const LOADTerminal = _ => {

    axios.get(`${BASE_URL}/MobileService/Terminal`)
        .then(res => {
            let items = res.data;

            var dropdown = $('#TerminalDrop');
            var trip = "";
            var optGroup;

            for (var i = 0; i < items.length; i++) {
                optGroup = optGroup + "<option value = " + items[i].id + ">" + items[i].name + "</option>";
            };
            dropdown.append(optGroup);
        });
};

const LODDTrips = _ => {

    axios.get(`${BASE_URL}/MobileService/trips`)
        .then(res => {
            let items = res.data;

            var dropdown = $('#tripsDrop');
            var trip = "";
            var optGroup;

            for (var i = 0; i < items.length; i++) {
                optGroup = optGroup + "<option value = " + items[i].IdTrip + ">" + items[i].NameTrip + "</option>";
            };
            dropdown.append(optGroup);
        });
};
const LODDPAYS = _ => {

    axios.get(`${BASE_URL}/MobileService/paymints`)
        .then(res => {
            let items = res.data;

            var dropdown = $('#PaysDrop');
            var trip = "";
            var optGroup;

            for (var i = 0; i < items.length; i++) {
                optGroup = optGroup + "<option value = " + items[i].IdPay + ">" + items[i].NamePay + "</option>";
            };
            dropdown.append(optGroup);
        });
};
const TRANSACT = _ => {

    document.getElementById('msgDiv').style.display = 'none';
    document.getElementById('loader').style.display = 'block';


    let validation = APPLYVALIDATION();

    if (validation) {


        let PHONE = document.getElementById('mobileNumber').value;
        let SERVICE = document.getElementById('serviceDrop').value;
        let name = document.getElementById('name').value;
        let TRIP = document.getElementById('tripsDrop').value;
        let PAY = document.getElementById('PaysDrop').value;

        let model = {
            userId: USER.id,
            serviceId: SERVICE,
            IdTrip: TRIP,
            IdPay: PAY,
            customer: {
                phone: PHONE,
                name: name
            }
        };

        axios(`${BASE_URL}/MobileService/Transact`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(model)
        }).then(res => {
            document.getElementById('loader').style.display = 'none';
            document.getElementById('msgDiv').style.display = 'block';
            document.getElementById('mobileNumber').value = '';
            document.getElementById('serviceDrop').value = ''
            document.getElementById('name').value = ''
            document.getElementById('tripsDrop').value = ''
            document.getElementById('PaysDrop').value = ''
            setTimeout(function () {
                document.getElementById('msgDiv').style.display = 'none';
            }, 1000);
            if (res.data == 0) {
                alert('لا يمكن شراء تذكرة اكثر في هذه الرحلة.');
            } else {
                alert('تم الاضافه بنجاح');    
            }
          
        })

    } else {
        document.getElementById('loader').style.display = 'none';
    }
}



(function () {

    if (!USER) {
        location.href = LOGINVIEW;
        return;
    };

    $('#DateDrop').val(new Date().toLocaleDateString());

    LODDTrips();
    LOADTerminal();

})();

function getreport() {

    document.getElementById('msgDiv').style.display = 'none';
    document.getElementById('loader').style.display = 'block';


    let validation = APPLYVALIDATION();

    if (validation) {


        var TRIP = document.getElementById('tripsDrop').value;
        var TERMINALID = document.getElementById('TerminalDrop').value;

        if (TRIP > 0) {
            let model = {
                idtrip: TRIP,
                terminalId: TERMINALID
            }
            axios(`${BASE_URL}/MobileService/Report/`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: JSON.stringify(model)
            }).then(res => {
                document.getElementById('loader').style.display = 'none';
                let items = res.data;
                $("#items").empty();
                $("#items").append(items);
              
            });
        };
    }
    else {
        document.getElementById('loader').style.display = 'none';
    }
    
}

function getTime() {
    var serviceId = $("#TerminalDrop").val();
    if (serviceId != 0) {
        let model = {
            serviceId: serviceId
        }
        axios(`${BASE_URL}/MobileService/Time`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(model)
        }).then(res => {
            let items = res.data;

            var dropdown = $('#TimeDrop');
            var trip = "";
            var optGroup;

            for (var i = 0; i < items.length; i++) {
                let result = items[i].name.substring(0, 5);
                optGroup = optGroup + "<option value = " + items[i].id + ">" + result + "</option>";
            };
            dropdown.empty();
            dropdown.append(optGroup);
            getTripDrop();
        });
    }
}

function getTripDrop() {
    var TimeId = $("#TimeDrop").val();
    if (TimeId != 0) {
        let model = {
            serviceId: TimeId
        }
        axios(`${BASE_URL}/MobileService/TimeTrip`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(model)
        }).then(res => {
            let item = res.data;

            var dropdown = $('#tripsDrop');
            dropdown.val(item.id);
        });
    }
}