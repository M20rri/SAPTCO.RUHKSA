const LOGIN = _ => {

    document.getElementById('loader').style.display = 'block';

    let validation = APPLYVALIDATION();
    if (validation) {

        var model = {
            username: document.getElementById('usernameTxt').value,
            password: document.getElementById('passwordTxt').value
        };

        axios(`${BASE_URL}/MobileService/Auth`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(model)
        }).then(res => {
            let result = res.data;
            const { code, message } = result;
            document.getElementById('loader').style.display = 'none';
            if (code == 0) {
                document.getElementById('msgDiv').style.display = 'block';
            } else {
                document.getElementById('msgDiv').style.display = 'none';
                sessionStorage.setItem('currentUser', message);
                location.href = INDEXVIEW;
            }
        });
    } else {
        document.getElementById('loader').style.display = 'none';
    };
}

const APPLYVALIDATION = _ => {

    let validation = true;
    $('.usn , .pwd').removeClass("d-block");

    let USERNAME = document.getElementById('usernameTxt').value;
    let PASSWORD = document.getElementById('passwordTxt').value;

    if (USERNAME == "") {
        $('.usn').addClass("d-block")
        validation = false;
    }

    if (PASSWORD == "") {
        $('.pwd').addClass("d-block")
        validation = false;
    }

    return validation;

};