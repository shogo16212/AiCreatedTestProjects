------------------------------------------------------------
-- 1. データベース作成
------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SilentMomentsDB')
BEGIN
    CREATE DATABASE SilentMomentsDB;
END
GO

USE SilentMomentsDB;
GO

------------------------------------------------------------
-- 2. テーブル作成
------------------------------------------------------------

-------------------------
-- 場所テーブル
-------------------------
IF OBJECT_ID('dbo.Places', 'U') IS NOT NULL
    DROP TABLE dbo.Places;
GO

CREATE TABLE dbo.Places (
    PlaceId INT IDENTITY(1,1) PRIMARY KEY,
    PlaceName NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE INDEX IX_Places_PlaceName ON dbo.Places(PlaceName);
GO

-------------------------
-- タグテーブル
-------------------------
IF OBJECT_ID('dbo.Tags', 'U') IS NOT NULL
    DROP TABLE dbo.Tags;
GO

CREATE TABLE dbo.Tags (
    TagId INT IDENTITY(1,1) PRIMARY KEY,
    TagName NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE INDEX IX_Tags_TagName ON dbo.Tags(TagName);
GO

-------------------------
-- 静けさ記録テーブル
-------------------------
IF OBJECT_ID('dbo.QuietMoments', 'U') IS NOT NULL
    DROP TABLE dbo.QuietMoments;
GO

CREATE TABLE dbo.QuietMoments (
    MomentId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    QuietLevel INT NOT NULL CHECK (QuietLevel BETWEEN 1 AND 10),
    Memo NVARCHAR(500) NULL,
    PhotoUrl NVARCHAR(200) NULL,
    PlaceId INT NOT NULL,
    RecordedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (PlaceId) REFERENCES dbo.Places(PlaceId)
);
GO

CREATE INDEX IX_QuietMoments_PlaceId ON dbo.QuietMoments(PlaceId);
CREATE INDEX IX_QuietMoments_RecordedAt ON dbo.QuietMoments(RecordedAt);
GO

-------------------------
-- 静けさ記録 × タグ（多対多）
-------------------------
IF OBJECT_ID('dbo.QuietMomentTags', 'U') IS NOT NULL
    DROP TABLE dbo.QuietMomentTags;
GO

CREATE TABLE dbo.QuietMomentTags (
    MomentId INT NOT NULL,
    TagId INT NOT NULL,
    PRIMARY KEY (MomentId, TagId),
    FOREIGN KEY (MomentId) REFERENCES dbo.QuietMoments(MomentId),
    FOREIGN KEY (TagId) REFERENCES dbo.Tags(TagId)
);
GO

CREATE INDEX IX_QuietMomentTags_TagId ON dbo.QuietMomentTags(TagId);
GO

-------------------------
-- 静けさルートテーブル
-------------------------
IF OBJECT_ID('dbo.Routes', 'U') IS NOT NULL
    DROP TABLE dbo.Routes;
GO

CREATE TABLE dbo.Routes (
    RouteId INT IDENTITY(1,1) PRIMARY KEY,
    RouteName NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-------------------------
-- 静けさルート × 記録（順番付き）
-------------------------
IF OBJECT_ID('dbo.RouteMoments', 'U') IS NOT NULL
    DROP TABLE dbo.RouteMoments;
GO

CREATE TABLE dbo.RouteMoments (
    RouteId INT NOT NULL,
    MomentId INT NOT NULL,
    SequenceNo INT NOT NULL,
    PRIMARY KEY (RouteId, MomentId),
    FOREIGN KEY (RouteId) REFERENCES dbo.Routes(RouteId),
    FOREIGN KEY (MomentId) REFERENCES dbo.QuietMoments(MomentId)
);
GO

CREATE INDEX IX_RouteMoments_RouteId ON dbo.RouteMoments(RouteId);
GO

------------------------------------------------------------
-- 3. 初期データ投入
------------------------------------------------------------

-- 場所
INSERT INTO dbo.Places (PlaceName) VALUES
('自宅'),
('図書館'),
('公園'),
('カフェ');

-- タグ
INSERT INTO dbo.Tags (TagName) VALUES
('早朝'),
('静寂'),
('読書'),
('自然');

-- 静けさ記録
INSERT INTO dbo.QuietMoments (Title, QuietLevel, Memo, PhotoUrl, PlaceId)
VALUES
('早朝の公園', 8, '鳥の声だけで静かだった', '/photos/1.jpg', 3),
('図書館の読書スペース', 9, '空調音のみで集中できた', '/photos/2.jpg', 2),
('自宅の夜', 7, 'とても落ち着いた時間', '/photos/3.jpg', 1);

-- 静けさ記録 × タグ
INSERT INTO dbo.QuietMomentTags (MomentId, TagId) VALUES
(1, 1), -- 早朝
(1, 4), -- 自然
(2, 3), -- 読書
(2, 2), -- 静寂
(3, 2); -- 静寂

-- 静けさルート
INSERT INTO dbo.Routes (RouteName) VALUES
('休日静けさ散歩');

-- 静けさルート × 記録
INSERT INTO dbo.RouteMoments (RouteId, MomentId, SequenceNo) VALUES
(1, 1, 1),
(1, 2, 2),
(1, 3, 3);

------------------------------------------------------------
-- 4. VIEW（便利機能）
------------------------------------------------------------

-- 静けさ記録の詳細ビュー
CREATE VIEW dbo.vQuietMomentDetails AS
SELECT
    qm.MomentId,
    qm.Title,
    qm.QuietLevel,
    qm.Memo,
    qm.PhotoUrl,
    p.PlaceName,
    qm.RecordedAt
FROM dbo.QuietMoments qm
INNER JOIN dbo.Places p ON qm.PlaceId = p.PlaceId;
GO

------------------------------------------------------------
-- 5. ストアドプロシージャ（例）
------------------------------------------------------------

-- 静けさ記録の検索（タグ・場所・期間）
CREATE PROCEDURE dbo.SearchQuietMoments
    @TagId INT = NULL,
    @PlaceId INT = NULL,
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL
AS
BEGIN
    SELECT DISTINCT qm.*
    FROM dbo.QuietMoments qm
    LEFT JOIN dbo.QuietMomentTags qmt ON qm.MomentId = qmt.MomentId
    WHERE
        (@TagId IS NULL OR qmt.TagId = @TagId)
        AND (@PlaceId IS NULL OR qm.PlaceId = @PlaceId)
        AND (@FromDate IS NULL OR qm.RecordedAt >= @FromDate)
        AND (@ToDate IS NULL OR qm.RecordedAt <= @ToDate)
    ORDER BY qm.RecordedAt DESC;
END
GO
