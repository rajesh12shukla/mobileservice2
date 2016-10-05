<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChartControl.ascx.cs" Inherits="ChartControl" %>

<div>
     <asp:Chart ID="Chart111" runat="server">
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
         <Series>
	</Series>
	<ChartAreas>
		<asp:ChartArea Name="ChartArea1">
		</asp:ChartArea>

	</ChartAreas>
</asp:Chart>
<asp:Chart ID="cTestChart" runat="server">
	<Series>
		<asp:Series Name="Testing" YValueType="Int32">

			<Points>
				<asp:DataPoint AxisLabel="Test 1" YValues="10" />
				<asp:DataPoint AxisLabel="Test 2" YValues="20" />

				<asp:DataPoint AxisLabel="Test 3" YValues="30" />
				<asp:DataPoint AxisLabel="Test 4" YValues="40" />

			</Points>
		</asp:Series>
	</Series>
	<ChartAreas>
		<asp:ChartArea Name="ChartArea1">
		</asp:ChartArea>

	</ChartAreas>
</asp:Chart>

</div>


