DECLARE @defaultSectionLayoutId INT;
SELECT TOP 1 @defaultSectionLayoutId = sl.SectionLayoutId
FROM SectionLayout sl
WHERE sl.TableName = 'Company' 
  AND sl.AccountId = 1
ORDER BY sl.SectionLayoutId;

WITH ExtendedColumns AS (
    SELECT
        c.COLUMN_NAME AS FieldName,
        CASE 
            WHEN fm.FieldTypeId IS NOT NULL THEN ft.Id
            WHEN c.DATA_TYPE = 'nvarchar' AND c.CHARACTER_MAXIMUM_LENGTH > -1 THEN 1
            WHEN c.DATA_TYPE = 'nvarchar' AND c.CHARACTER_MAXIMUM_LENGTH = -1 THEN 2
            WHEN c.DATA_TYPE = 'int' THEN 3
            WHEN c.DATA_TYPE = 'bit' THEN 6
            ELSE NULL
        END AS CustomFieldTypeId,
        COALESCE(fl.SectionLayoutId, @defaultSectionLayoutId) AS SectionLayoutId,
        CASE WHEN c.IS_NULLABLE = 'NO' THEN CONVERT(BIT, 0) ELSE CONVERT(BIT, 1) END AS IsNullable,
        CASE WHEN c.IS_NULLABLE = 'NO' THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END AS IsLocked,
        COALESCE(fl.Required, 0) AS IsRequired,
        COALESCE(fl.Visible, 1) AS IsVisible,
        COALESCE(fl.Sort, c.ORDINAL_POSITION) AS SortOrder,
        CONVERT(BIT, 0) AS IsCustomField -- Explicitly stating that these are not custom fields
    FROM 
        INFORMATION_SCHEMA.COLUMNS c
    LEFT JOIN 
        FieldLayout fl 
        ON c.TABLE_NAME = fl.TableName 
        AND c.COLUMN_NAME = fl.FieldName
        AND fl.TableName = 'Company'
        AND fl.AccountId = 1
    LEFT JOIN
        FieldMapping fm
        ON fm.TableName = c.TABLE_NAME
        AND fm.FieldName = c.COLUMN_NAME
    LEFT JOIN
        FieldType ft
        ON fm.FieldTypeId = ft.Id
    WHERE 
        c.TABLE_NAME = 'Company'
        AND c.COLUMN_NAME IN ('CompanyName', 'Website', 'Notes', 'Tags', 'StatusId')
)
SELECT * FROM ExtendedColumns
UNION ALL
SELECT
    cf.FieldName,
    cf.FieldTypeId AS CustomFieldTypeId,
    COALESCE(fl.SectionLayoutId, @defaultSectionLayoutId) AS SectionLayoutId,
    1 AS IsNullable,
    0 AS IsLocked,
    COALESCE(fl.Required, 0) AS IsRequired,  -- Fetch from FieldLayout or default to false
    COALESCE(fl.Visible, 1) AS IsVisible,    -- Fetch from FieldLayout or default to true
    COALESCE(fl.Sort, 9999) AS SortOrder,
    CONVERT(BIT, 1) AS IsCustomField
FROM 
    CustomField cf
LEFT JOIN
    FieldLayout fl 
    ON fl.TableName = cf.TableName 
    AND fl.FieldName = cf.FieldName
    AND fl.AccountId = cf.AccountId
WHERE 
    cf.TableName = 'Company'
    AND cf.AccountId = 1
ORDER BY
    SectionLayoutId, SortOrder;
