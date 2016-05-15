--
-- Convert local TIME to UTC TIME
--
CREATE OR REPLACE FUNCTION public.fn_to_utc_time
(
    _time_zone_id VARCHAR,
    _local_time TIMESTAMP
)
RETURNS TIMESTAMP
AS $$
BEGIN
    RETURN public.fn_time_converter(_time_zone_id, _local_time, FALSE);
END;
$$ LANGUAGE PLPGSQL;