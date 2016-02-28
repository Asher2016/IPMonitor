CREATE OR REPLACE FUNCTION ods.load_alert_info
(
    _alert_info ods.tvp_alert_info[]
)
RETURNS void
AS $$
DECLARE
    _local_now timestamp := (now() AT TIME ZONE 'UTC-8');
BEGIN
	CREATE TEMP TABLE _add_alert(
		ip varchar,
		first_lost_time timestamp,
		second_lost_time timestamp,
		send boolean,
		create_date timestamp,
		recovery_time timestamp
	) ON COMMIT DROP;

	CREATE TEMP TABLE _update_alert(
		sid bigint,
		ip varchar,
		first_lost_time timestamp,
		second_lost_time timestamp,
		send boolean,
		create_date timestamp,
		recovery_time timestamp
	) ON COMMIT DROP;

	INSERT INTO _add_alert(
		ip,
		first_lost_time,
		second_lost_time,
		send,
		create_date,
		recovery_time
	)
	SELECT
		record.ip,
		record.first_lost_time,
		record.second_lost_time,
		record.send,
		_local_now,
		record.recovery_time
	FROM
		unnest(_alert_info) AS record
	WHERE
		record.sid = 0;

	INSERT INTO _update_alert(
		sid,
		ip,
		first_lost_time,
		second_lost_time,
		send,
		create_date,
		recovery_time
	)
	SELECT
		record.sid,
		record.ip,
		record.first_lost_time,
		record.second_lost_time,
		record.send,
		_local_now,
		record.recovery_time
	FROM
		unnest(_alert_info) AS record
	WHERE
		record.sid > 0;

	INSERT INTO ods.alert_info
	(
		ip,
		first_lost_time,
		second_lost_time,
		send,
		create_date,
		recovery_time
	)
	SELECT
		record.ip,
		record.first_lost_time,
		record.second_lost_time,
		record.send,
		_local_now,
		record.recovery_time
	FROM
		_add_alert AS record;

	UPDATE ods.alert_info AS target
	SET
		recovery_time = record.recovery_time,
		send = false
	FROM
		_update_alert AS record
	WHERE
		target.sid = record.sid;

END;
$$ LANGUAGE PLPGSQL;