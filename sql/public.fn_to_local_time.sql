--
-- Convert UTC TIME to local TIME
--
CREATE OR REPLACE FUNCTION public.fn_to_local_time
(
    _time_zone_id VARCHAR,
    _utc_time TIMESTAMP
)
RETURNS TIMESTAMP
AS $$
BEGIN
    RETURN public.fn_time_converter(_time_zone_id,_utc_time, TRUE);
END;
$$ LANGUAGE PLPGSQL;