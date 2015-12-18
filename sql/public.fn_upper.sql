CREATE OR REPLACE FUNCTION public.fn_upper
(
    _arr varchar[]
)
RETURNS varchar[]
AS $$
DECLARE
    arr_len integer;
BEGIN
    arr_len := array_length(_arr, 1);

    IF arr_len > 0
    THEN
        FOR i IN 1..arr_len LOOP
            _arr[i] := upper(_arr[i]);
        END LOOP;
    END IF;

    RETURN _arr;
END;
$$ LANGUAGE PLPGSQL;