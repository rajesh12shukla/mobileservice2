


$(function () {
    $("#MoveRight,#MoveLeft").click(function (event) {
        var id = $(event.target).attr("id");
        var selectFrom = id == "MoveRight" ? "#from" : "#to";
        var moveTo = id == "MoveRight" ? "#to" : "#from";

        var selectedItems = $(selectFrom + " :selected").toArray();
        $(moveTo).append(selectedItems);
        selectedItems.remove;
    });
});
//function moveUp() {
$(function () {
    $("#MoveUp").click(function (event) {
        $('#ctl00_ContentPlaceHolder1_lstColumnSort :selected').each(function (i, selected) {
            if (!$(this).prev().length) return false;
            $(this).insertBefore($(this).prev());
        });
        $('#ctl00_ContentPlaceHolder1_lstColumnSort select').focus().blur();

    });
});

//function moveDown() {
$(function () {
    $("#MoveDown").click(function (event) {
        $($('#ctl00_ContentPlaceHolder1_lstColumnSort :selected').get().reverse()).each(function (i, selected) {
            if (!$(this).next().length) return false;
            $(this).insertAfter($(this).next());
        });
        $('#ctl00_ContentPlaceHolder1_lstColumnSort select').focus().blur();

    });
});
$(function () {
    $("#ReadAll").click(function (event) {
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function (index) {
            //  if ( ($(this).is(':selected')) {
            //    // do stuff if selected
            //  }
            //  else {
            //   // this one isn't selected, do other stuff
            //  }
            alert($(this).val() + '--' + $(this).text());
        });
    });
});
$(function () {

    $("#Delete").click(function (event) {
        $('#ctl00_ContentPlaceHolder1_lstColumnSort :selected').each(function (i, selected) {

            $(this).remove();
            alert('Selected item deleted');
            $('#ctl00_ContentPlaceHolder1_lstColumnSort').focus().blur();

        });
    });
});


function UserDeleteConfirmation() {
    return confirm("Are you sure you want to delete this report?");
}

function EmptyReportName() {
    debugger;
    //Changed by Yashasvi Jadav
    var _reportName = $('#ctl00_ContentPlaceHolder1_txtReportName').val();
    var _hdnName = $("#ctl00_ContentPlaceHolder1_hdnCustomizeReportName").val();
    alert(_reportName + " | " + _hdnName);
    return false;
    if ($("#ctl00_ContentPlaceHolder1_txtReportName").val() == "") {
        alert("Report name cann't be empty.");
        return false;
    }
    else {
        return true;
    }
}

