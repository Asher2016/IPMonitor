CREATE OR REPLACE FUNCTION ods.fn_log_info_search
(
    _from_date timestamp,
    _to_date timestamp,
    _search_column varchar,
    _search_text varchar,
    _page_size integer,
    _page_index integer,
    _result_log_info REFCURSOR = null,
    _result_log_count REFCURSOR = null
)
RETURNS SETOF REFCURSOR
AS $$
DECLARE
	_upper_search_text varchar;
	_info_offset integer;
BEGIN
	_upper_search_text := '%' || upper(_search_text) || '%';
	_info_offset := (_page_index - 1) * _page_size;

	CREATE TEMP TABLE _log_information
	(
		ip_address varchar,
		log_level varchar,
		local_time timestamp,
		remote_time timestamp,
		message varchar
	) ON COMMIT DROP;

	INSERT INTO _log_information
	(
		ip_address,
		log_level,
		local_time,
		remote_time,
		message
	)
	SELECT
		coalesce(ip_user.user_name, log_info.ip_address),
		log_level,
		local_time,
		remote_time,
		message
	FROM
		ods.log_information AS log_info
        LEFT JOIN
        ods.ip_user_map AS ip_user
            ON ip_user.ip_address = log_info.ip_address
	WHERE
		log_info.local_time >= _from_date
		AND log_info.local_time < _to_date
		AND
			(
				_search_text IS null
				OR
				_search_text = ''
				OR
				CASE
					WHEN UPPER(_search_column) = 'IP_ADDRESS'
						THEN
							log_info.ip_address LIKE _upper_search_text
					WHEN UPPER(_search_column) = 'LOG_LEVEL'
						THEN
							log_info.log_level = _search_text
					WHEN UPPER(_search_column) = 'MESSAGE'
						THEN
							UPPER(log_info.message) like _upper_search_text 
				END
			);
		

	OPEN _result_log_info FOR
    SELECT
    	ip_address,
		log_level,
		local_time,
		remote_time,
		message
    FROM
    	_log_information
    LIMIT
    	_page_size::integer
    OFFSET
    	_info_offset::integer;

    RETURN NEXT _result_log_info;

    OPEN _result_log_count FOR
    SELECT
    	cast(count(1) AS bigint)
    FROM
    	_log_information;

    RETURN NEXT _result_log_count;
END;
$$ LANGUAGE PLPGSQL;