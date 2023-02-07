CREATE TRIGGER modifiedDate
ON Orders
AFTER INSERT, UPDATE
AS
	UPDATE [Orders] SET ModifiedDate = (SELECT getdate())
	FROM [Orders] x
		INNER JOIN inserted y
		ON x.Id = y.Id
GO