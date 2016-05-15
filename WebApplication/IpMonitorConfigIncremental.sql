INSERT INTO config.start_up_config
(
	config_type,
	config_data,
	create_date,
	modified_date,
	mark_for_delete
)
VALUES
(
	'IPMonitorConfig',
	'<?xml version=''1.0'' encoding=''utf-16''?>
	<IPMonitorConfig xmlns:xsi=''http://www.w3.org/2001/XMLSchema-instance'' xmlns:xsd=''http://www.w3.org/2001/XMLSchema''>
		<Timeout>1</Timeout>
		<MaxLostCount>5</MaxLostCount>
		<MaxRecoverCount>2</MaxRecoverCount>
		<InvalidCount>6</InvalidCount>
	</IPMonitorConfig>',
	now()::timestamp without time zone,
	now()::timestamp without time zone,
	FALSE
);

UPDATE config.start_up_config
SET
	config_data = '<?xml version=''1.0'' encoding=''utf-16''?>
<IPMonitorConfig xmlns:xsi=''http://www.w3.org/2001/XMLSchema-instance'' xmlns:xsd=''http://www.w3.org/2001/XMLSchema''>
	<Timeout>1</Timeout>
	<MaxLostCount>5</MaxLostCount>
	<MaxRecoverCount>2</MaxRecoverCount>
	<InvalidCount>6</InvalidCount>
</IPMonitorConfig>'
WHERE
	config_type = 'IPMonitorConfig';
