/* Add here all your JS customizations */
jQuery(document).ready(function ($) {
    $(".invoiceItemIds").change(function () {
        var myId = $(this).attr('id');
        var myIdNum = myId.replace("InvoiceItemId", "");
        var selectedClass = "Selected" + myIdNum;

        var myVal = $(this).val();
        if (myVal == "") {
            $("." + selectedClass).removeAttr('readonly');
        } else {
            $("." + selectedClass).attr('readonly', "readonly");
        }
    });
});