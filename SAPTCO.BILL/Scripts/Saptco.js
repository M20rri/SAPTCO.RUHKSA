const USER = JSON.parse(sessionStorage.getItem('currentUser'));
let cost = 0;


const APPLYVALIDATION = _ => {

    let validation = true;
    $('.phone , .service , .name , .TRIP , .PAy , .hotel , .countTiket , .Terminal , .Time').removeClass("is-invalid");

    let phoneRegx = '^(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$'; //regex

    let PHONE = document.getElementById('mobileNumber').value;
    let SERVICE = document.getElementById('serviceDrop').value;
    let TRIP = document.getElementById('tripsDrop').value;
    let PAY = document.getElementById('PaysDrop').value;
    let name = document.getElementById('name').value;
    let hotel = document.getElementById('hotelDrop').value;
    let countTiket = document.getElementById('countTiketDrop').value;
    let TerminalDrop = document.getElementById('TerminalDrop').value;
    let TimeDrop = document.getElementById('TimeDrop').value;


    if (name.length > 20) {
        $('.name').addClass("is-invalid")
        $('#nameValidator').html('الاسم اكبر من 20 حرف');
        validation = false;
    }

    if (TimeDrop == "0") {
        $('.Time').addClass("is-invalid")
        validation = false;
    }

    if (TerminalDrop == "0") {
        $('.Terminal').addClass("is-invalid")
        validation = false;
    }

    if (countTiket == "0") {
        $('.countTiket').addClass("is-invalid")
        validation = false;
    }

    if (hotel == "") {
        $('.hotel').addClass("is-invalid")
        validation = false;
    }


    if (!PHONE.match(phoneRegx)) {
        $('.phone').addClass("is-invalid");
        $('#mobileValidator').html('الرقم غير سعودى');
        validation = false;
    }

    if (PHONE == "") {
        $('.phone').addClass("is-invalid")
        $('#mobileValidator').html('ادخل رقم الهاتف');
        validation = false;
    }

    if (SERVICE == "0") {
        $('.service').addClass("is-invalid")
        validation = false;
    }
    if (TRIP == "") {
        $('.TRIP').addClass("is-invalid")
        validation = false;
    }
    if (PAY == "") {
        $('.PAy').addClass("is-invalid")
        validation = false;
    }



    if (name == "") {
        $('.name').addClass("is-invalid")
        validation = false;
    }

    return validation;

}

