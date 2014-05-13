CREATE NONCLUSTERED INDEX sections_search
ON [dbo].[Sections] ([PTWP],[PRGE],[PMER])
INCLUDE ([Id],[SECT],[geom])

CREATE NONCLUSTERED INDEX lsds_search
ON [dbo].[LSDs] ([PSECT],[PTWP],[PRGE],[PMER])


--this index needs to be added second as adding it without the help of the lsds_search index times out
CREATE NONCLUSTERED INDEX lsds_search_include
ON [dbo].[LSDs] ([PSECT],[PTWP],[PRGE],[PMER])
INCLUDE ([Id],[PPID],[EFFDT],[FEATURECD],[LSD],[QLSD],[PQPPID],[LLD],[geom])   