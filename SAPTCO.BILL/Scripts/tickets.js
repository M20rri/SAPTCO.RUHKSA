const USER = JSON.parse(sessionStorage.getItem('currentUser'));
const CHECkOUTENTITY = JSON.parse(sessionStorage.getItem('checkOutEntity'));
const TBLBODY = document.getElementById('tblBody');
const DOWNLOADSPAN = document.getElementById('downloadSpan');


const PREPARECHECKOUT = _ => {

    document.getElementById('loader').style.display = 'block';
    document.getElementById('billDiv').style.display = 'none';
    document.getElementById('paymentDiv').style.display = 'none';

    axios(`${BASE_URL}/MobileService/GenerateTicketTable/${id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(result => {
        document.getElementById('loader').style.display = 'none';
        document.getElementById('paymentDiv').style.display = 'block';
        console.log(result.data);
        TBLBODY.innerHTML = '';
        DOWNLOADSPAN.innerHTML = '';
        for (let ticketUrl of result.data) {
            TBLBODY.innerHTML += `${HTMLTICKET(ticketUrl)}`;
        };
    });

};

const DOWNLOAD = _ => {
    location.href = sessionStorage.getItem('downloadUrl');
}

const HTMLTICKET = (row) => {
    return `
    <tr>
        <th scope="row"><a href="${row.Url}">Ticekt_${row.Id}</a></th>
    </tr>
    `;
};

(function () {

    if (!USER) {
        location.href = LOGINVIEW;
        return;
    };
    PREPARECHECKOUT();
})();