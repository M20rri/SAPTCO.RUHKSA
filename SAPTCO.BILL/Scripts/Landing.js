const USER = JSON.parse(sessionStorage.getItem('currentUser'));
const CHECkOUTENTITY = JSON.parse(sessionStorage.getItem('checkOutEntity'));

const PREPARECHECKOUT = _ => {

    document.getElementById('loader').style.display = 'block';

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
        const { SAPTCOResult: { code, description }, merchantTransactionId } = res.data;

        if (code === 'paid_delivered') {
            axios(`${BASE_URL}/MobileService/GenerateInvoice/${+merchantTransactionId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(result => {
                document.getElementById('loader').style.display = 'none';
                location.href = `${TICKETVIEW}/${merchantTransactionId}`;
            });
            return;
        }

    });

};


(function () {

    if (!USER) {
        location.href = LOGINVIEW;
        return;
    };
    PREPARECHECKOUT();
})();