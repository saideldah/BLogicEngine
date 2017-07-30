create table Cover (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null,
   CodeBack             nvarchar(200)        not null
)
go

alter table Cover
   add constraint PK_COVER primary key (Id)
go


create table CoverMapping (
   Id                   uniqueidentifier     not null,
   CoverId              uniqueidentifier     not null,
   CodeBack             nvarchar(200)        null
)
go

alter table CoverMapping
   add constraint PK_COVERMAPPING primary key (Id)
go


create table LineOfBusiness (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null,
   GroupId              uniqueidentifier     not null
)
go

alter table LineOfBusiness
   add constraint PK_LINEOFBUSINESS primary key (Id)
go


create table LineOfBusinessProperty (
   Id                   uniqueidentifier     not null,
   Value                nvarchar(500)        null,
   LineOfBusinessId     uniqueidentifier     not null,
   PropertyId           uniqueidentifier     not null
)
go

alter table LineOfBusinessProperty
   add constraint PK_LINEOFBUSINESSPROPERTY primary key (Id)
go

create table LobGroup (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null
)
go

alter table LobGroup
   add constraint PK_LOBGROUP primary key (Id)
go


create table Package (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null,
   LineOfBusinessId     uniqueidentifier     not null,
   SmiWizardType        nvarchar(200)        null,
   SavingsCalculator    nvarchar(200)        null,
   Status               nvarchar(200)        null
)
go

alter table Package
   add constraint PK_PACKAGE primary key (Id)
go


create table PackageProperty (
   Id                   uniqueidentifier     not null,
   Value                nvarchar(500)        null,
   PackageId            uniqueidentifier     not null,
   PropertyId           uniqueidentifier     not null
)
go

alter table PackageProperty
   add constraint PK_PACKAGEPROPERTY primary key (Id)
go


create table Product (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null
)
go

alter table Product
   add constraint PK_PRODUCT primary key (Id)
go


create table ProductCoverProperty (
   Id                   uniqueidentifier     not null,
   Value                nvarchar(500)        null,
   CoverId              uniqueidentifier     not null,
   PropertyId           uniqueidentifier     not null,
   ProductId            uniqueidentifier     null
)
go

alter table ProductCoverProperty
   add constraint PK_PRODUCTCOVERPROPERTY primary key (Id)
go

create table ProductMapping (
   Id                   uniqueidentifier     not null,
   MappingRule          nvarchar(MAX)        not null,
   CodeBack             nvarchar(200)        null,
   Status               nvarchar(200)        not null,
   ProductId            uniqueidentifier     not null
)
go

alter table ProductMapping
   add constraint PK_PRODUCTMAPPING primary key (Id)
go

create table ProductPackage (
   Id                   uniqueidentifier     not null,
   ProductId            uniqueidentifier     not null,
   PackageId            uniqueidentifier     not null
)
go

alter table ProductPackage
   add constraint PK_PRODUCTPACKAGE primary key (Id)
go


create table ProductSetupFile (
   Id                   uniqueidentifier     not null,
   LibraryName          nvarchar(MAX)        not null,
   FolderPath           int                  not null,
   OriginFileName       nvarchar(MAX)        not null,
   UploadedFileName     nvarchar(MAX)        not null,
   FilePath             nvarchar(MAX)        not null,
   FileType             nvarchar(MAX)        not null,
   CreatedDate          datetime             not null,
   [Version]            int                  not null default 1
)
go

alter table ProductSetupFile
   add constraint PK_PRODUCTSETUPFILE primary key (Id)
go

create table Property (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null,
   DataType             nvarchar(200)        null,
   PropertyType         nvarchar(200)        null
)
go

alter table Property
   add constraint PK_PROPERTY primary key (Id)
go

create table SMI (
   Id                   uniqueidentifier     not null,
   Code                 nvarchar(200)        not null,
   Title                nvarchar(200)        not null,
   RetrievedFromBank    bit                  not null,
   FieldControlOnScreen nvarchar(MAX)        null,
   Description          nvarchar(MAX)        null
)
go

alter table SMI
   add constraint PK_SMI primary key (Id)
go


create table SmiProperty (
   Id                   uniqueidentifier     not null,
   Value                nvarchar(500)        null,
   SmiId                uniqueidentifier     not null,
   PropertyId           uniqueidentifier     not null
)
go

alter table SmiProperty
   add constraint PK_SMIPROPERTY primary key (Id)
go

alter table CoverMapping
   add constraint FK_COVERMAP_REFERENCE_COVER foreign key (CoverId)
      references Cover (Id)
go

alter table LineOfBusiness
   add constraint FK_LINEOFBU_REFERENCE_LOBGROUP foreign key (GroupId)
      references LobGroup (Id)
go

alter table LineOfBusinessProperty
   add constraint FK_LINEOFBU_REFERENCE_LINEOFBU foreign key (LineOfBusinessId)
      references LineOfBusiness (Id)
go

alter table LineOfBusinessProperty
   add constraint FK_LINEOFBU_REFERENCE_PROPERTY foreign key (PropertyId)
      references Property (Id)
go

alter table Package
   add constraint FK_PACKAGE_REFERENCE_LINEOFBU foreign key (LineOfBusinessId)
      references LineOfBusiness (Id)
go

alter table PackageProperty
   add constraint FK_PACKAGEP_REFERENCE_PROPERTY foreign key (PropertyId)
      references Property (Id)
go

alter table PackageProperty
   add constraint FK_PACKAGEP_REFERENCE_PACKAGE foreign key (PackageId)
      references Package (Id)
go

alter table ProductCoverProperty
   add constraint FK_PRODUCTC_REFERENCE_PROPERTY foreign key (PropertyId)
      references Property (Id)
go

alter table ProductCoverProperty
   add constraint FK_PRODUCTC_REFERENCE_COVER foreign key (CoverId)
      references Cover (Id)
go

alter table ProductCoverProperty
   add constraint FK_PRODUCTC_REFERENCE_PRODUCT foreign key (ProductId)
      references Product (Id)
go

alter table ProductMapping
   add constraint FK_PRODUCTM_REFERENCE_PRODUCT foreign key (ProductId)
      references Product (Id)
go

alter table ProductPackage
   add constraint FK_PRODUCTP_REFERENCE_PRODUCT foreign key (ProductId)
      references Product (Id)
go

alter table ProductPackage
   add constraint FK_PRODUCTP_REFERENCE_PACKAGE foreign key (PackageId)
      references Package (Id)
go

alter table SmiProperty
   add constraint FK_SMIPROPE_REFERENCE_SMI foreign key (SmiId)
      references SMI (Id)
go

alter table SmiProperty
   add constraint FK_SMIPROPE_REFERENCE_PROPERTY foreign key (PropertyId)
      references Property (Id)
go

