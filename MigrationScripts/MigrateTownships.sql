
insert into Test.dbo.Townships(twp,rge,mer,geom) 
select substring(name, 0, 4) as township, substring(name,5,2) as [range], SUBSTRING(name, 9,1) as meridian ,
geography::STGeomFromText( 'POLYGON((' + cast(nelongitude * -1 as nvarchar) + ' ' + cast(nelatitude as nvarchar) + ', '  +
cast(nwlongitude * -1 as nvarchar) + ' ' + cast(nwlatitude as nvarchar) + ', ' +
cast(swlongitude * -1 as nvarchar) + ' ' + cast(swlatitude as nvarchar) + ', ' +
cast(selongitude * -1 as nvarchar) + ' ' + cast(selatitude as nvarchar) + ', ' +
cast(nelongitude * -1 as nvarchar) + ' ' + cast(nelatitude as nvarchar) + '))', 4326)
from tTownship