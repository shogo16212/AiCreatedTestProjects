```sql
/*=========================================================
 Database : PassportArchive
 DBMS     : Microsoft SQL Server
=========================================================*/

IF DB_ID('PassportArchive') IS NULL
BEGIN
    CREATE DATABASE PassportArchive;
END
GO

USE PassportArchive;
GO

/*=========================================================
 Users
=========================================================*/
CREATE TABLE Users
(
    UserId          INT IDENTITY(1,1) PRIMARY KEY,
    UserName        NVARCHAR(50) NOT NULL,
    Email           NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(255) NOT NULL,
    AvatarImage     NVARCHAR(255) NULL,

    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(50) NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0
);
GO

/*=========================================================
 Countries
=========================================================*/
CREATE TABLE Countries
(
    CountryId       INT IDENTITY(1,1) PRIMARY KEY,
    CountryCode     CHAR(2) NOT NULL UNIQUE,
    CountryName     NVARCHAR(100) NOT NULL,

    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(50) NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0
);
GO

/*=========================================================
 Regions
=========================================================*/
CREATE TABLE Regions
(
    RegionId        INT IDENTITY(1,1) PRIMARY KEY,
    CountryId       INT NOT NULL,
    RegionName      NVARCHAR(100) NOT NULL,

    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(50) NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Regions_Countries
        FOREIGN KEY(CountryId)
        REFERENCES Countries(CountryId)
);
GO

/*=========================================================
 Cities
=========================================================*/
CREATE TABLE Cities
(
    CityId          INT IDENTITY(1,1) PRIMARY KEY,
    RegionId        INT NOT NULL,
    CityName        NVARCHAR(100) NOT NULL,

    Latitude        DECIMAL(10,7) NULL,
    Longitude       DECIMAL(10,7) NULL,

    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(50) NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Cities_Regions
        FOREIGN KEY(RegionId)
        REFERENCES Regions(RegionId)
);
GO

/*=========================================================
 Spots
=========================================================*/
CREATE TABLE Spots
(
    SpotId          INT IDENTITY(1,1) PRIMARY KEY,
    CityId          INT NOT NULL,

    SpotName        NVARCHAR(150) NOT NULL,
    Description     NVARCHAR(500) NULL,

    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(50) NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Spots_Cities
        FOREIGN KEY(CityId)
        REFERENCES Cities(CityId)
);
GO

/*=========================================================
 Badges
=========================================================*/
CREATE TABLE Badges
(
    BadgeId         INT IDENTITY(1,1) PRIMARY KEY,

    BadgeName       NVARCHAR(100) NOT NULL,
    Description     NVARCHAR(300) NULL,
    ConditionText   NVARCHAR(300) NOT NULL,

    IsEnabled       BIT NOT NULL DEFAULT 1,

    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy       NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt       DATETIME2 NULL,
    UpdatedBy       NVARCHAR(50) NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0
);
GO

/*=========================================================
 Index
=========================================================*/

CREATE INDEX IX_Users_Email
ON Users(Email);

CREATE INDEX IX_Regions_CountryId
ON Regions(CountryId);

CREATE INDEX IX_Cities_RegionId
ON Cities(RegionId);

CREATE INDEX IX_Spots_CityId
ON Spots(CityId);

CREATE INDEX IX_Badges_Name
ON Badges(BadgeName);
GO
```

