const USER = JSON.parse(sessionStorage.getItem('currentUser'));
const CHECkOUTENTITY = JSON.parse(sessionStorage.getItem('checkOutEntity'));
const TBLBODY = document.getElementById('tblBody');
const DOWNLOADSPAN = document.getElementById('downloadSpan');


const PREPARECHECKOUT = _ => {

    let url = window.location.pathname;
    let invoiceId = url.substring(url.lastIndexOf('/') + 1);


    document.getElementById('loader').style.display = 'block';
    document.getElementById('billDiv').style.display = 'none';
    document.getElementById('paymentDiv').style.display = 'none';
    axios(`${BASE_URL}/MobileService/GenerateTicketTable/${invoiceId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(result => {
        TBLBODY.innerHTML = '';
        DOWNLOADSPAN.innerHTML = '';

        const { TicketUrl, InvoiceUrl } = result.data;

        TBLBODY.innerHTML += `${HTMLTICKET(TicketUrl)}`;
        DOWNLOADSPAN.innerHTML = `${HTMLINVOICE(InvoiceUrl)}`

    });

};

const HTMLTICKET = (row) => {
    return `
    <tr>
        <th scope="row"><a href="${row.Url}">Ticekt</a></th>
    </tr>
    `;
};

const HTMLINVOICE = (url) => {

    document.getElementById('loader').style.display = 'none';
    document.getElementById('billDiv').style.display = 'block';

    return `
            <a href="${url}" class="btn btn-link mt-2"><b>التحميل</b></button>
    `;
};


(function () {

    if (!USER) {
        location.href = LOGINVIEW;
        return;
    };
    PREPARECHECKOUT();
})();