const APPLYHTPERPAYVALIDATION = _ => {

    let validation = true;
    $('.phone , .service , .name , .TRIP , .PAy , .hotel , .countTiket , .Terminal , .Time').removeClass("is-invalid");

    let phoneRegx = '^(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$'; //regex

    let PHONE = document.getElementById('mobileNumber').value;
    let SERVICE = document.getElementById('serviceDrop').value;
    let TRIP = document.getElementById('tripsDrop').value;
    let PAY = document.getElementById('PayTypeDrop').value;
    let name = document.getElementById('name').value;
    let hotel = document.getElementById('hotelDrop').value;
    let countTiket = document.getElementById('countTiketDrop').value;
    let TerminalDrop = document.getElementById('TerminalDrop').value;
    let TimeDrop = document.getElementById('TimeDrop').value;


    if (name.length > 20) {
        $('.name').addClass("is-invalid")
        $('#nameValidator').html('الاسم اكبر من 20 حرف');
        validation = false;
    }

    if (TimeDrop == "0") {
        $('.Time').addClass("is-invalid")
        validation = false;
    }

    if (TerminalDrop == "0") {
        $('.Terminal').addClass("is-invalid")
        validation = false;
    }

    if (countTiket == "0") {
        $('.countTiket').addClass("is-invalid")
        validation = false;
    }

    if (hotel == "") {
        $('.hotel').addClass("is-invalid")
        validation = false;
    }


    if (!PHONE.match(phoneRegx)) {
        $('.phone').addClass("is-invalid");
        $('#mobileValidator').html('الرقم غير سعودى');
        validation = false;
    }

    if (PHONE == "") {
        $('.phone').addClass("is-invalid")
        $('#mobileValidator').html('ادخل رقم الهاتف');
        validation = false;
    }

    if (SERVICE == "0") {
        $('.service').addClass("is-invalid")
        validation = false;
    }
    if (TRIP == "") {
        $('.TRIP').addClass("is-invalid")
        validation = false;
    }
    if (PAY == "") {
        $('.PAy').addClass("is-invalid")
        validation = false;
    }



    if (name == "") {
        $('.name').addClass("is-invalid")
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
                optGroup = optGroup + "<option value = " + items[i].id + ">" + items[i].service + "</option>";
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

const TRANSACT = _ => {

    document.getElementById('msgDiv').style.display = 'none';
    document.getElementById('loader').style.display = 'block';


    let validation = APPLYVALIDATION();

    if (validation) {


        let PHONE = document.getElementById('mobileNumber').value;
        let SERVICE = document.getElementById('hotelDrop').value;
        let name = document.getElementById('name').value;
        let TRIP = document.getElementById('tripsDrop').value;
        let PAY = document.getElementById('PaysDrop').value;
        let countTiket = document.getElementById('countTiketDrop').value;
        let TerminalDrop = document.getElementById('TerminalDrop').value;
        let TimeDrop = document.getElementById('TimeDrop').value;

        let model = {
            userId: USER.id,
            serviceId: SERVICE,
            IdTrip: TRIP,
            IdPay: PAY,
            countTiket: countTiket,
            idTerminal: TerminalDrop,
            idTime: TimeDrop,
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
            document.getElementById('serviceDrop').value = '';
            document.getElementById('name').value = '';
            document.getElementById('tripsDrop').value = '';
            document.getElementById('PaysDrop').value = '';

            document.getElementById('hotelDrop').value = '';
            document.getElementById('countTiketDrop').value = '';
            document.getElementById('TerminalDrop').value = '';
            document.getElementById('TimeDrop').value = '';
            setTimeout(function () {

            }, 1000);
            if (res.data == 0) {
                document.getElementById('msgDiv').style.display = 'contents';
                $("#msg").empty();
                $("#msg").append("لا يمكن شراء تذاكر اكثر في هذه الرحلة.");
                //  document.getElementById('msgDiv').innerText('لا يمكن شراء تذكرة اكثر في هذه الرحلة.');
                //  alert('لا يمكن شراء تذكرة اكثر في هذه الرحلة.');
            } else {
                document.getElementById('msgDiv').style.display = 'contents';
                $("#msg").empty();
                $("#msg").append("تم الاضافه بنجاح");
            }

        })

    } else {
        document.getElementById('loader').style.display = 'none';
    }
}

const PREPARECHECKOUT = _ => {

    document.getElementById('loader').style.display = 'block';

    let validation = APPLYHTPERPAYVALIDATION();

    if (validation) {

        let PHONE = document.getElementById('mobileNumber').value;
        let SERVICE = document.getElementById('hotelDrop').value;
        let name = document.getElementById('name').value;
        let PAY = document.getElementById('PayTypeDrop').value;
        let countTiket = document.getElementById('countTiketDrop').value;

        let model = {
            Merchand: {
                PaymentMethod: PAY,
                GivenName: name,
                SureName: name,
                Phone: PHONE
            },
            Quantity: countTiket,
            ServiceId: +SERVICE
        }

        console.log(model)

        axios(`${BASE_URL}/MobileService/CheckOut`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(model)
        }).then(res => {
            document.getElementById('loader').style.display = 'none';
            console.log(res.data);
            const { creatFormResponse, id: checkoutId } = res.data;
            $('#checkOutPanel').html('');
            $('#createPaymentPanel').html(creatFormResponse);
            let checkOutEntity = { checkoutId, paymentMethod: PAY };
            sessionStorage.setItem('checkOutEntity', JSON.stringify(checkOutEntity));
        });

    } else {
        document.getElementById('loader').style.display = 'none';
    }
}

function getHotel() {

    var serviceId = $("#serviceDrop").val();

    if (serviceId != 0) {

        let model = {
            serviceId: serviceId
        };

        axios(`${BASE_URL}/MobileService/Hotels`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(model)
        }).then(res => {

            let items = res.data;
            var dropdown = $('#hotelDrop');
            var trip = "";
            var optGroup;

            for (var i = 0; i < items.length; i++) {
                optGroup = optGroup + "<option value = " + items[i].id + ">" + items[i].name + "</option>";
            };
            dropdown.empty();
            dropdown.append('<option value="">اختر ...</option>');
            dropdown.append(optGroup);
        });
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


            axios(`${BASE_URL}/MobileService/reminder/${item.id}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(result => {
                let tripCount = result.data;
                if (tripCount > 20) {
                    tripCount = 20;
                }
                var optGroup;
                var countTrips = $('#countTiketDrop');
                optGroup = "<option value = '0'>اختر ...</option>";
                for (var i = 1; i <= tripCount; i++) {

                    optGroup = optGroup + "<option value = " + i + ">" + i + "</option>";
                };
                countTrips.empty();
                countTrips.append(optGroup);

            })
        });
    }
}

(function () {

    if (!USER) {
        location.href = LOGINVIEW;
        return;
    };

    LOADSERVICES();
    LODDTrips();
    LODDPAYS();
    LOADTerminal();
    $("#name").keypress(function (e) {
        var key = e.keyCode;
        if (key >= 48 && key <= 57) {
            e.preventDefault();
        }
    });
})();