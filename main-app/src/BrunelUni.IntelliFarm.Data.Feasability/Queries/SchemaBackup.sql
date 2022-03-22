-- auto-generated definition
create table FeasabilityDtos
(
    Id                char(36) not null
        primary key,
    PolyCount         float    not null,
    Coverage          float    not null,
    Clusters          int      not null,
    MetaDataJson      longtext null,
    Samples           int      not null,
    MaxBounces        int      not null,
    RenderTimeSeconds double   not null,
    Session           char(36) not null,
    Device            longtext null,
    UtcCurrent        datetime(6) not null
);

-- auto-generated definition
create table Sessions
(
    Id          varchar(255) not null
        primary key,
    Description varchar(1000) null,
    DataGather  tinyint(1) null
);



