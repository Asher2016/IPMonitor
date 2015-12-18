CREATE OR REPLACE FUNCTION public.fn_time_converter
(
    _time_zone_id varchar,
    _time timestamp,
    _from_utc_or_not boolean
)
RETURNS timestamp
AS $$
DECLARE
    _timezone_name varchar;
BEGIN
    -- Convert .NET standard timezone name to PSQL timezone name.
    _timezone_name := CASE _time_zone_id
                          WHEN 'Newfoundland Standard Time' THEN 'Canada/Newfoundland'
                          WHEN 'Central Standard Time' THEN 'US/Central'
                          WHEN 'Eastern Standard Time' THEN 'US/Eastern'
                          WHEN 'Mountain Standard Time' THEN 'US/Mountain'
                          WHEN 'Pacific Standard Time' THEN 'US/Pacific'
                          WHEN 'Atlantic Standard Time' THEN 'Canada/Atlantic'
                          WHEN 'Hawaiian Standard Time' THEN 'US/Hawaii'
                          WHEN 'US Mountain Standard Time' THEN 'US/Arizona'
                          WHEN 'Canada Central Standard Time' THEN 'Canada/Saskatchewan'
                          WHEN 'Alaskan Standard Time' THEN 'US/Alaska'
                          WHEN 'US Eastern Standard Time' THEN 'US/East-Indiana'
                          ELSE 'UTC'
                      END;

    -- Convert time value from UTC to given timezone or form given timezone to UTC.
    IF _from_utc_or_not
    THEN
        -- Convert UTC to local
        RETURN (_time AT TIME ZONE 'UTC') AT TIME ZONE _timezone_name;
    ELSE
        -- Convert local to UTC
        RETURN (_time AT TIME ZONE _timezone_name) AT TIME ZONE 'UTC';
    END IF;

END;
$$ LANGUAGE PLPGSQL;