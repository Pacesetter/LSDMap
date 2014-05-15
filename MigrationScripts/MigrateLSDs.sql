
insert into Test.dbo.LSDs(lsd,psect,ptwp,prge,pmer,geom) 
select
substring(ltrim(name), 0,3) as lsd,
replicate('0', 2-LEN(REPLACE(substring(ltrim(name), charindex('-', ltrim(name))+1,2), '-', ''))) +  REPLACE(substring(ltrim(name), charindex('-', ltrim(name))+1,2), '-', '') as section,
substring(substring(ltrim(name), CHARINDEX('-', Ltrim(name))+2, 10), CHARINDEX('-', substring(ltrim(name), CHARINDEX('-', Ltrim(name))+1, 10)), 3) as township, 
reverse(substring(reverse(ltrim(name)), charindex(' ', reverse(ltrim(name))) + 1, 2)) as [range], 
right(name,1) as meridian, 
geography::STGeomFromText( 'POLYGON((' + cast(nelongitude * -1 as nvarchar) + ' ' + cast(nelatitude as nvarchar) + ', '  +
cast(nwlongitude * -1 as nvarchar) + ' ' + cast(nwlatitude as nvarchar) + ', ' +
cast(swlongitude * -1 as nvarchar) + ' ' + cast(swlatitude as nvarchar) + ', ' +
cast(selongitude * -1 as nvarchar) + ' ' + cast(selatitude as nvarchar) + ', ' +
cast(nelongitude * -1 as nvarchar) + ' ' + cast(nelatitude as nvarchar) + '))', 4326)
from lsd