CREATE FUNCTION [dbo].[RemoveSpecialChars] ( @InputString VARCHAR(8000) )
RETURNS VARCHAR(8000)  
BEGIN
    IF @InputString IS NULL
        RETURN NULL
    DECLARE @OutputString VARCHAR(8000)
    SET @OutputString = ''
    DECLARE @l INT
    SET @l = LEN(@InputString)
    DECLARE @p INT
    SET @p = 1
    WHILE @p <= @l
        BEGIN
            DECLARE @c INT
            SET @c = ASCII(SUBSTRING(@InputString, @p, 1))
            IF @c BETWEEN 48 AND 57
                OR @c BETWEEN 65 AND 90
                OR @c BETWEEN 97 AND 122
                  --OR @c = 32 --space
                  --OR @c = 39 --Apostrp'
                  --OR @c = 92 --\
                SET @OutputString = @OutputString + CHAR(@c)
            SET @p = @p + 1
        END
    IF LEN(@OutputString) = 0
        RETURN NULL
    RETURN @OutputString
END
