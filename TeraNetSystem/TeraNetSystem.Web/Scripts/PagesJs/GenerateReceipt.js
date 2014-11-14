$("#btnRegisterPayment").on("click", function () {
    var selectedMonth = $('#MonthDropdown').children(':selected').text();

    var $content = $("#PaymentInfo").clone(true);
    $content.find('#buttonsBlock').hide();
    $content.find('#MonthDropdown').replaceWith(selectedMonth);
    console.log($content.html());

    var printWindow = window.open('', '', 'height=800,width=800');
    printWindow.document.write('<html><head><title>Payment</title>');
    printWindow.document.write('</head><body >');
    printWindow.document.write($content.html());
    printWindow.document.write('<div style="float:right;margin-right:20%">Stamp and sign:</div>')
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
});