```sql
/*=========================================================
 TravelRecords
=========================================================*/
CREATE TABLE TravelRecords
(
    TravelId            INT IDENTITY(1,1) PRIMARY KEY,
    UserId              INT NOT NULL,
    CountryId           INT NOT NULL,
    RegionId            INT NOT NULL,
    CityId              INT NOT NULL,

    Title               NVARCHAR(150) NOT NULL,
    StartDate           DATE NOT NULL,
    EndDate             DATE NOT NULL,
    Memo                NVARCHAR(MAX) NULL,

    FavoriteFlag        BIT NOT NULL DEFAULT 0,

    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy           NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,
    UpdatedAt           DATETIME2 NULL,
    UpdatedBy           NVARCHAR(50) NULL,
    IsDeleted           BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_TravelRecords_Users
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_TravelRecords_Countries
        FOREIGN KEY(CountryId)
        REFERENCES Countries(CountryId),

    CONSTRAINT FK_TravelRecords_Regions
        FOREIGN KEY(RegionId)
        REFERENCES Regions(RegionId),

    CONSTRAINT FK_TravelRecords_Cities
        FOREIGN KEY(CityId)
        REFERENCES Cities(CityId),

    CONSTRAINT CHK_Travel_Date
        CHECK(StartDate <= EndDate)
);
GO

/*=========================================================
 TravelPhotos
=========================================================*/
CREATE TABLE TravelPhotos
(
    PhotoId             INT IDENTITY(1,1) PRIMARY KEY,
    TravelId            INT NOT NULL,

    FileName            NVARCHAR(255) NOT NULL,
    FilePath            NVARCHAR(500) NOT NULL,
    FileSize            BIGINT NULL,

    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreatedBy           NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER,

    CONSTRAINT FK_TravelPhotos_TravelRecords
        FOREIGN KEY(TravelId)
        REFERENCES TravelRecords(TravelId)
        ON DELETE CASCADE
);
GO

/*=========================================================
 TravelSpots
=========================================================*/
CREATE TABLE TravelSpots
(
    TravelSpotId        INT IDENTITY(1,1) PRIMARY KEY,
    TravelId            INT NOT NULL,
    SpotId              INT NOT NULL,

    VisitDate           DATE NULL,

    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_TravelSpots_Travel
        FOREIGN KEY(TravelId)
        REFERENCES TravelRecords(TravelId)
        ON DELETE CASCADE,

    CONSTRAINT FK_TravelSpots_Spots
        FOREIGN KEY(SpotId)
        REFERENCES Spots(SpotId),

    CONSTRAINT UQ_TravelSpot UNIQUE
    (
        TravelId,
        SpotId
    )
);
GO

/*=========================================================
 PassportStamps
=========================================================*/
CREATE TABLE PassportStamps
(
    StampId             INT IDENTITY(1,1) PRIMARY KEY,

    UserId              INT NOT NULL,
    TravelId            INT NOT NULL,
    CityId              INT NOT NULL,

    StampDate           DATE NOT NULL,

    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_PassportStamp_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_PassportStamp_Travel
        FOREIGN KEY(TravelId)
        REFERENCES TravelRecords(TravelId),

    CONSTRAINT FK_PassportStamp_City
        FOREIGN KEY(CityId)
        REFERENCES Cities(CityId)
);
GO

/*=========================================================
 UserBadges
=========================================================*/
CREATE TABLE UserBadges
(
    UserBadgeId         INT IDENTITY(1,1) PRIMARY KEY,

    UserId              INT NOT NULL,
    BadgeId             INT NOT NULL,

    EarnedDate          DATE NOT NULL,

    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_UserBadges_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_UserBadges_Badge
        FOREIGN KEY(BadgeId)
        REFERENCES Badges(BadgeId),

    CONSTRAINT UQ_UserBadge UNIQUE
    (
        UserId,
        BadgeId
    )
);
GO

/*=========================================================
 TravelCertificates
=========================================================*/
CREATE TABLE TravelCertificates
(
    CertificateId       INT IDENTITY(1,1) PRIMARY KEY,

    TravelId            INT NOT NULL,

    CertificateNo       NVARCHAR(30) NOT NULL UNIQUE,

    PdfPath             NVARCHAR(300) NULL,

    IssuedDate          DATE NOT NULL,

    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_TravelCertificate_Travel
        FOREIGN KEY(TravelId)
        REFERENCES TravelRecords(TravelId)
        ON DELETE CASCADE
);
GO

/*=========================================================
 INDEX
=========================================================*/

CREATE INDEX IX_Travel_User
ON TravelRecords(UserId);

CREATE INDEX IX_Travel_Country
ON TravelRecords(CountryId);

CREATE INDEX IX_Travel_City
ON TravelRecords(CityId);

CREATE INDEX IX_Travel_Date
ON TravelRecords(StartDate, EndDate);

CREATE INDEX IX_Photo_Travel
ON TravelPhotos(TravelId);

CREATE INDEX IX_TravelSpot_Travel
ON TravelSpots(TravelId);

CREATE INDEX IX_Stamp_User
ON PassportStamps(UserId);

CREATE INDEX IX_UserBadge_User
ON UserBadges(UserId);

CREATE INDEX IX_Certificate_Travel
ON TravelCertificates(TravelId);
GO
```


