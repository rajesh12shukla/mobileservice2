create PROCEDURE [dbo].[spAddMapData]
@DtMapData As [dbo].[tblTypeMapData] Readonly

as

insert into MapData
(
deviceId,
latitude,
longitude,
date
)
Select 
deviceId, latitude, longitude, date
From @DtMapData d
