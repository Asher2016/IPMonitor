CREATE OR REPLACE FUNCTION ods.ip_user_map_search
(
    _search_column varchar,
    _search_text varchar,
    _page_size integer,
    _page_index integer,
    _result_info REFCURSOR = null,
    _result_count REFCURSOR = null
)
RETURNS SETOF REFCURSOR
AS $$
DECLARE
	_upper_search_text varchar;
	_info_offset integer;
BEGIN
    SET client_encoding = euc_cn;
	_upper_search_text := '%' || upper(_search_text) || '%';
	_info_offset := (_page_index - 1) * _page_size;

	CREATE TEMP TABLE _ip_user_map
	(
        sid bigint,
		ip_address varchar,
		user_name varchar
	) ON COMMIT DROP;

	INSERT INTO _ip_user_map
	(
        sid,
		ip_address,
		user_name
	)
	SELECT
        sid,
		ip_address,
		user_name
	FROM
		ods.ip_user_map AS ip_user_map
	WHERE
       ( _search_text IS null
        OR
        _search_text = ''
        OR
        CASE
            WHEN upper(_search_column) = 'IP_ADDRESS'
                THEN
                    ip_user_map.ip_address = _search_text
            WHEN upper(_search_column) = 'USER_NAME'
                THEN
                    upper(ip_user_map.user_name) like _upper_search_text
        END
        )
        AND mark_for_delete = false
    ORDER BY
        ip_user_map.modified_date DESC;

	OPEN _result_info FOR
    SELECT
        sid,
    	ip_address,
		user_name
    FROM
    	_ip_user_map
    LIMIT
    	_page_size::integer
    OFFSET
    	_info_offset::integer;

    RETURN NEXT _result_info;

    OPEN _result_count FOR
    SELECT
    	cast(count(1) AS bigint)
    FROM
    	_ip_user_map;

    RETURN NEXT _result_count;
END;
$$ LANGUAGE PLPGSQL;