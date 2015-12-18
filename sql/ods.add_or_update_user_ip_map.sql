CREATE OR REPLACE FUNCTION ods.add_or_update_user_ip_map
(
    _sid bigint,
    _ip_address varchar,
    _user_name varchar
)
RETURNS void
AS $$
DECLARE
    _utc_now timestamp := (now() AT TIME ZONE 'UTC');
BEGIN
	IF (_sid > 0)
    THEN
        UPDATE ods.ip_user_map
        SET
            ip_address = _ip_address,
            user_name = _user_name,
            modified_date = _utc_now
        WHERE
            sid = _sid;
    ELSE
        INSERT INTO ods.ip_user_map
        (
            ip_address,
            user_name,
            mark_for_delete,
            modified_date,
            create_date
        )
        VALUES
        (
            _ip_address,
            _user_name,
            false,
            _utc_now,
            _utc_now
        );
    END IF;
END;
$$ LANGUAGE PLPGSQL;