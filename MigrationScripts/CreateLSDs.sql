create table LSD(
  id int identity(1,1),
  name nvarchar(30),
  NWLatitude numeric(14,10),
  NWLongitude numeric(14,10),
  NELatitude numeric(14,10),
  NELongitude numeric(14,10),
  SWLatitude numeric(14,10),
  SWLongitude numeric(14,10),
  SELatitude numeric(14,10),
  SELongitude numeric(14,10)

)

insert into LSD(name,  NWLatitude, NWLongitude, NELatitude,  NELongitude, SWLatitude,  SWLongitude, SELatitude, SELongitude)
(
SELECT REPLACE ([Name], 'NW', '13'),[NWLatitude] 'NWLatitude',[NWLongitude] 'NWLongitude',[NCLatitude] 'NELatitude',[NCLongitude] 'NELongitude',[CWLatitude] 'SWLatitude',[CWLongitude] 'SWLongitude',[CC1Latitude] 'SELatitude',[CC1Longitude] 'SELongitude'
  FROM tQuarter
 WHERE [Quarter] = 'NW'
 UNION
SELECT REPLACE ([Name], 'NW', '14'),[NCLatitude],[NCLongitude],[NELatitude],[NELongitude],[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'NW'
 UNION
SELECT REPLACE ([Name], 'NW', '12'),[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude],[SWLatitude],[SWLongitude],[SCLatitude],[SCLongitude]
  FROM tQuarter
 WHERE [Quarter] = 'NW'
 UNION
SELECT REPLACE ([Name], 'NW', '11'),[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude],[SCLatitude],[SCLongitude],[SELatitude],[SELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'NW'
 UNION

SELECT REPLACE ([Name], 'NE', '15'),[NWLatitude],[NWLongitude],[NCLatitude],[NCLongitude],[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude]
  FROM tQuarter
 WHERE [Quarter] = 'NE'
 UNION
SELECT REPLACE ([Name], 'NE', '16'),[NCLatitude],[NCLongitude],[NELatitude],[NELongitude],[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'NE'
 UNION
SELECT REPLACE ([Name], 'NE', '10'),[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude],[SWLatitude],[SWLongitude],[SCLatitude],[SCLongitude]
  FROM tQuarter
 WHERE [Quarter] = 'NE'
 UNION
SELECT REPLACE ([Name], 'NE', '09'),[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude],[SCLatitude],[SCLongitude],[SELatitude],[SELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'NE'
 UNION

SELECT REPLACE ([Name], 'SW', '05'),[NWLatitude],[NWLongitude],[NCLatitude],[NCLongitude],[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude]
  FROM tQuarter
 WHERE [Quarter] = 'SW'
 UNION
SELECT REPLACE ([Name], 'SW', '06'),[NCLatitude],[NCLongitude],[NELatitude],[NELongitude],[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'SW'
 UNION
SELECT REPLACE ([Name], 'SW', '04'),[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude],[SWLatitude],[SWLongitude],[SCLatitude],[SCLongitude]
  FROM tQuarter
 WHERE [Quarter] = 'SW'
 UNION
SELECT REPLACE ([Name], 'SW', '03'),[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude],[SCLatitude],[SCLongitude],[SELatitude],[SELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'SW'
 UNION

SELECT REPLACE ([Name], 'SE', '07'),[NWLatitude],[NWLongitude],[NCLatitude],[NCLongitude],[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude]
  FROM tQuarter
 WHERE [Quarter] = 'SE'
 UNION
SELECT REPLACE ([Name], 'SE', '08'),[NCLatitude],[NCLongitude],[NELatitude],[NELongitude],[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'SE'
 UNION
SELECT REPLACE ([Name], 'SE', '02'),[CWLatitude],[CWLongitude],[CC1Latitude],[CC1Longitude],[SWLatitude],[SWLongitude],[SCLatitude],[SCLongitude]
  FROM tQuarter
 WHERE [Quarter] = 'SE'
 UNION
SELECT REPLACE ([Name], 'SE', '01'),[CC1Latitude],[CC1Longitude],[CELatitude],[CELongitude],[SCLatitude],[SCLongitude],[SELatitude],[SELongitude]
  FROM tQuarter
 WHERE [Quarter] = 'SE')