```sql
/*=========================================================
 TravelStatistics
=========================================================*/
CREATE TABLE TravelStatistics
(
    StatisticId         INT IDENTITY(1,1) PRIMARY KEY,
    UserId              INT NOT NULL,
    TotalTrips          INT NOT NULL DEFAULT 0,
    TotalCountries      INT NOT NULL DEFAULT 0,
    TotalCities         INT NOT NULL DEFAULT 0,
    TotalDistanceKm     DECIMAL(10,2) NOT NULL DEFAULT 0,

    UpdatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_TravelStatistics_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);
GO

/*=========================================================
 NotificationLogs
=========================================================*/
CREATE TABLE NotificationLogs
(
    NotificationId      INT IDENTITY(1,1) PRIMARY KEY,
    UserId              INT NOT NULL,

    Title               NVARCHAR(100) NOT NULL,
    Message             NVARCHAR(500) NOT NULL,

    IsRead              BIT NOT NULL DEFAULT 0,
    CreatedAt           DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Notification_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);
GO

/*=========================================================
 LoginHistories
=========================================================*/
CREATE TABLE LoginHistories
(
    LoginHistoryId      INT IDENTITY(1,1) PRIMARY KEY,
    UserId              INT NOT NULL,

    LoginDate           DATETIME2 NOT NULL,
    IpAddress           NVARCHAR(45),
    UserAgent           NVARCHAR(500),

    CONSTRAINT FK_LoginHistory_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);
GO

/*=========================================================
 SystemSettings
=========================================================*/
CREATE TABLE SystemSettings
(
    SettingId           INT IDENTITY(1,1) PRIMARY KEY,
    SettingKey          NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue        NVARCHAR(500) NOT NULL,
    Description         NVARCHAR(500) NULL
);
GO

/*=========================================================
 VIEW
=========================================================*/
CREATE VIEW vw_UserTravelSummary
AS
SELECT
    U.UserId,
    U.UserName,
    COUNT(T.TravelId) AS TravelCount
FROM Users U
LEFT JOIN TravelRecords T
    ON U.UserId = T.UserId
   AND T.IsDeleted = 0
GROUP BY
    U.UserId,
    U.UserName;
GO

/*=========================================================
 Stored Procedure
=========================================================*/
CREATE PROCEDURE usp_GetTravelHistory
(
    @UserId INT
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        TravelId,
        Title,
        StartDate,
        EndDate
    FROM TravelRecords
    WHERE UserId=@UserId
      AND IsDeleted=0
    ORDER BY StartDate DESC;

END;
GO

/*=========================================================
 Initial Data
=========================================================*/

INSERT INTO Countries
(CountryCode,CountryName)
VALUES
('JP','Japan'),
('KR','South Korea'),
('TW','Taiwan'),
('FR','France');

INSERT INTO Regions
(CountryId,RegionName)
VALUES
(1,N'関西'),
(1,N'関東'),
(2,N'ソウル特別市'),
(3,N'台北市');

INSERT INTO Cities
(RegionId,CityName)
VALUES
(1,N'京都'),
(1,N'大阪'),
(2,N'東京'),
(3,N'ソウル'),
(4,N'台北');

INSERT INTO Spots
(CityId,SpotName)
VALUES
(1,N'清水寺'),
(1,N'金閣寺'),
(2,N'大阪城'),
(4,N'景福宮'),
(5,N'台北101');

INSERT INTO Badges
(
BadgeName,
Description,
ConditionText
)
VALUES
(
N'Weekend Traveler',
N'週末旅行10回',
N'旅行10回'
),
(
N'Asia Explorer',
N'アジア10都市',
N'アジア10都市訪問'
),
(
N'World Collector',
N'30か国達成',
N'30か国訪問'
);

INSERT INTO Users
(
UserName,
Email,
PasswordHash
)
VALUES
(
N'admin',
'admin@passportarchive.com',
'hashed_password'
);

INSERT INTO TravelStatistics
(
UserId,
TotalTrips,
TotalCountries,
TotalCities,
TotalDistanceKm
)
VALUES
(
1,
0,
0,
0,
0
);

INSERT INTO SystemSettings
(
SettingKey,
SettingValue,
Description
)
VALUES
(
'MaxUploadSize',
'10485760',
'10MB'
),
(
'ImageExtension',
'jpg,png',
'Upload Image Types'
);
GO

/*=========================================================
 INDEX
=========================================================*/

CREATE INDEX IX_TravelStatistics_User
ON TravelStatistics(UserId);

CREATE INDEX IX_LoginHistories_User
ON LoginHistories(UserId);

CREATE INDEX IX_Notification_User
ON NotificationLogs(UserId);

CREATE INDEX IX_SystemSettings_Key
ON SystemSettings(SettingKey);
GO
```
