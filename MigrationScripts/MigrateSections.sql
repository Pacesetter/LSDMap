insert into Test.dbo.sections(sect,ptwp,prge,pmer,geom) 
select
replicate('0', 3-CHARINDEX('-', ltrim(name))) +  substring(ltrim(name), 0, CHARINDEX('-', ltrim(name))) as section,
substring(ltrim(name), CHARINDEX('-', Ltrim(name))+1, 3) as township, 
reverse(substring(reverse(ltrim(name)), charindex(' ', reverse(ltrim(name))) + 1, 2)) as [range], 
right(name,1) as meridian, 
geography::STGeomFromText( 'POLYGON((' + cast(nelongitude * -1 as nvarchar) + ' ' + cast(nelatitude as nvarchar) + ', '  +
cast(nwlongitude * -1 as nvarchar) + ' ' + cast(nwlatitude as nvarchar) + ', ' +
cast(swlongitude * -1 as nvarchar) + ' ' + cast(swlatitude as nvarchar) + ', ' +
cast(selongitude * -1 as nvarchar) + ' ' + cast(selatitude as nvarchar) + ', ' +
cast(nelongitude * -1 as nvarchar) + ' ' + cast(nelatitude as nvarchar) + '))', 4326)
from tSection