//function EmptyEmailBox() {
//    if ($("#ctl00_ContentPlaceHolder1_txtEmails").val() == "") {
//        alert("Report name cann't be empty.");
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function EmptyFilters() {
    $("#tblName").css("display", "none");
    //$("#ctl00_ContentPlaceHolder1_drpName").val('All');
    $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').attr('checked', false);
    $("#tblCity").css("display", "none");
    //            $("#ctl00_ContentPlaceHolder1_txtCity").val('');
    $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').attr('checked', false);
    $("#tblState").css("display", "none");
    //$("#ctl00_ContentPlaceHolder1_ddlState").val('All');
    $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').attr('checked', false);
    $("#tblZip").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtZip").val('');
    $("#tblPhone").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtPhone").val('');
    $("#tblFax").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtFax").val('');
    $("#tblContact").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtContact").val('');
    $("#tblAddress").css("display", "none");
    //            $("#ctl00_ContentPlaceHolder1_txtAddress").val('');
    $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').attr('checked', false);
    $("#tblEmail").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtEmail").val('');
    $("#tblCountry").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtCountry").val('');
    $("#tblWebsite").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtWebsite").val('');
    $("#tblCellular").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtCellular").val('');
    $("#tblCategory").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_drpCategory").val('Category');
    $("#tblType").css("display", "none");
    // $("#ctl00_ContentPlaceHolder1_drpType").val('All');
    $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);

    $("#tblBalance").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');

    $("#tblStatus").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_drpStatus").val('All');

    $("#tblLocationId").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationId input[type="checkbox"]').attr('checked', false);

    $("#tblLocationName").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationName input[type="checkbox"]').attr('checked', false);

    $("#tblLocationAddress").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationAddress input[type="checkbox"]').attr('checked', false);

    $("#tblLocationCity").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationCity input[type="checkbox"]').attr('checked', false);

    $("#tblLocationState").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').attr('checked', false);

    $("#tblLocationZip").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtLocationZip").val('');

    $("#tblLocationType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationType input[type="checkbox"]').attr('checked', false);

    $("#tblEquipmentName").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpEquipmentName input[type="checkbox"]').attr('checked', false);

    $("#tblManuf").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpManuf input[type="checkbox"]').attr('checked', false);

    $("#tblEquipmentType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpEquipmentType input[type="checkbox"]').attr('checked', false);

    $("#tblServiceType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpServiceType input[type="checkbox"]').attr('checked', false);

    $("#tblLoc").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');

    $("#tblEquip").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');

    $("#tblOpenCalls").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');


    $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", "disabled");


    $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", "disabled");

    $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", "disabled");

    $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", "disabled");

    $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", "disabled");
}

function setFilters(filterName) {
    //    if (filterName == "Name") {
    //        $("#tblName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblName").css("display", "none");
    //    }

    //    if (filterName == "City") {
    //        $("#tblCity").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCity").css("display", "none");
    //    }

    //    if (filterName == "State") {
    //        $("#tblState").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblState").css("display", "none");
    //    }

    //    if (filterName == "Zip") {
    //        $("#tblZip").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblZip").css("display", "none");
    //    }

    //    if (filterName == "Phone") {
    //        $("#tblPhone").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblPhone").css("display", "none");
    //    }

    //    if (filterName == "Fax") {
    //        $("#tblFax").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblFax").css("display", "none");
    //    }

    //    if (filterName == "Contact") {
    //        $("#tblContact").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblContact").css("display", "none");
    //    }

    //    if (filterName == "Address") {
    //        $("#tblAddress").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblAddress").css("display", "none");
    //    }

    //    if (filterName == "Email") {
    //        $("#tblEmail").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblEmail").css("display", "none");
    //    }

    //    if (filterName == "Country") {
    //        $("#tblCountry").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCountry").css("display", "none");
    //    }

    //    if (filterName == "Website") {
    //        $("#tblWebsite").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblWebsite").css("display", "none");
    //    }

    //    if (filterName == "Cellular") {
    //        $("#tblCellular").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCellular").css("display", "none");
    //    }


    //    if (filterName == "Category") {
    //        $("#tblCategory").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCategory").css("display", "none");
    //    }

    //    if (filterName == "Type") {
    //        $("#tblType").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblType").css("display", "none");
    //    }

    //    if (filterName == "Balance") {
    //        $("#tblBalance").fadeIn(200).css("display", "block");
    //        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
    //            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
    //            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", "disabled");
    //            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", "disabled");
    //            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", "disabled");
    //        }

    //    }
    //    else {
    //        $("#tblBalance").css("display", "none");
    //    }


    //    if (filterName == "Status") {
    //        $("#tblStatus").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblStatus").css("display", "none");
    //    }

    //    if (filterName == "LocationId") {
    //        $("#tblLocationId").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationId").css("display", "none");
    //    }

    //    if (filterName == "LocationName") {
    //        $("#tblLocationName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationName").css("display", "none");
    //    }

    //    if (filterName == "LocationAddress") {
    //        $("#tblLocationAddress").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationAddress").css("display", "none");
    //    }

    //    if (filterName == "LocationCity") {
    //        $("#tblLocationCity").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationCity").css("display", "none");
    //    }

    //    if (filterName == "LocationState") {
    //        $("#tblLocationState").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationState").css("display", "none");
    //    }

    //    if (filterName == "LocationZip") {
    //        $("#tblLocationZip").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationZip").css("display", "none");
    //    }

    //    if (filterName == "LocationType") {
    //        $("#tblLocationType").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationType").css("display", "none");
    //    }

    //    if (filterName == "EquipmentName") {
    //        $("#tblEquipmentName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblEquipmentName").css("display", "none");
    //    }

    //    if (filterName == "EquipmentName") {
    //        $("#tblEquipmentName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblEquipmentName").css("display", "none");
    //    }


    $("#Div2 table").css("display", "none");
    if (filterName != "loc" || filterName != "equip" || filterName != "opencall") {
        $("#tbl" + filterName).fadeIn(200).css("display", "block");
    }

    if (filterName == "Balance") {
        $("#tblBalance").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {
    //        $("#tblBalance").css("display", "none");
    //    }

    if (filterName == "loc") {
        $("#tblLoc").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtLocEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblLoc").css("display", "none");
    //    }

    if (filterName == "equip") {
        $("#tblEquip").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtEquipEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblEquip").css("display", "none");
    //    }

    if (filterName == "opencall") {
        $("#tblOpenCalls").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtOCEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblOpenCalls").css("display", "none");
    //    }

    if (filterName == "EquipmentPrice") {
        $("#tblEquipmentPrice").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", "disabled");
        }

    }
}

function setFiltersValue(filterName, filterValue) {

    if (filterName == "Name") {
        //$("#ctl00_ContentPlaceHolder1_txtName").val(filterValue);
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName option:contains('" + filterValue + "')").prop('selected', true));
        // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpName input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "City") {
        //                $("#ctl00_ContentPlaceHolder1_txtCity").val(filterValue);
        $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedCity = filterValue.split("|");
            for (i = 0; i <= splitedCity.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpCity input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedCity[i] = splitedCity[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedCity[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "State") {
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState option:contains('" + filterValue + "')").prop('selected', true));
        //  filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_ddlState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    actualText = splitedNames[i].replace("'", "").replace("'", "");
                    // alert(actualText);
                    // alert(actualText.trim().length);
                    if (actualText.trim().length <= 2) {

                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option[value=" + actualText.trim() + "]").attr("selected", "selected");
                    }
                    else {
                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText.trim() + "')").attr("selected", "selected")
                    }

                    var getText = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").text();
                    // alert(getText);
                    if (lblValue.trim() == getText.trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').attr('checked', false);
        }

    }

    if (filterName == "LocationState") {
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState option:contains('" + filterValue + "')").prop('selected', true));
        //  filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedLocStates = filterValue.split("|");
            for (i = 0; i <= splitedLocStates.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpLocationState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    actualText = splitedLocStates[i].replace("'", "").replace("'", "");
                    // alert(actualText);
                    // alert(actualText.trim().length);
                    if (actualText.trim().length <= 2) {

                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option[value=" + actualText.trim() + "]").attr("selected", "selected");
                    }
                    else {
                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText.trim() + "')").attr("selected", "selected")
                    }

                    var getText = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").text();
                    // alert(getText);
                    if (lblValue.trim() == getText.trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').attr('checked', false);
        }

    }

    if (filterName == "Zip") {
        $("#ctl00_ContentPlaceHolder1_txtZip").val(filterValue);
    }

    if (filterName == "Phone") {
        $("#ctl00_ContentPlaceHolder1_txtPhone").val(filterValue);
    }

    if (filterName == "Fax") {
        $("#ctl00_ContentPlaceHolder1_txtFax").val(filterValue);
    }

    if (filterName == "Contact") {
        $("#ctl00_ContentPlaceHolder1_txtContact").val(filterValue);
    }

    if (filterName == "Address") {
        //                $("#ctl00_ContentPlaceHolder1_txtAddress").val(filterValue);
        $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedAddress = filterValue.split("|");
            for (i = 0; i <= splitedAddress.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpAddress input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedAddress[i] = splitedAddress[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedAddress[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Email") {
        $("#ctl00_ContentPlaceHolder1_txtEmail").val(filterValue);
    }

    if (filterName == "Country") {
        $("#ctl00_ContentPlaceHolder1_txtCountry").val(filterValue);
    }

    if (filterName == "Website") {
        $("#ctl00_ContentPlaceHolder1_txtWebsite").val(filterValue);
    }

    if (filterName == "Cellular") {
        $("#ctl00_ContentPlaceHolder1_txtCellular").val(filterValue);
    }

    if (filterName == "Category") {
        filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpCategory").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpCategory option:selected").val(filterValue));
    }

    if (filterName == "Type") {
        // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpType").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpType option:contains('" + filterValue + "')").prop('selected', true));
        $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedTypes = filterValue.split("|");
            for (i = 0; i <= splitedTypes.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpType input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedTypes[i] = splitedTypes[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedTypes[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Balance") {
        if (filterValue == "") {
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedBalance = '';
            var greaterValue = '';
            var lessValue = '';
            splitedBalance = filterValue.split('and');
            greaterValue = splitedBalance[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedBalance[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }


    if (filterName == "loc") {
        if (filterValue == "") {
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
        }
            //else if ((filterValue.contains(">=") || filterValue.contains("&gt;")) && !filterValue.contains('and')) {
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedLoc = '';
            var greaterValue = '';
            var lessValue = '';
            splitedLoc = filterValue.split('and');
            greaterValue = splitedLoc[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedLoc[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "equip") {
        if (filterValue == "") {
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", false);
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedEquip = '';
            var greaterValue = '';
            var lessValue = '';
            splitedEquip = filterValue.split('and');
            greaterValue = splitedEquip[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedEquip[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", false);
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "opencall") {
        if (filterValue == "") {
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedOC = '';
            var greaterValue = '';
            var lessValue = '';
            splitedOC = filterValue.split('and');
            greaterValue = splitedOC[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedOC[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);

        }

    }

    if (filterName == "EquipmentPrice") {
        if (filterValue == "") {
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedOC = '';
            var greaterValue = '';
            var lessValue = '';
            splitedOC = filterValue.split('and');
            greaterValue = splitedOC[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedOC[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);

        }

    }


    if (filterName == "Status") {
        filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpStatus").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpStatus  option:contains('" + filterValue + "')").prop('selected', true));
    }


    //  if (filterName == "LocationId") {
    // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpType").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpType option:contains('" + filterValue + "')").prop('selected', true));

    if (filterName == "LocationId" || filterName == "LocationName" || filterName == "LocationAddress" || filterName == "LocationCity" || filterName == "LocationZip" || filterName == "LocationType" || filterName == "EquipmentName" || filterName == "Manuf" || filterName == "EquipmentType" || filterName == "ServiceType") {

        $('#ctl00_ContentPlaceHolder1_drp' + filterName + ' input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedValues = filterValue.split("|");
            for (i = 0; i <= splitedValues.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drp' + filterName + ' input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedValues[i] = splitedValues[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedValues[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drp' + filterName + ' input[type="checkbox"]').attr('checked', false);
        }
    }

}


$(function () {
    var appendthis = ("<div class='modal-overlay js-modal-close'></div>");

    $('a[data-modal-id]').click(function (e) {
        e.preventDefault();
        $("body").append(appendthis);
        //$(".modal-overlay").fadeTo(500, 0.7);
        //$(".js-modalbox").fadeIn(500);
        var modalBox = $(this).attr('data-modal-id');
        $('#' + modalBox).fadeIn($(this).data());
    });


    $(".js-modal-close, .modal-overlay").click(function () {
        $(".modal-box, .modal-overlay").fadeOut(500, function () {
            $(".modal-overlay").remove();
        });
    });

    $(window).resize(function () {
        $(".modal-box").css({
            top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
            left: ($(window).width() - $(".modal-box").outerWidth()) / 2
        });
    });

    $(window).resize();

});


jQuery(document).ready(function () {

    $('#colorNav #dynamicUI li').remove();
    $('#colorNav #dynamicUI').append('<li><a onClick="ExportToPDF();"><img src="images/PDFIcon.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Export to PDF</span><div style="clear:both;"></div></a></li>')
    $('#colorNav #dynamicUI').append('<li><a onClick="ExportToExcel();"><img src="images/ExcelIcon.jpg" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Export to Excel</span><div style="clear:both;"></div></a></li>')

    $('#dvBtnEmail #dvEmailOptions li').remove();

    $('#dvBtnEmail #dvEmailOptions').append('<li><a id="btnSendPDFReport" href="#" onClick="SendPDFReport(this.id);"><img src="images/EmailPDFIcon.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Send report as PDF</span><div style="clear:both;"></div></a></li>')
    $('#dvBtnEmail #dvEmailOptions').append('<li><a id="btnSendExcelReport" href="#" onClick="SendPDFReport(this.id);"><img src="images/EmailExcelIcon.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Send report as Excel</span><div style="clear:both;"></div></a></li>')

    if ($("#ctl00_ContentPlaceHolder1_hdnMainHeader").val() == 'True') {
        $("#dvMainHeader").show();
    }
    else {
        $("#dvMainHeader").hide();
    }
    var getColumnWidth = $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val();
    var splittedColWidth = '';
    if (getColumnWidth != '') {
        splittedColWidth = $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val().split(",");

        $('#tblResize tr th').each(function (index, element) {
            var colWidth = splittedColWidth[index];
            $(this).css('width', colWidth);
        });
    }

    var columnList = $("#ctl00_ContentPlaceHolder1_hdnColumnList").val();
    $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
    var splitedColList = columnList.split(",");
    $.each(splitedColList, function (key, value) {

        $('#ctl00_ContentPlaceHolder1_lstColumnSort')
                                            .append($("<option></option>")
                                 .attr("value", value)
                                 .text(value));

    });

    //$("#ctl00_ContentPlaceHolder1_lstFilter option[value='loc'],option[value='equip'], option[value='opencall']").remove();

    if ($("#ctl00_ContentPlaceHolder1_drpReports").val() == "0") {
        $("#btnCustomizeReport").hide();
        $("#ctl00_ContentPlaceHolder1_btnDeleteReport").hide();
        $("#ctl00_ContentPlaceHolder1_btnSaveReport2").hide();
        $("#btnPrint").hide();
        $("#dvBtnEmail").hide();
        $("#colorNav").hide();
    }
    else {
        $("#btnCustomizeReport").show();
        $("#ctl00_ContentPlaceHolder1_btnDeleteReport").show();
        $("#ctl00_ContentPlaceHolder1_btnSaveReport2").show();

        $("#dvBtnEmail").show();
        $("#colorNav").show();
    }


    $('[name="ctl00$ContentPlaceHolder1$CrystalReportViewer1$ctl02$ctl11"]').hide();
    $('[name="ctl00$ContentPlaceHolder1$CrystalReportViewer1$ctl02$ctl03"]').hide();

    jQuery('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = jQuery(this).attr('href');

        // Show/Hide Tabs
        jQuery('.tabs ' + currentAttrValue).show().siblings().hide();

        // Change/remove current tab to active
        jQuery(this).parent('li').addClass('active').siblings().removeClass('active');

        e.preventDefault();
    });

    //$("#btnPrint").click(function() {
    //    $("#ctl00_ContentPlaceHolder1_dvGridReport").print();
    //    return (false);
    //});


    $("#btnApply").click(function () {
        var isChecked = null;
        var chkArray = [];

        $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]:checked').each(function () {
            isChecked = true;
            // chkArray.push($(this).parent().find('label').html());
        });
        if (isChecked) {

            $("#popup").hide();
            $("#ctl00_ContentPlaceHolder1_dvSaveReport").show();
            var drpSortByVal = $('#ctl00_ContentPlaceHolder1_drpSortBy option:selected').val();
            $("#ctl00_ContentPlaceHolder1_hdnDrpSortBy").val(drpSortByVal);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_dvSaveReport").hide();
            alert("Please select at least one field!");
            return;
        }

        var lstSortColumn = '';
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function (index, element) {
            //alert($(element).val());
            lstSortColumn += $(element).val() + "^";

        });

        $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstSortColumn);

        var lstColumnWidth = '';
        $('#tblResize tr th').each(function () {
            lstColumnWidth += $(this).css('width') + "^";
        });

        $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

        var filterColumns = '';
        var filterValues = '';
        var checkIndex = '';
        $('#tblFilterChoices tr').each(function () {
            filterColumns += $(this).find("td:first").html() + "^";
            checkIndex = $(this).find("td:first").html();
            if (checkIndex.indexOf("State") == -1) {
                filterValues += $(this).find("td:last").html() + "^";
                //alert(filterValues);
            }
            else {
                var getStates = $(this).find("td:last").html() + "^";
                var splitedStates = getStates.split("|");
                for (i = 0; i < splitedStates.length; i++) {
                    actualText = $.trim(splitedStates[i].replace("'", "").replace("'", "").replace("^", ""));
                    $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText + "')").attr("selected", "selected")
                    var getValue = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").val();
                    filterValues += "'" + getValue + "'" + "|";
                }
                filterValues = filterValues.substring(0, filterValues.length - 1) + "^";
            }
        });

        $("#ctl00_ContentPlaceHolder1_hdnFilterColumns").val(filterColumns);
        $("#ctl00_ContentPlaceHolder1_hdnFilterValues").val(filterValues);

    });

    $("#btnCancel2").click(function () {
        $("#popup").show();
    });

    $("#btnNewReport").click(function () {
        $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
        $("#ctl00_ContentPlaceHolder1_hdnReportAction").val('Save');
        $("INPUT[type='checkbox']").attr('checked', false);
        $("#ctl00_ContentPlaceHolder1_txtReportName").val('');
        $('#spnModelTitle').html('New Report: Customer');

        var radio = $("[id*=rdbOrders] input[value=1]");
        radio.attr("checked", "checked");
        EmptyFilters();
        $("#tblFilterChoices tbody").empty();
        $("#ctl00_ContentPlaceHolder1_lstFilter").val('Name');
        $("#tblName").fadeIn(200).css("display", "block");
        //                $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", "disabled");

        $("#ctl00_ContentPlaceHolder1_chkMainHeader").attr("checked", true);
        $("#ctl00_ContentPlaceHolder1_chkDatePrepared").attr("checked", true);
        $("#ctl00_ContentPlaceHolder1_chkTimePrepared").attr("checked", true);

    });



    $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() == '') {
                    $("#trBalance").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
        }
        else {
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
            $("#trBalance").remove();

        }
    });

    $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtLocEqual").val() == '') {
                    $("#trloc").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
            $("#trloc").remove();

        }
    });

    $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtEquipEqual").val() == '') {
                    $("#trequip").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtequipLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
            $("#trequip").remove();

        }
    });

    $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtOCEqual").val() == '') {
                    $("#tropencall").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
            $("#tropencall").remove();

        }
    });

    $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val() == '') {
                    $("#trEquipmentPrice").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
            $("#trEquipmentPrice").remove();
        }
    });

    $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpType input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("type")) {
                $(".type").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trType"><td width="100px" style="height:25px;">Type</td><td  class="type" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trType").remove();
        }
    });

    $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpName input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("name")) {
                $(".name").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trName"><td width="100px" style="height:25px;">Name</td><td  class="name" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trName").remove();
        }
    });

    $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpCity input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("city")) {
                $(".city").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCity"><td width="100px" style="height:25px;">City</td><td  class="city" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCity").remove();
        }
    });

    $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpAddress input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("address")) {
                $(".address").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trAddress"><td width="100px" style="height:25px;">Address</td><td  class="address" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trAddress").remove();
        }
    });

    $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_ddlState input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("state")) {
                $(".state").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trState"><td width="100px" style="height:25px;">State</td><td  class="state" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trState").remove();
        }
    });

    function SetFiltersChoicesFromDropDown(ControlId, cls) {
        var filterValue = '';
        $(' ' + ControlId + ' input[type=checkbox]:checked + label').each(function () {
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass(cls)) {
                $("." + cls).html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tr' + cls + '"><td width="100px" style="height:25px;">' + cls + '</td><td  class=' + cls + ' width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#tr" + cls + "").remove();
        }
    }

    $('#ctl00_ContentPlaceHolder1_drpLocationId input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationId", "LocationId");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationName input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationName", "LocationName");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationAddress input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationAddress", "LocationAddress");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationCity input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationCity", "LocationCity");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationState", "LocationState");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationZip input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationZip", "LocationZip");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationType", "LocationType");
    });

    $('#ctl00_ContentPlaceHolder1_drpEquipmentName input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpEquipmentName", "EquipmentName");
    });

    $('#ctl00_ContentPlaceHolder1_drpManuf input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpManuf", "Manuf");
    });

    $('#ctl00_ContentPlaceHolder1_drpEquipmentType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpEquipmentType", "EquipmentType");
    });

    $('#ctl00_ContentPlaceHolder1_drpServiceType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpServiceType", "ServiceType");
    });

    $("#btnCustomizeReport").click(function () {

        $("#ctl00_ContentPlaceHolder1_chkColumnList INPUT[type='checkbox']").attr('checked', false);
        // var reportValue = $("#ctl00_ContentPlaceHolder1_drpReports option:selected").text();
        $("#ctl00_ContentPlaceHolder1_hdnReportAction").val('Update');
        var reportName = $("#ctl00_ContentPlaceHolder1_hdnCustomizeReportName").val();
        $('#spnModelTitle').html('Modify Report : ' + reportName);
        $("#ctl00_ContentPlaceHolder1_txtReportName").val(reportName);
        var columnList = $("#ctl00_ContentPlaceHolder1_hdnColumnList").val();

        var splitedColList = columnList.split(",");
        for (i = 0; i <= splitedColList.length; i++) {
            $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]').each(function () {
                var lblValue = $(this).parent().find('label').html();
                if (lblValue == splitedColList[i]) {
                    $(this).attr('checked', true);
                }
            });
        }

        $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
        $("#ctl00_ContentPlaceHolder1_lstColumnSort option").remove();

        $.each(splitedColList, function (key, value) {

            $('#ctl00_ContentPlaceHolder1_drpSortBy')
         .append($("<option></option>")
         .attr("value", value)
         .text(value));


            $('#ctl00_ContentPlaceHolder1_lstColumnSort')
                                            .append($("<option></option>")
                                 .attr("value", value)
                                 .text(value));

        });

        if ($('#tblResize tr').length > 0) {
            $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
            $('#tblResize tr th').each(function () {
                $('#ctl00_ContentPlaceHolder1_lstColumnSort')
                                    .append($("<option></option>")
                         .attr("value", $(this).html())
                         .text($(this).html()));
            });
        }



        $('#ctl00_ContentPlaceHolder1_drpSortBy').val($("#ctl00_ContentPlaceHolder1_hdnDrpSortBy").val());
        if (filters.length == 0) {
            $("#tblFilterChoices tbody").empty();
            $("#ctl00_ContentPlaceHolder1_lstFilter").val('Name');
            EmptyFilters();
            $("#tblName").fadeIn(200).css("display", "block");
        }
        else {
            $("#tblFilterChoices tbody").empty();

            $(filters).each(function (index, filter) {
                if (filter.FilterValues != '') {
                    if (filter.FilterColumns != "State" && filter.FilterColumns != "LocationState") {
                        $('#tblFilterChoices tbody').append('<tr  id="tr' + filter.FilterColumns + '"><td width="100px" style="height:25px;">' + filter.FilterColumns + '</td><td  class="' + filter.FilterColumns.toLowerCase() + '" width="220px"  style="height:25px;">' + filter.FilterValues + '</td></tr>');
                    }
                    else {
                        var getStates = filter.FilterValues;
                        var splitedStates = getStates.split("|");
                        var getText = '';
                        for (i = 0; i < splitedStates.length; i++) {
                            actualText = $.trim(splitedStates[i].replace("'", "").replace("'", "").replace(",", ""));
                            $("#ctl00_ContentPlaceHolder1_ddlStateReference option[value= " + actualText + "]").attr("selected", "selected")
                            getText += "'" + $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").text() + "'" + " | ";

                        }
                        if (filter.FilterColumns == "State") {
                            $('#tblFilterChoices tbody').append('<tr  id="tr' + filter.FilterColumns + '"><td width="100px" style="height:25px;">' + filter.FilterColumns + '</td><td  class="' + filter.FilterColumns.toLowerCase() + '" width="220px"  style="height:25px;">' + getText.trim().substring(0, getText.trim().length - 1) + '</td></tr>');
                        }
                        else {
                            $('#tblFilterChoices tbody').append('<tr  id="tr' + filter.FilterColumns + '"><td width="100px" style="height:25px;">' + filter.FilterColumns + '</td><td  class="' + filter.FilterColumns + '" width="220px"  style="height:25px;">' + getText.trim().substring(0, getText.trim().length - 1) + '</td></tr>');
                        }
                        //alert(getText);
                    }
                    setFiltersValue(filter.FilterColumns, filter.FilterValues);
                }
            });

            $('#tblFilterChoices tr:first').each(function () {
                filterColumns = $(this).find("td:first").html()
                setFilters(filterColumns);
                $("#ctl00_ContentPlaceHolder1_lstFilter").val(filterColumns);
            });
        }
    });

    $('#ctl00_ContentPlaceHolder1_chkColumnList').change(function () {
        $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
        //  var lstColumnValues = '';
        $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                $('#ctl00_ContentPlaceHolder1_drpSortBy').append($("<option></option>").attr("value", $(this).parent().find('label').html()).text($(this).parent().find('label').html()));

                $('#ctl00_ContentPlaceHolder1_lstColumnSort').append($("<option></option>").attr("value", $(this).parent().find('label').html()).text($(this).parent().find('label').html()));

            }

        });

    });


    $("#ctl00_ContentPlaceHolder1_lstFilter").change(function () {
        var filterName = $("#ctl00_ContentPlaceHolder1_lstFilter option:selected").text();

        setFilters(filterName);
    });

    $('#tblFilterChoices').on('click', 'tr', function (event) {
        if ($(this).attr('style'))
            $(this).removeAttr('style');

        var selected = $(this).hasClass("highlight");

        $("#tblFilterChoices tr").removeClass("highlight");
        if (!selected) {
            $(this).addClass("highlight");
        }

        var filterName = $(this).find("td:first").html();
        var filterValue = $(this).find("td:last").html();
        //EmptyFilters();
        setFilters(filterName);
        setFiltersValue(filterName, filterValue);
        $("#ctl00_ContentPlaceHolder1_lstFilter").val(filterName);
    });

    $("#btnRemoveFilter").click(function () {
        $('#tblFilterChoices tr').each(function () {
            var selected = $(this).hasClass("highlight");
            if (selected) {
                var filterName = $(this).find("td:first").html();
                $(this).remove();
                setFiltersValue(filterName, "");
            }
        });
    });

    $("#ctl00_ContentPlaceHolder1_txtZip").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtZip").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("zip")) {
                $(".zip").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trZip"><td width="100px" style="height:25px;">Zip</td><td  class="zip" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trZip").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtPhone").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtPhone").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("phone")) {
                $(".phone").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trPhone"><td width="100px" style="height:25px;">Phone</td><td  class="phone" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trPhone").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtFax").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtFax").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("fax")) {
                $(".fax").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trFax"><td width="100px" style="height:25px;">Fax</td><td  class="fax" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trFax").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtContact").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtContact").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("contact")) {
                $(".contact").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trContact"><td width="100px" style="height:25px;">Contact</td><td  class="contact" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trContact").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEmail").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEmail").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("email")) {
                $(".email").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEmail"><td width="100px" style="height:25px;">Email</td><td  class="email" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trEmail").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtCountry").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtCountry").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("country")) {
                $(".country").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCountry"><td width="100px" style="height:25px;">Country</td><td  class="country" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCountry").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtWebsite").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtWebsite").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("website")) {
                $(".website").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trWebsite"><td width="100px" style="height:25px;">Website</td><td  class="website" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trWebsite").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtCellular").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtCellular").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("cellular")) {
                $(".cellular").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCellular"><td width="100px" style="height:25px;">Cellular</td><td  class="cellular" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCellular").remove();
        }
    });

    $("#ctl00_ContentPlaceHolder1_drpCategory").change(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_drpCategory option:selected").text();
        if (filterValue != 'All') {
            if ($('#tblFilterChoices tr td').hasClass("category")) {
                $(".category").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCategory"><td width="100px" style="height:25px;">Category</td><td  class="category" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCategory").remove();
        }
    });


    $("#ctl00_ContentPlaceHolder1_txtBalEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBalEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name=ctl00$ContentPlaceHolder1$Balance]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance")) {
                $(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name=ctl00$ContentPlaceHolder1$Balance]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val();
                $(".balance").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".balance").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".balance").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '') {
                $("#trBalance").remove();
            }
            else {
                $(".balance").html("<=" + $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name=ctl00$ContentPlaceHolder1$Balance]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val()
                $(".balance").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".balance").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".balance").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
                $("#trBalance").remove();
            }
            else {
                $(".balance").html(">=" + $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val());
            }
        }
    });


    $("#ctl00_ContentPlaceHolder1_drpStatus").change(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_drpStatus option:selected").text();
        if (filterValue != 'All') {
            if ($('#tblFilterChoices tr td').hasClass("status")) {
                $(".status").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trStatus"><td width="100px" style="height:25px;">Status</td><td  class="status" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trStatus").remove();
        }
    });


    $("#ctl00_ContentPlaceHolder1_txtLocEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLocEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name=ctl00$ContentPlaceHolder1$loc]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc")) {
                $(".loc").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name=ctl00$ContentPlaceHolder1$loc]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val();
                $(".loc").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".loc").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".loc").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '') {
                $("#trloc").remove();
            }
            else {
                $(".loc").html("<=" + $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name=ctl00$ContentPlaceHolder1$loc]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val()
                $(".loc").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".loc").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".loc").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
                $("#trloc").remove();
            }
            else {
                $(".loc").html(">=" + $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val());
            }
        }
    });




    $("#ctl00_ContentPlaceHolder1_txtEquipEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name=ctl00$ContentPlaceHolder1$equip]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip")) {
                $(".equip").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });


    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name=ctl00$ContentPlaceHolder1$equip]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val();
                $(".equip").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equip").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equip").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equip").html("<=" + $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name=ctl00$ContentPlaceHolder1$equip]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val()
                $(".equip").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equip").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equip").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equip").html(">=" + $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val());
            }
        }
    });



    $("#ctl00_ContentPlaceHolder1_txtOCEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtOCEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name=ctl00$ContentPlaceHolder1$oc]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall")) {
                $(".opencall").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });




    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name=ctl00$ContentPlaceHolder1$oc]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val();
                $(".opencall").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".opencall").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".opencall").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '') {
                $("#tropencall").remove();
            }
            else {
                $(".opencall").html("<=" + $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name=ctl00$ContentPlaceHolder1$oc]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val()
                $(".opencall").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".opencall").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".opencall").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
                $("#tropencall").remove();
            }
            else {
                $(".opencall").html(">=" + $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val());
            }
        }
    });




    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name=ctl00$ContentPlaceHolder1$ep]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice")) {
                $(".equipmentprice").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });




    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name=ctl00$ContentPlaceHolder1$ep]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val();
                $(".equipmentprice").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equipmentprice").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentprice").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '') {
                $("#trEquipmentPrice").remove();
            }
            else {
                $(".equipmentprice").html("<=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name=ctl00$ContentPlaceHolder1$ep]:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val()
                $(".equipmentprice").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equipmentprice").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentprice").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
                $("#trEquipmentPrice").remove();
            }
            else {
                $(".equipmentprice").html(">=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val());
            }
        }
    });

    //    $("#ctl00_ContentPlaceHolder1_txtLocationZip").blur(function() {
    //        SetFiltersChoicesFromTextBox("#ctl00_ContentPlaceHolder1_txtLocationZip", "LocationZip");
    //    });

    //    function SetFiltersChoicesFromTextBox(ControlId, cls) {
    //        var filterValue = $(ControlId).val();
    //        if (filterValue != '') {
    //            if ($('#tblFilterChoices tr td').hasClass(cls)) {
    //                $("." + cls).html(filterValue);
    //            }
    //            else {
    //                $('#tblFilterChoices tbody').append('<tr id="tr' + cls + '"><td width="100px" style="height:25px;">' + cls + '</td><td  class=' + cls + ' width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
    //            }
    //        }
    //        else {
    //            $("#tr" + cls + "").remove();
    //        }
    //    }


    /////////////////////// Header/Footer /////////////////////////
    $("#ctl00_ContentPlaceHolder1_chkCompanyName").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtCompanyName").attr("disabled", false);
        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtCompanyName").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtCompanyName").val('');
        }
    });

    $("#ctl00_ContentPlaceHolder1_chkReportTitle").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtReportTitle").attr("disabled", false);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtReportTitle").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtReportTitle").val('');
        }
    });


    $("#ctl00_ContentPlaceHolder1_chkSubtitle").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtSubtitle").attr("disabled", false);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtSubtitle").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtSubtitle").val('');
        }
    });

    $("#ctl00_ContentPlaceHolder1_chkDatePrepared").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_drpDatePrepared").attr("disabled", false);
        }
        else {
            $("#ctl00_ContentPlaceHolder1_drpDatePrepared").attr("disabled", true);
        }
    });


    $("#ctl00_ContentPlaceHolder1_chkPageNumber").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_drpPageNumber").attr("disabled", false);
        }
        else {
            $("#ctl00_ContentPlaceHolder1_drpPageNumber").attr("disabled", true);
        }
    });

    $("#ctl00_ContentPlaceHolder1_chkExtraFootLine").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtExtraFooterLine").attr("disabled", false);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtExtraFooterLine").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExtraFooterLine").val('');
        }
    });

});

