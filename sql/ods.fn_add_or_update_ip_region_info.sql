CREATE OR REPLACE FUNCTION ods.fn_add_or_update_ip_region_info
(
    _sid bigint,
    _ip varchar,
    _region varchar,
    _model varchar,
    _telephone varchar
)
RETURNS void
AS $$
DECLARE
    _utc_now timestamp := (now() AT TIME ZONE 'UTC');
    _region_sid bigint;
BEGIN
    SELECT
        region_info.sid
    INTO
        _region_sid
    FROM
       ods.region_info AS region_info
    WHERE
        upper(region_info.name) = upper(_region)
        AND region_info.mark_for_delete = FALSE
    LIMIT 1;
        
	IF (_sid > 0)
    THEN
        UPDATE ods.monitor_ip_info
        SET
            ip = _ip,
            region_sid = _region_sid,
            model = _model,
            telephone = _telephone,
            modified_date = _utc_now
        WHERE
            sid = _sid;
    ELSE
        INSERT INTO ods.monitor_ip_info
        (
            region_sid,
            ip,
            model,
            telephone,
            modified_date,
            create_date,
            mark_for_delete
        )
        VALUES
        (
            _region_sid,
            _ip,
            _model,
            _telephone,
            _utc_now,
            _utc_now,
            FALSE
        );
    END IF;
END;
$$ LANGUAGE PLPGSQL;