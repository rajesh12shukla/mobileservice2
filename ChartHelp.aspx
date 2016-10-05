<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartHelp.aspx.cs" Inherits="ChartHelp" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="~/ChartControl.ascx" TagPrefix="uc1" TagName="ChartControl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
        <div>
<%--                        <asp:Chart ID="Chart111" runat="server">
	<Series>
		<asp:Series Name="Testing" YValueType="Int32" ChartType="Pie">

			<Points>
				<asp:DataPoint AxisLabel="10.00%" YValues="10" />
				<asp:DataPoint AxisLabel="20.00%" YValues="20" />

				<asp:DataPoint AxisLabel="20.00%" YValues="30" />
				<asp:DataPoint AxisLabel="50.00%" YValues="40" />

			</Points>
		</asp:Series>
	</Series>
	<ChartAreas>
		<asp:ChartArea Name="ChartArea1">
		</asp:ChartArea>

	</ChartAreas>
</asp:Chart>--%>

            <uc1:ChartControl runat="server" ID="ChartControl" />

        </div>
    </form>
</body>
</html>
