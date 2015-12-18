CREATE OR REPLACE FUNCTION public.fn_is_numeric
(
    _value varchar
)
RETURNS boolean
AS $$
BEGIN
    return _value ~  '^[-+]?\d+(\.\d+)?$';
END;
$$ LANGUAGE PLPGSQL;