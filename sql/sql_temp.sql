create table ods.region_info
(
	sid bigserial,
	name varchar,
	display_name varchar,
	mark_for_delete boolean,
	create_date timestamp,
	modified_date timestamp
)

alter table ods.region_info add constraint pk_region_info primary key (sid)

insert into ods.region_info(name,display_name,mark_for_delete,create_date, modified_date)
values
('PianGuan', '偏关县',false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('HeQu','河曲县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('BaoDe','保德县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('ShenChi','神池县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('WuZhai','五寨县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('KeLan','岢岚县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('NingWu','宁武县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('JingLe','静乐县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('YuanPing','原平县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('XiFu','忻府区', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('JingLe','代县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('JingLe','五台县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('JingLe','定襄县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC'),
('JingLe','繁峙县', false, now() AT TIME ZONE 'UTC', now() AT TIME ZONE 'UTC');

create table ods.monitor_ip_info
(
	sid bigserial,
	region_sid bigint,
	ip varchar,
	model varchar,
	create_date timestamp,
	modified_date timestamp,
	mark_for_delete boolean
)

drop table  ods.monitor_ip_info;

alter table ods.monitor_ip_info add constraint fk_ip_region foreign key (region_sid) references ods.region_info(sid)

select * from ods.region_info;

insert into ods.monitor_ip_info(region_sid, ip,model, create_date,modified_date, mark_for_delete)
values(1, 'www.baidu.com','1111', now() AT TIME ZONE 'UTC',now() AT TIME ZONE 'UTC',false),
(1, 'www.youku.com','2222', now() AT TIME ZONE 'UTC',now() AT TIME ZONE 'UTC',false),
(1, 'www.qidian.com','2222', now() AT TIME ZONE 'UTC',now() AT TIME ZONE 'UTC',false),
(2, 'www.sina.com','3333', now() AT TIME ZONE 'UTC',now() AT TIME ZONE 'UTC',false),
(3, 'www.douyutv.com','4444', now() AT TIME ZONE 'UTC',now() AT TIME ZONE 'UTC',false),
(4, 'www.baidu.com','1111', now() AT TIME ZONE 'UTC',now() AT TIME ZONE 'UTC',false)

select * from ods.monitor_ip_info;
select ip,name from ods.monitor_ip_info AS ip_info left join ods.region_info AS region_info ON ip_info.region_sid = region_info.sid where ip_info.mark_for_delete = false;
