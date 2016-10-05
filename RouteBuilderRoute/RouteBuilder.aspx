<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true"
    CodeFile="RouteBuilder.aspx.cs" Inherits="RouteBuilder" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>       
        body, html {
          height: 100%;
          width: 100%;
        }

        /*	start styles for the ContextMenu	*/
        .context_menu
        {
            background-color: white;
            border: 1px solid gray;
        }
        .context_menu_item
        {
            padding: 3px 6px;
        }
        .context_menu_item:hover
        {
            background-color: #CCCCCC;
        }
        .context_menu_separator
        {
            background-color: gray;
            height: 1px;
            margin: 0;
            padding: 0;
        }
        #clearDirectionsItem, #getDirectionsItem
        {
            display: none;
        }
        /*	end styles for the ContextMenu	*/
        
    </style>

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>

    <script type="text/javascript" src="js/ContextMenu.js"></script>

    <script type="text/javascript">
    var coordi  =  [<asp:Repeater ID="rptWaypts" runat="server">
                <ItemTemplate>('<%# Eval("coordinates") %>')</ItemTemplate><SeparatorTemplate>,</SeparatorTemplate>                
                </asp:Repeater>];    
                
    var markers =[
    <asp:Repeater ID="rptMarkers" runat="server">
    <ItemTemplate>
             {
                "title": '<%# Eval("tagrep") %>',
                "lat": '<%# Eval("lat") %>',
                "lng": '<%# Eval("lng") %>' ,
                "description": '<%# Eval("address") %>', 
                "worker": '<%# Eval("worker") %>'            
            }
    </ItemTemplate>
    <SeparatorTemplate>
        ,
    </SeparatorTemplate>
    </asp:Repeater>
    ];
    </script>

    <script type="text/javascript">
            
        var GooglemarkersArray = [];
        var markerselected = new Array();
        var map;
        var coord = new Array();
        //        var waypointorder = []; 
        var directionsDisplay;
        var directionsService = new google.maps.DirectionsService();
        directionsDisplay = new google.maps.DirectionsRenderer();
        var cityCircle;
        var contextMenu;
        var contextMenuCircle;

        function initialize() {
            cityCircle = new google.maps.Circle();
            GooglemarkersArray = [];
            markerselected = new Array();
            coord = new Array();

            var mapOptions = {
                zoom: 6,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: new google.maps.LatLng(0, 0),
                scrollwheel: true
            }
            map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

            if (document.getElementById('<%=hdnSwitchMethods.ClientID%>').value == "0") {
                markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
                AddMarker(markers, 1, 'NONE');
            }
            else {
                calcRoute();
            }

            AddContextMenu();

            //            seen = []
            //           json= JSON.stringify(GooglemarkersArray[0], function(key, val) {
            //                if (typeof val == "object") {
            //                    if (seen.indexOf(val) >= 0)
            //                        return
            //                    seen.push(val)
            //                }
            //                return val
            //            })
            //            alert(json);
        }
        google.maps.event.addDomListener(window, 'load', initialize);

        function JSON2Marker() {
            GooglemarkersArray = [];
            markers = JSON.parse(document.getElementById('<%=hdnMarkers.ClientID%>').value);
            AddMarker(markers, 1, 'NONE');
        }

        function MarkerToArray(lat, lng, title, desc, chk, loc, row, worker) {
            var chkSelect = document.getElementById(chk);
            if (chkSelect.checked == true) {
                clearOverlays();

                var markeraddarrey = [{ "title": title, "lat": lat, "lng": lng, "description": desc, "loc": loc, "worker": worker}];

                markerselected.push(markeraddarrey[0]);

                AddMarker(markerselected, 1, 'DROP');
                $('#' + row).attr("onclick", "GridHover(" + (markerselected.length - 1) + ")");

            }
            else {
                clearOverlays();
                var c = 0
                $(markerselected).each(function(msindex) {

                    if (this["loc"] == loc) {
                        c = 1;
                        markerselected.splice(msindex, 1);
                    }
                    if (c > 0) {

                        var grid = document.getElementById('<%=gvLocations.ClientID%>');
                        if (grid.rows.length > 0) {
                            for (var i = 1; i < grid.rows.length - 1; i++) {
                                var cell = grid.rows[i].cells[0];
                                for (var j = 0; j < cell.childNodes.length; j++) {
                                    if (cell.childNodes[j].className == "loc") {
                                        if (cell.childNodes[j].innerHTML == this["loc"]) {
                                            $('#' + grid.rows[i].id).attr("onclick", "GridHover(" + (msindex - 1) + ")");
                                        }
                                    }
                                }
                            }
                        }

                    }

                });

                AddMarker(markerselected, 0, 'NONE');
                document.getElementById(row).removeAttribute("onclick");
            }
        }

        function AddMarker(markers, boundmap, animation) {
            var infoWindow = new google.maps.InfoWindow({ maxWidth: 300 });
            var bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < markers.length; i++) {
                var data = markers[i];
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title,
                    icon: "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=|" + GetColor(data.worker) + "|ffffff"
                    //                    ,animation: google.maps.Animation.DROP
                });
                (function(marker, data) {
                    google.maps.event.addListener(marker, "click", function(e) {
                    infoWindow.setContent('<div STYLE=width:200px; height:150px><DIV  width:200px; height:150px><i>' + data.worker + '</i></br><b>' + data.title + '</b></br>' + data.description + '</DIV></DIV>');
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
                bounds.extend(myLatlng);
                GooglemarkersArray.push(marker);                
            }
            if (boundmap == 1) {
                map.fitBounds(bounds);
            }
        }

        function AddAssignedMarker() {
           
            var markers = JSON.parse(document.getElementById('<%=hdnAssignedMarkers.ClientID%>').value);
            
            var infoWindow = new google.maps.InfoWindow({ maxWidth: 300 });
            for (var i = 0; i < markers.length; i++) {
                var data = markers[i];
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title,                    
                    icon: "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=T|" + GetColor(document.getElementById('<%=hdnAssignedWorker.ClientID%>').value.split(';')[1]) + "|000"
                });                
                (function(marker, data) {
                    google.maps.event.addListener(marker, "click", function(e) {
                    infoWindow.setContent('<div STYLE=width:200px; height:150px><DIV  width:200px; height:150px>Newly assigned worker: <i>' + document.getElementById('<%=hdnAssignedWorker.ClientID%>').value.split(';')[1] + '</i></br><b>' + data.title + '</b></br>' + data.description + '</DIV></DIV>');
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
        }       

        function GetColor(worker) {
            var color;
            workers = JSON.parse(document.getElementById('<%=hdnColor.ClientID%>').value);
            $(workers).each(function(ind) {
                if (this["worker"] == worker) {
                    color = this["color"];
                }
            });
            return color;
        }

        function OptmizeRoute() {
            var valid = false;
            valid = ValidateSelection('chkSelect', 'Location');
            if (valid == false)
                return;

            valid = ValidateSelection('chkGridOrigin', 'Origin');
            if (valid == false)
                return;

            valid = ValidateSelection('chkGridDest', 'Destination');
            if (valid == false)
                return;

            coord = [];
            clearOverlays();
            var grid = document.getElementById('<%=gvLocations.ClientID%>');
            if (grid.rows.length > 0) {
                for (var i = 1; i < grid.rows.length - 1; i++) {
                    var cell = grid.rows[i].cells[0];
                    var CellOrigin = grid.rows[i].cells[3];
                    var CellDest = grid.rows[i].cells[4];
                    var CellLocName = grid.rows[i].cells[1];
                    var CellLocAdd = grid.rows[i].cells[2];

                    for (var j = 0; j < cell.childNodes.length; j++) {

                        if (cell.childNodes[j].type == "checkbox") {
                            if (cell.childNodes[j].checked == true) {
                                var Cord = cell.childNodes[3].value;
                                if (Cord != '') {

                                    var chkorigin = false;
                                    var chkDest = false;
                                    var loc = '';
                                    var locname = '';
                                    var locaddress = '';
                                    var lat;
                                    var lng;

                                    for (var k = 0; k < CellOrigin.childNodes.length; k++) {
                                        if (CellOrigin.childNodes[k].className == "origin") {
                                            chkorigin = CellOrigin.childNodes[k].childNodes[0].checked;
                                        }
                                    }
                                    for (var l = 0; l < CellDest.childNodes.length; l++) {
                                        if (CellDest.childNodes[l].className == "dest") {
                                            chkDest = CellDest.childNodes[l].childNodes[0].checked;
                                        }
                                    }
                                    for (var m = 0; m < cell.childNodes.length; m++) {
                                        if (cell.childNodes[m].className == "loc") {
                                            loc = cell.childNodes[m].innerHTML;
                                        }
                                    }

                                    locname = CellLocName.childNodes[1].innerHTML;
                                    locaddress = CellLocAdd.childNodes[1].innerHTML;
                                    lat = Cord.split(",")[0];
                                    lng = Cord.split(",")[1];

                                    var corditem = [{ "coordinates": "" + Cord + "", "origin": chkorigin, "dest": chkDest, "loc": loc, "name": locname, "addr": locaddress, "lat": lat, "lng": lng, "title": locname, "description": locaddress}];
                                    coord.push(corditem[0]);
                                }
                            }
                        }
                    }
                }
            }
            calcRoute();
        }

        function calcRoute() {
            //            var directionsDisplay;
            //            var directionsService = new google.maps.DirectionsService();
            //            directionsDisplay = new google.maps.DirectionsRenderer();

            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById('directions-panel'));

            var waypointorder = [];
            var wayptsloc = [];
            var orderedLoc = [];
            var waypts = [];
            var start;
            var end;

            for (var i = 0; i < coord.length; i++) {
                if (coord[i].origin == false && coord[i].dest == false) {
                    waypts.push({
                        location: coord[i].coordinates,
                        stopover: true
                    });
                    wayptsloc.push(coord[i]);
                }
                if (coord[i].origin == true) {
                    start = coord[i].coordinates;
                }
                if (coord[i].dest == true) {
                    end = coord[i].coordinates;
                }
            }

            var request = {
                origin: start,
                destination: end,
                waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: google.maps.TravelMode.DRIVING
            };

            directionsService.route(request, function(response, status) {
                if (status == google.maps.DirectionsStatus.OK) {

                    waypointorder = response.routes[0].waypoint_order;
                    directionsDisplay.setDirections(response);

                    $(coord).each(function(ms1index) {
                        if (this["origin"] == true) {
                            orderedLoc.push(this);
                        }
                    });

                    $(waypointorder).each(function(ms2index) {
                        orderedLoc.push(wayptsloc[this]);
                    });

                    $(coord).each(function(ms3index) {
                        if (this["dest"] == true) {
                            orderedLoc.push(this);
                        }
                    });

                    var sequence = [];
                    var chararray = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
                    var content = '<table  class="altrowstable" cellspacing="0" border="1" style="border-collapse:collapse; width : 430px;" rules="all">'
                    content += '<tr><th></th><th style="width:130px;">Name</th><th style="width:180px;">Address</th></tr>';
                    $(orderedLoc).each(function(ms4index) {
                        sequence.push(this["loc"]);
                        var css = 'evenrowcolor';
                        if (ms4index % 2 != 0)
                            css = 'oddrowcolor'

                        content += '<tr class=' + css + '><td><p>' + chararray[ms4index] + '</p></td><td ><p style="width:130px; word-wrap:break-word;">' + this["name"] + '</p></td> <td ><p style="width:220px; word-wrap:break-word;">' + this["addr"] + '</p></td></tr>';
                    });
                    content += '</table>'
                    $('#Optresult').empty();
                    $('#Optresult').append(content);

                    $('#<%=hdnRouteSeq.ClientID%>').val(sequence);
                }
                else {
                    alert(status);
                }
            });
        }



        //            $(coord).each(function(msindex) {
        //                if (this["dest"] == false && this["origin"] == false) {
        //                    locArray.push(this["loc"]);
        //                }
        //            });



        //            var waypts = [];
        //            for (var i = 1; i < coord.length - 1; i++) {
        //                waypts.push({
        //                    location: coord[i],
        //                    stopover: true
        //                });
        //            }

        //            var start = coord[0];
        //            var end = coord[coord.length - 1];
        //        }


        function GridHover(i) {
            google.maps.event.trigger(GooglemarkersArray[i], "click");
        }

        function clearOverlays() {
            for (var i = 0; i < GooglemarkersArray.length; i++) {
                GooglemarkersArray[i].setMap(null);
            }
            GooglemarkersArray = [];

            if (markerselected.length == 0) {
                clearGridHover();
            }
        }

        function clearGridHover() {
            var grid = document.getElementById('<%=gvLocations.ClientID%>');
            if (grid.rows.length > 0) {
                for (var i = 1; i < grid.rows.length - 1; i++) {
                    grid.rows[i].removeAttribute("onclick");
                }
            }
        }

        function checkAll(checktoggle) {
            var checkboxes = new Array();
            checkboxes = document.getElementsByTagName('input');

            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox') {
                    checkboxes[i].checked = checktoggle;
                }
            }
        }

        function toggleSelectionGrid(gridview, chkLike, source) {

            var isChecked = $("#" + source).is(":checked");

            $("#" + gridview + " input[id*='" + chkLike + "']").each(function(index) {
                $(this).attr('checked', false);
            });
            $("#" + source).prop('checked', isChecked);
        }

        function SelectionLimit(gridview, chkLike, source) {
            var isChecked = $("#" + source).is(":checked");
            var count = 0;
            $("#" + gridview + " input[id*='" + chkLike + "']").each(function(index) {
                if ($(this).is(":checked") == true) {
                    count++;
                }
            });
            if (count >= 11) {
                alert('You can select only 10 locations.');
                $("#" + source).prop('checked', false);
            }
        }

        function CheckOrigDest(chkSelectID, chkOrigID, chkDescID) {

            var chkSelect = $("#" + chkSelectID).is(":checked");
            var chkOrig = $("#" + chkOrigID);
            var chkDesc = $("#" + chkDescID);

            if (chkSelect == true) {
                chkDesc.prop('disabled', false);
                chkOrig.prop('disabled', false);
            }
            else {
                chkDesc.prop('disabled', true);
                chkOrig.prop('disabled', true);
                chkDesc.prop('checked', false);
                chkOrig.prop('checked', false);
            }
        }

        function ValidateSelection(chkLike, msg) {
            var count = 0;
            $("#<%=gvLocations.ClientID%> input[id*='" + chkLike + "']").each(function(index) {
                if ($(this).is(":checked") == true) {
                    count++;
                }
            });
            if (count == 0) {
                alert('Please select the ' + msg + '.');
                return false;
            }
            else {
                return true;
            }
        }



        function DeleteTemplate() {
            var ddltemplate = $("#<%=ddlTemplates.ClientID%>").val();
            if (ddltemplate == "0" || ddltemplate == "-1") {
                alert('Please select template to delete.');
                return false;
            }
            else {
                return confirm('Are you sure you want to delete the selected template?');
            }
        }

        function AddContextMenu() {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItems = [];
            menuItems.push({ className: 'context_menu_item', eventName: 'add_circle_here', label: 'Add Circle Here' });
            menuItems.push({});
            menuItems.push({ className: 'context_menu_item', eventName: 'center_map_click', label: 'Center map here' });
            contextMenuOptions.menuItems = menuItems;

            contextMenu = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(map, 'rightclick', function(mouseEvent) {
                contextMenu.show(mouseEvent.latLng);
            });
            eventRightClick(contextMenu);
        }

        function AddContextMenuCircle() {
            var contextMenuOptions = {};
            contextMenuOptions.classNames = { menu: 'context_menu', menuSeparator: 'context_menu_separator' };

            var menuItemsCircle = [];
            menuItemsCircle.push({ className: 'context_menu_item', eventName: 'clear_circle', label: 'Clear Circle' });
            menuItemsCircle.push({});
            menuItemsCircle.push({ className: 'context_menu_item', eventName: 'assign_worker', label: 'Assign Worker' });
            contextMenuOptions.menuItems = menuItemsCircle;

            contextMenuCircle = new ContextMenu(map, contextMenuOptions);

            google.maps.event.addListener(cityCircle, 'rightclick', function(mouseEvent) {
                contextMenuCircle.show(mouseEvent.latLng);
            });

            google.maps.event.addListener(cityCircle, 'click', function(mouseEvent) {
                contextMenuCircle.hide();
            });

            eventRightClick(contextMenuCircle);
        }

        function eventRightClick(contextMenu) {
            google.maps.event.addListener(contextMenu, 'menu_item_selected', function(latLng, eventName) {
                switch (eventName) {
                    case 'add_circle_here':
//                        var zoom = map.getZoom();
//                        var radius = 200 / (zoom/1000);                        
                        AddCircle(latLng, 50);
                        break;
                    case 'center_map_click':
                        map.panTo(latLng);
                        break;
                    case 'clear_circle':
                        cityCircle.setMap(null);
                        break;
                    case 'assign_worker':
                        $find('PMPBehaviour').show();
                        break;
                }
            });
        }

        function AddCircleLatLng(latLng, radius) {

            var lat = latLng.split(",")[0];
            var lng = latLng.split(",")[1];
            AddCircle(new google.maps.LatLng(lat, lng), parseFloat(radius));
            map.fitBounds(cityCircle.getBounds());
        }

        function AddCircle(latLng, radius) {
            cityCircle.setMap(null);
            var populationOptions = {
                strokeColor: '#000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FFF',
//              fillOpacity: 0.35,
                fillOpacity: 0.0,
                map: map,
//              center: citymap[city].center,
                center: latLng,
                radius: radius,
//              radius: citymap[city].population / 20,
                draggable: true,
                editable: true
            };
            cityCircle = new google.maps.Circle(populationOptions);
            
            AddContextMenuCircle();            
        }

        function assignClick() {
            $('#<%=hdnCenter.ClientID%>').val(cityCircle.getCenter().lat() + ',' + cityCircle.getCenter().lng());
            $('#<%=hdnRadius.ClientID%>').val((cityCircle.getRadius()));            
            $('#<%=btnAssign.ClientID%>').click();
        }

        function validateSave() {
            var txtTemplate = $("#<%=txtTemplate.ClientID%>").val();
            if (txtTemplate == "") {
                alert('Please enter the template name.');
                return false;
            }
            else {
                return confirm('Are you sure you want to save the template?');
            }

        }

        function ClickTemplateDropdown() {
            var hdnEdited = $("#<%=hdnEdited.ClientID%>").val();
            if (hdnEdited == "1") {
                if ($("#<%=hdnPreviousTempl.ClientID%>").val() == "0" || $("#<%=hdnPreviousTempl.ClientID%>").val() == "-1") {
                    return confirm('You have not saved the new template. Do you want to continue without saving and move to existing template?');
                }
                else {
                    return confirm('There are unsaved changes to the current template. Do you want to continue without saving?');
                }
            }
            else {
                return true;
            }
        }

        function DisplayConfirmation() {
            if (ClickTemplateDropdown() == true) {
                __doPostBack("<%=ddlTemplates.ClientID%>", '');
            }
            else {
                $("#<%=ddlTemplates.ClientID%>").val($('#<%= hdnPreviousTempl.ClientID%>').val());
            }
        }

        function ClickAssignCheck() {
            var template = $("#<%=ddlTemplates.ClientID%>").val();
            if (template == "0" || template == "-1") {
            }
            else {
                var conf= confirm('Do you want to create new template? Ckick Ok to create new template and Cancel to make changes to the selected template.');

                if (conf == true) {
                    $("#<%=ddlTemplates.ClientID%>").val('0');                                       
                }
            }
        }

        function AssignedMarker() {
            if ($('#<%=chkMap.ClientID%>').is(":checked")) {
                AddAssignedMarker();
            }
            else {

            }
        }

        //Initial load of page
        $(document).ready(sizeContent);
       
        //Every resize of window
        $(window).resize(sizeContent);

        function sizeContent() {
            var contentHeight = $(".content").height();
            var newHeight = $("html").height() - ($("#footer").height() + $("#header_main").height()) - 50; // + "px";
            
            if (contentHeight < newHeight) {
                $(".content").css("height", newHeight + "px");
            }
        }       

    </script>

    <asp:HiddenField ID="hdnSwitchMethods" runat="server" Value="0" />
    <asp:HiddenField ID="hdnRouteSeq" runat="server" />
    <table class="roundCorner" style="width:100%; height:100% ">
        <tr style="height:100%">
            <td valign="top" style="height:100%" >
                <table class="roundCorner shadow" style="background-color: #EBEBEB; height:100%">
                    <tr>
                        <td valign="top">
                            <div>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="Server" TargetControlID="pnlSidePanel"
                                            ExpandControlID="pnlCollapse" CollapseControlID="pnlCollapse" Collapsed="false" 
                                            ExpandDirection="Horizontal" SuppressPostBack="true" ExpandedImage="~/images/coll.png"
                                            CollapsedImage="~/images/expand.png" ImageControlID="imgCollapse" />
                                        <asp:CollapsiblePanelExtender ID="cpeLocations" runat="Server" TargetControlID="PnlgridLoc"
                                            ExpandControlID="imgExpand" CollapseControlID="imgExpand" Collapsed="false" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow_down.png" CollapsedImage="~/images/arrow_right.png"
                                            ImageControlID="imgExpand" />
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlWorkerGrid"
                                            ExpandControlID="imgExpandWork" CollapseControlID="imgExpandWork" Collapsed="false"
                                            SuppressPostBack="true" ExpandedImage="~/images/arrow_down.png" CollapsedImage="~/images/arrow_right.png"
                                            ImageControlID="imgExpandWork" />
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="Server" TargetControlID="pnlTemplateGrid"
                                            ExpandControlID="imgExpandTemp" CollapseControlID="imgExpandTemp" Collapsed="false"
                                            SuppressPostBack="true" ExpandedImage="~/images/arrow_down.png" CollapsedImage="~/images/arrow_right.png"
                                            ImageControlID="imgExpandTemp" />
                                        <asp:HiddenField ID="hdnMarkers" runat="server" />
                                        <asp:HiddenField ID="hdnAssignedMarkers" runat="server" />
                                        <asp:HiddenField ID="hdnColor" runat="server" />
                                        <asp:Panel ID="pnlSidePanel" runat="server">
                                            <div style="margin: 0 0 0 5px; padding: 5px; width: 719px;">
                                                <asp:Panel Visible="false" runat="server" ID="pnlGridButtons" Style="border-style: solid solid none solid;
                                                    background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px;
                                                    color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px;
                                                    border-color: #a9c6c9;">
                                                    <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                                                        ID="Label1" runat="server">Locations</asp:Label>
                                                    <a id="lnkOptimize" onclick="OptmizeRoute();" style="cursor: pointer; float: right;
                                                        display: none; color: #2382B2; margin-right: 20px; margin-left: 10px;" title="Optimize route for selected locations">
                                                        Optimize Route</a>
                                                    <asp:LinkButton Visible="false" Style="float: right; color: #2382B2; margin-right: 20px;
                                                        margin-left: 10px;" ID="btnOptimize" runat="server" ToolTip="Optimize route for selected locations"
                                                        OnClick="btnOptimize_Click">Optimize Route</asp:LinkButton>
                                                </asp:Panel>
                                                <div class="roundCorner">
                                                    <asp:Panel ID="pnlGridLocHandle" runat="server" Style="background: #ccc; width: 100%;
                                                        height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px;
                                                        border-width: 1px; border-color: #a9c6c9;">
                                                        <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                                                            ID="Label2" runat="server">Locations</asp:Label>
                                                        <asp:Image ID="imgExpand" runat="server" Style="float: right; margin-right: 10px;
                                                            height: 20px; cursor: pointer" />
                                                        <asp:DropDownList ID="ddlWorker" runat="server" ToolTip="Select default worker" AutoPostBack="true"
                                                            Visible="true" TabIndex="14" Width="150px" Style="float: left" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtSearchLoc" runat="server" Width="280px" 
                                                            placeholder="Search"></asp:TextBox>
                                                        <asp:ImageButton ID="btnSearch" Width="11px" runat="server" ImageUrl="images/Black_Search.png"
                                                            ToolTip="Search" OnClick="btnSearch_Click"></asp:ImageButton>
                                                        <asp:ImageButton ID="btnClear" Width="10px" runat="server" ImageUrl="images/cross.png"
                                                            ToolTip="Search" Style="left: -31px; position: relative;" OnClick="btnClear_Click">
                                                        </asp:ImageButton>
                                                        <asp:Label ID="lblTotalRecLoc" runat="server" Text="Label" Style="color: #fff; float: right;
                                                            margin-right: 20px"></asp:Label>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlGridLoc" runat="server">
                                                        <div style="min-height:145px; max-height:400px; overflow-y: scroll">
                                                            <asp:GridView ID="gvLocations" runat="server" AutoGenerateColumns="False" CssClass="altrowstable"
                                                                DataKeyNames="loc" ShowFooter="True" Width="100%" 
                                                                AllowPaging="True" onpageindexchanging="gvLocations_PageIndexChanging">
                                                                <RowStyle CssClass="evenrowcolor rowpointer" />
                                                                <FooterStyle CssClass="footer" />
                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                <AlternatingRowStyle CssClass="oddrowcolor rowpointer" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkAllSelect" Visible="false" runat="server" onclick="checkAll(false);" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" Visible="false" runat="server" />
                                                                            <asp:HiddenField ID="hdnCoordinate" runat="server" Value='<%# Bind("coordinates") %>' />
                                                                            <asp:Label ID="lblLoc" CssClass="loc" Style="display: none" runat="server" Text='<%# Eval("loc") %>' />
                                                                            <asp:Label ID="lblworker" CssClass="worker" Style="display: none" runat="server"
                                                                                Text='<%# Eval("worker") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Name" SortExpression="name" HeaderStyle-Width="130px"
                                                                        ItemStyle-Width="130px" ItemStyle-Wrap="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" Style="display: none" runat="server" Text='<%#Eval("tag")%>'></asp:Label>
                                                                            <asp:HyperLink ID="lnkLoc" NavigateUrl='<%# Eval("job","AddRecContract.aspx?uid={0}") %>'
                                                                                runat="server" Target="_blank" Text='<%#Eval("tag")%>'></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="130px" />
                                                                        <ItemStyle Width="130px" Wrap="True" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Address" SortExpression="address" HeaderStyle-Width="180px"
                                                                        ItemStyle-Width="180px" ItemStyle-Wrap="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("address")+", "+ Eval("City")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="180px" />
                                                                        <ItemStyle Width="180px" Wrap="True" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Origin" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkGridOrigin" ToolTip="Origin" runat="server" CssClass="origin"
                                                                                Enabled="false" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dest." Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkGridDest" ToolTip="Destination" runat="server" CssClass="dest"
                                                                                Enabled="false" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Worker">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblworkername" runat="server" Text='<%# Eval("worker") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Monthly Billing" SortExpression="MonthlyBill">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMonthlyBill" runat="server"><%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalMonthBill" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Period Billing" SortExpression="BAmt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotPerBill" runat="server"><%# DataBinder.Eval(Container.DataItem, "BAmt", "{0:c}")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalPeriodBill" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Monthly Hours" SortExpression="MonthlyHours">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMonthlyHours" runat="server"><%#DataBinder.Eval(Container.DataItem,"MonthlyHours", "{0:n}")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalMonthlyHours" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Period Hours" SortExpression="Hours">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotPeriodHours" runat="server"><%#Eval("Hours")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblGTotalPeriodHours" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Units">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblunits" runat="server"><%#Eval("elevcount")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblUnitTotal" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div class="roundCorner " style="margin-top: 5px">
                                                    <asp:Panel ID="pnlGridWorkerHandle" runat="server" Style="background: #ccc; width: 100%;
                                                        height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px;
                                                        border-width: 1px; border-color: #a9c6c9;">
                                                        <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                                                            ID="Label3" runat="server">Workers</asp:Label>
                                                        <asp:Image ID="imgExpandWork" runat="server" Style="float: right; margin-right: 10px;
                                                            height: 20px; cursor: pointer" />
                                                        <asp:Label ID="lblTotalRecWork" runat="server" Text="Label" Style="color: #fff; float: right;
                                                            margin-right: 20px"></asp:Label>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlWorkerGrid" runat="server">
                                                        <div style="max-height: 200px; overflow-y: scroll">
                                                            <asp:GridView ID="gvWorkers" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                CssClass="altrowstable" PageSize="20" ShowFooter="True" OnRowCommand="gvWorkers_RowCommand">
                                                                <RowStyle CssClass="evenrowcolor" />
                                                                <FooterStyle CssClass="footer" />
                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                <Columns>
                                                                <asp:TemplateField ItemStyle-Width="12px">
                                                                        <ItemTemplate >
                                                                        <img width="12px" src='<%# getWorkerColor(Eval("Name")) %>' />
                                                                        </ItemTemplate>
                                                                </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Worker">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblName" runat="server" CommandName="select" CommandArgument='<%# Bind("id") %>'
                                                                                Text='<%# Bind("Name") %>'></asp:LinkButton>
                                                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# Bind("id") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Contracts">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblContracts" runat="server" Text='<%#Eval("contracts")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblContractsTotal" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Units">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("units")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblUnitTotal" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Hours">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHours" runat="server" Text='<%#Eval("Hour")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblHoursTotal" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblAmountTotal" runat="server" Text=""></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div class="roundCorner " style="margin-top: 5px">
                                                    <asp:Panel ID="pnlGridTemplateHandle" runat="server" Style="background: #ccc; width: 100%;
                                                        height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px;
                                                        border-width: 1px; border-color: #a9c6c9;">
                                                        <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                                                            ID="Label4" runat="server">Template</asp:Label>
                                                        <asp:Image ID="imgExpandTemp" runat="server" Style="float: right; margin-right: 10px;
                                                            height: 20px; cursor: pointer" />                                                            
                                                        <asp:CheckBox ID="chkMap" runat="server"  Style="float: right; 
                                                            margin-right: 20px;" onclick="AssignedMarker();" />  
                                                        <asp:LinkButton ID="lnkDelTemplate" runat="server" OnClick="lnkDeleteRoute_Click"
                                                            OnClientClick="return DeleteTemplate();" Style="float: right; color: #2382B2;
                                                            margin-right: 20px;" ToolTip="Delete Template"><img height="20px" alt="Delete" 
                                            src="images/delete.png"/></asp:LinkButton>
                                                        <asp:ImageButton ID="lnkSaveTemplate" runat="server" OnClick="btnSaveRoute_Click" ImageUrl="images/saveiconblack.png" AlternateText="save" Height="20px"
                                                            Style="float: right; color: #2382B2; margin-right: 10px;" OnClientClick="return validateSave();"
                                                            ToolTip="Save Template"></asp:ImageButton>
                                                        <asp:LinkButton ID="lnkUpdateLocs" runat="server" Style="float: right; color: #2382B2;
                                                            margin-right: 10px; margin-left: 10px;" ToolTip="Update Locations" OnClientClick="return confirm('Are you sure you want to update the locations with the template?');"
                                                            OnClick="lnkUpdateLocs_Click"><img height="18px" 
                                            alt="Save" src="images/update.png"/></asp:LinkButton>
                                                        <asp:TextBox ID="txtTemplate" runat="server" placeholder="Template Name" Style="float: right"
                                                            ToolTip="Template Name" Width="120px"></asp:TextBox>
                                                            <div id="divTemplateOuter" onclick="$('#<%= hdnPreviousTempl.ClientID%>').val($('#<%= ddlTemplates.ClientID%>').val());">
                                                        <asp:DropDownList ID="ddlTemplates" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlTemplates_SelectedIndexChanged"
                                                            Style="float: right" TabIndex="14" ToolTip="Select Template" Width="100px" onchange="DisplayConfirmation()" >
                                                        </asp:DropDownList>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlTemplateGrid" runat="server">
                                                        <asp:TextBox ID="txtRemarks" CssClass="register_input_bg_generic" TextMode="MultiLine"
                                                            Height="50px" Width="702px" placeholder="Remarks" ToolTip="Remarks" runat="server"></asp:TextBox>
                                                        <asp:TabContainer runat="server" ID="cpChanges">
                                                            <asp:TabPanel runat="server" ID="tpLoc">
                                                                <HeaderTemplate>
                                                                    Location Changes
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div style="max-height: 200px; overflow-y: scroll; min-height: 93px">
                                                                        <asp:GridView ID="gvLocChanges" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            CssClass="altrowstable" PageSize="20" ShowFooter="True">
                                                                            <RowStyle CssClass="evenrowcolor" />
                                                                            <FooterStyle CssClass="footer" />
                                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Location">
                                                                                    <ItemTemplate>
                                                                                        <%-- <asp:Label ID="Label6" runat="server" Text='<%# Bind("MonthlyBill") %>' />
                                                                    <asp:Label ID="Label" runat="server" Text='<%# Bind("MonthlyHours") %>' />--%>
                                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("tag") %>'></asp:Label>
                                                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Bind("loc") %>' />
                                                                                        <asp:HiddenField ID="hdnHours" runat="server" Value='<%# Bind("MonthlyHours") %>' />
                                                                                        <asp:HiddenField ID="hdnAmt" runat="server" Value='<%# Bind("MonthlyBill") %>' />                                                                                       
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Current Worker">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCurWorker" runat="server" Text='<%# Bind("worker") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="New Worker">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNewWorker" runat="server" Text='<%# lstWorker.SelectedItem.Text %>'></asp:Label>
                                                                                        <asp:HiddenField ID="hdnWorker" runat="server" Value='<%# lstWorker.SelectedValue %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:TabPanel>
                                                            <asp:TabPanel runat="server" ID="tpworker">
                                                                <HeaderTemplate>
                                                                    Worker Changes
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div style="max-height: 200px; overflow-y: scroll; min-height: 93px">
                                                                        <asp:GridView ID="gvWorkerChanges" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            CssClass="altrowstable" PageSize="20" ShowFooter="True">
                                                                            <RowStyle CssClass="evenrowcolor" />
                                                                            <FooterStyle CssClass="footer" />
                                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                            <Columns>
                                                                             <asp:TemplateField ItemStyle-Width="12px">
                                                                        <ItemTemplate >
                                                                        <img width="12px" src='<%# getWorkerColor(Eval("Name")) %>' />
                                                                        </ItemTemplate>
                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Worker">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCurWorker" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                 <asp:TemplateField HeaderText="Current Contracts - Units ">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblContr" runat="server" Text='<%# Eval("contr") %>'></asp:Label>
                                                                                        -
                                                                                        <asp:Label ID="lblUnits" runat="server" Text='<%# Eval("units") %>'></asp:Label>
                                                                                      
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                 <asp:TemplateField HeaderText="New Contracts - Units ">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNewContr" runat="server" Text=""></asp:Label>
                                                                                        -
                                                                                        <asp:Label ID="lblNewUnits" runat="server" Text=""></asp:Label>
                                                                                      
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Current Hours - Amount (Monthly)">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "monthlyHours", "{0:n}") %>'></asp:Label>
                                                                                        -
                                                                                        <asp:Label ID="lblAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}") %>'></asp:Label>
                                                                                        <asp:HiddenField ID="hdnAmt" runat="server" Value='<%# Eval("MonthlyBill") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="New Hours - Amount (Monthly)">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNewHours" runat="server" Text=""></asp:Label>
                                                                                        -
                                                                                        <asp:Label ID="lblNewamt" runat="server" Text=""></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:TabPanel>
                                                        </asp:TabContainer>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%--  <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <div class="roundCorner shadow" style="margin: 5px 0 0 5px; padding: 5px 5px 5px 5px">
                        <asp:Panel runat="server" ID="pnlOptimize" Style="border-style: solid solid none solid;
                            background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px;
                            color: #23AEE8; font-weight: bold; font-size: 12px; border-width: 1px; padding-top: 5px;
                            border-color: #a9c6c9;">
                            <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                                ID="lblOptimize" runat="server">Optimized Route</asp:Label>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hdnTemplateData" runat="server" />
                                    <asp:LinkButton ID="lnkDeleteRoute" runat="server" OnClientClick="return DeleteTemplate();"
                                        Style="float: right; color: #2382B2; margin-right: 20px;" ToolTip="Delete Route Template"
                                        OnClick="lnkDeleteRoute_Click"><img height="20px" alt="Delete" src="images/delete.png"/></asp:LinkButton>
                                    <asp:LinkButton ID="btnSaveRoute" runat="server" Style="float: right; color: #2382B2;
                                        margin-right: 10px; margin-left: 10px;" OnClick="btnSaveRoute_Click" ToolTip="Save Route Template"><img height="20px" alt="Save" src="images/saveicon.png"/></asp:LinkButton>
                                    <asp:TextBox ID="txtTemplateName" Style="float: right" placeholder="Template Name"
                                        ToolTip="Template Name" Width="120px" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlTemplate" runat="server" ToolTip="Select Route Template"
                                        TabIndex="14" Width="100px" Style="float: right" onchange="TemplateOnChange(this)">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        
                        <div id="Optresult" style="height: 199px; overflow-y: scroll">
                            <asp:GridView ID="gvOptimized" runat="server" AutoGenerateColumns="False" CssClass="altrowstable"
                                DataKeyNames="loc" PageSize="20" ShowFooter="True">
                                <RowStyle CssClass="evenrowcolor" />
                                <FooterStyle CssClass="footer" />
                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndex" runat="server" Text='<%# alpha[Container.DataItemIndex] %>'></asp:Label>
                                            <asp:HiddenField ID="hdnCoordinate0" runat="server" Value='<%# Bind("coordinates") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId0" runat="server" Text='<%# Eval("loc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" SortExpression="name" HeaderStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName0" runat="server" Text='<%#Eval("tag")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address" SortExpression="address" HeaderStyle-Width="180px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress0" runat="server" Text='<%#Eval("address")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="City" SortExpression="City">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCity0" runat="server" Text='<%#Eval("City")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>--%>
                            </div>
                        </td>
                        <td valign="middle" style="background-color: #ccc; cursor: pointer; width: 20px;"
                            class="roundCorner" runat="server" id="pnlCollapse" title="Expand/Collapse" onclick="setTimeout(function(){google.maps.event.trigger(map, 'resize');},1000);" >
                            <asp:Image ID="imgCollapse" runat="server" Width="20px"  />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="width:100%; height:100%">
                <div id="map-canvas" style=" margin-left: 5px; width:100%; height:100%; min-width:1024px; min-height:800px " class="roundCorner shadow">
                </div>
            </td>
            <td valign="top" style="height:100%" >
                <div id="directions-panel" style="max-width: 300px; height: 587px; overflow: auto;">
                </div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnAssign" runat="server" Text="Button" OnClick="btnAssign_Click"
                Style="display: none" OnClientClick="ClickAssignCheck();" />
            <asp:HiddenField ID="hdnCenter" runat="server" />
            <asp:HiddenField ID="hdnRadius" runat="server" />
            <asp:HiddenField ID="hdnEdited" runat="server" Value="0" />
            <asp:HiddenField ID="hdnPreviousTempl" runat="server"  Value="0"  />
            <asp:HiddenField ID="hdnAssignedWorker" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel2" BackgroundCssClass="pnlUpdateoverlay"
        RepositionMode="RepositionOnWindowResizeAndScroll" PopupDragHandleControlID="Panel3">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="Panel2" Style="display: none; background: #fff; border: solid;"
        CssClass="roundCorner shadow">
        <asp:Panel runat="server" ID="Panel3" Style="background: #ccc; width: 100%; height: 20px;
            color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; cursor: move">
            <asp:Label Style="float: left; color: #000; margin-right: 20px; margin-left: 10px;"
                ID="Label5" runat="server">Workers</asp:Label>
        </asp:Panel>
        <div>
            <%--  <a style="top: -18px; position: absolute; z-index:1000; float:right" onclick="$find('PMPBehaviour').hide();">
                <img height="20px" src="images/close.png" /></a>--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:ListBox ID="lstWorker" runat="server" Style="max-height: 200px; min-height: 200px;
                        width: 100%"></asp:ListBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <input id="Button2" type="button" value="Assign" style="width: 80px" onclick="assignClick();" />
        <input id="Button1" type="button" value="Cancel" style="width: 80px" onclick="$find('PMPBehaviour').hide();" />
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="">
        <ProgressTemplate>
            <asp:Panel CssClass="pnlUpdateoverlay" Style="z-index: 900000" ID="Pnael1" runat="server"
                HorizontalAlign="Center">
                <asp:Image ID="Image1" runat="server" ImageUrl="images/loader_round.gif" 
                                                    style="position: absolute;
                                                    left: 50%;
                                                    top: 50%;
                                                    margin-left: -32px; /* -1 * image width / 2 */
                                                    margin-top: -32px;  /* -1 * image height / 2 */
                                                    display: block;" />
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>    
</asp:Content>
