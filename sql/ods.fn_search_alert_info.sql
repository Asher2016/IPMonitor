CREATE OR REPLACE FUNCTION ods.fn_search_alert_info
(
    _from_date timestamp,
    _to_date timestamp,
    _search_column varchar,
    _search_text varchar,
    _page_size integer,
    _page_index integer,
    _region varchar,
    _is_send boolean,
    _result_alert_info REFCURSOR = null,
    _result_count REFCURSOR = null
)
RETURNS SETOF REFCURSOR
AS $$
DECLARE
	_upper_search_text varchar;
	_info_offset integer;
    _region_list varchar[];
BEGIN
    SET client_encoding = euc_cn;
	_upper_search_text := '%' || upper(_search_text) || '%';
	_info_offset := (_page_index - 1) * _page_size;
    _to_date := _to_date + '1 DAY'::interval;
    
    SELECT
        array_agg(region)
    INTO
        _region_list
    FROM
        public.fn_split_strings(upper(_region), ',') AS region;

	CREATE TEMP TABLE _alert_record_info
	(
		ip varchar,
		region varchar,
		model varchar,
        first_lost_time timestamp,
        second_lost_time timestamp,
        is_send boolean
	) ON COMMIT DROP;

	INSERT INTO _alert_record_info
	(
        ip,
        region,
        model,
        first_lost_time,
        second_lost_time,
        is_send
	)
	SELECT
        alert_info.ip,
        region_info.display_name,
        ip_info.model,
        alert_info.first_lost_time,
        alert_info.second_lost_time,
        alert_info.send
	FROM
        ods.alert_info AS alert_info
        INNER JOIN
		ods.monitor_ip_info AS ip_info
            ON alert_info.ip = ip_info.ip
            AND ip_info.mark_for_delete = false
        INNER JOIN
        ods.region_info AS region_info
            ON ip_info.region_sid = region_info.sid
	WHERE
		(
            _search_text IS null
            OR
            _search_text = ''
            OR
            CASE
                WHEN upper(_search_column) = 'IP'
                    THEN
                        UPPER(alert_info.ip) like _upper_search_text
                WHEN upper(_search_column) = 'REGION'
                    THEN
                        UPPER(region_info.name) like _upper_search_text
                WHEN upper(_search_column) = 'MODEL'
                    THEN
                        UPPER(ip_info.model) like _upper_search_text 
            END
        )
        AND
        (
            _region IS NULL
            OR _region = ''
            OR upper(region_info.name) =any (_region_list)
        )
        AND alert_info.send = _is_send
        AND alert_info.create_date >= _from_date
        AND alert_info.create_date < _to_date;

	OPEN _result_alert_info FOR
    SELECT
    	ip,
        region,
        model,
        first_lost_time,
        second_lost_time,
        is_send
    FROM
    	_alert_record_info
    LIMIT
    	_page_size::integer
    OFFSET
    	_info_offset::integer;

    RETURN NEXT _result_alert_info;

    OPEN _result_count FOR
    SELECT
    	cast(count(1) AS bigint)
    FROM
    	_alert_record_info;

    RETURN NEXT _result_count;
END;
$$ LANGUAGE PLPGSQL;