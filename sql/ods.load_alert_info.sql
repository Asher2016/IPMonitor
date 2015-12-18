CREATE OR REPLACE FUNCTION ods.load_alert_info
(
    _alert_info ods.tvp_alert_info[]
)
RETURNS void
AS $$
DECLARE
    _utc_now timestamp := (now() AT TIME ZONE 'UTC');
BEGIN

	INSERT INTO ods.alert_info
	(
		ip,
		first_lost_time,
		second_lost_time,
		send,
		create_date
	)
	SELECT
		record.ip,
		record.first_lost_time,
		record.second_lost_time,
		record.send,
		_utc_now
	FROM
		unnest(_alert_info) AS record;
END;
$$ LANGUAGE PLPGSQL;