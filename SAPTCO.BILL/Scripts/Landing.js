const USER = JSON.parse(sessionStorage.getItem('currentUser'));
const CHECkOUTENTITY = JSON.parse(sessionStorage.getItem('checkOutEntity'));


const PREPARECHECKOUT = _ => {

    document.getElementById('loader').style.display = 'block';
    document.getElementById('billDiv').style.display = 'none';
    document.getElementById('paymentDiv').style.display = 'none';

    let model = {
        PaymentMethod: CHECkOUTENTITY.paymentMethod,
        CheckoutId: CHECkOUTENTITY.checkoutId
    };

    axios(`${BASE_URL}/MobileService/PaymentStatus`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(model)
    }).then(res => {
        console.log(res.data);
        const { SAPTCOResult: { code, description }, merchantTransactionId } = res.data;

        if (code === 'paid_delivered') {
            axios(`${BASE_URL}/MobileService/GenerateInvoice/${+merchantTransactionId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(result => {
                document.getElementById('billDiv').style.display = 'block';
                document.getElementById('loader').style.display = 'none';
                const { response, url } = result.data;
                sessionStorage.setItem('downloadUrl', url);
            });
            return;
        }

        document.getElementById('paymentDiv').style.display = 'block';
        document.getElementById('code').innerHTML = code;
        document.getElementById('description').innerHTML = description;

    });

};

const DOWNLOAD = _ => {
    location.href = sessionStorage.getItem('downloadUrl');
}


(function () {

    if (!USER) {
        location.href = LOGINVIEW;
        return;
    };
    PREPARECHECKOUT();
})();