CREATE OR REPLACE FUNCTION ods.fn_search_ip_region_info
(
    _search_column varchar,
    _search_text varchar,
    _page_size integer,
    _page_index integer,
    _region varchar,
    _result_ip_region_list REFCURSOR = null,
    _result_info_count REFCURSOR = null
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

    SELECT
        array_agg(region)
    INTO
        _region_list
    FROM
        public.fn_split_strings(upper(_region), ',') AS region;

	CREATE TEMP TABLE _ip_region_info
	(
		sid bigint,
		ip varchar,
		region varchar,
        region_display_name varchar,
		model varchar,
        telephone varchar,
        mark_for_delete boolean
	) ON COMMIT DROP;

	INSERT INTO _ip_region_info
	(
        sid,
        ip,
        region,
        region_display_name,
        model,
        telephone,
        mark_for_delete
	)
	SELECT
		ip_info.sid,
        ip_info.ip,
        region_info.name,
        region_info.display_name,
        ip_info.model,
        ip_info.telephone,
        ip_info.mark_for_delete
	FROM
		ods.monitor_ip_info AS ip_info
        LEFT JOIN
        ods.region_info AS region_info
            ON ip_info.region_sid = region_info.sid
	WHERE
		(
            _search_text IS null
            OR
            _search_text = ''
            OR
            CASE
                WHEN UPPER(_search_column) = 'IP'
                    THEN
                        UPPER(ip_info.ip) like _upper_search_text
                WHEN UPPER(_search_column) = 'MODEL'
                    THEN
                        UPPER(ip_info.model) like _upper_search_text  
            END
        )
        AND
        (
            _region = ''
            OR _region IS NULL
            OR upper(region_info.name) =any (_region_list)
        )
        AND ip_info.mark_for_delete = false
    ORDER BY
        ip_info.modified_date DESC;

	OPEN _result_ip_region_list FOR
    SELECT
    	sid,
        ip,
        region,
        region_display_name,
        model,
        telephone
    FROM
    	_ip_region_info
    LIMIT
    	_page_size::integer
    OFFSET
    	_info_offset::integer;

    RETURN NEXT _result_ip_region_list;

    OPEN _result_info_count FOR
    SELECT
    	cast(count(1) AS bigint)
    FROM
    	_ip_region_info
    WHERE
        mark_for_delete = false;

    RETURN NEXT _result_info_count;
END;
$$ LANGUAGE PLPGSQL;