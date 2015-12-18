CREATE OR REPLACE FUNCTION ods.load_ip_monitor_record
(
    _ip_monitor_info ods.tvp_bref_ip_info[]
)
RETURNS void
AS $$
DECLARE
    _utc_now timestamp := (now() AT TIME ZONE 'UTC');
BEGIN

	INSERT INTO ods.ip_monitor_record
	(
		ip,
		lost_time,
		recovery_time,
		create_date
	)
	SELECT
		record.ip,
		record.lost_time,
		record.recovery_time,
		_utc_now
	FROM
		unnest(_ip_monitor_info) AS record;
END;
$$ LANGUAGE PLPGSQL;