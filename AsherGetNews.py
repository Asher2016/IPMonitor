import requests
import re
import time
import datetime
import sys
import os

def GetPage(pageIndex):

    url = 'http://roll.news.sina.com.cn/interface/rollnews_ch_out_interface.php?col=90&spec=&type=&ch=01&k=&offset_page=0&offset_num=0&num=300&asc=&page='+str(pageIndex)
    
    hea = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.80 Safari/537.36'}
    html = requests.get(url,headers=hea)
    html.encoding = 'gbk'
    list = re.findall(r'},title : "(.*?)",url : "(.*?)",type : \'.*?\',pic : \'.*?\',time : (.*?)}', html.text, re.S);
    return list

def GetNewsController():
    if (len(sys.argv) == 1):
        fromDate = datetime.date.today().strftime("%Y-%m-%d")
        nextDate = datetime.date.today() + datetime.timedelta(days=1)
        toDate = nextDate.strftime("%Y-%m-%d")
    elif (len(sys.argv) == 2):
        fromDate = sys.argv[1]
        nextDate = datetime.datetime.strptime(sys.argv[1],'%Y-%m-%d') + datetime.timedelta(days=1)
        toDate = nextDate.strftime("%Y-%m-%d")
    elif (len(sys.argv) == 3):
        fromDate = sys.argv[1]
        toDate = sys.argv[2]
        if (sys.argv[1] > sys.argv[2]):
            print("开始时间不能比结束时间晚。")
            sys.exit()

    print("开始收集新闻信息：")
    print("开始日期："+fromDate+", 结束日期：" + toDate)
    BeginSendRequest(fromDate, toDate)
    
def datetime_timestamp(dt):
     #dt为字符串
     #中间过程，一般都需要将字符串转化为时间数组
     time.strptime(dt, '%Y-%m-%d %H:%M:%S')
     ## time.struct_time(tm_year=2012, tm_mon=3, tm_mday=28, tm_hour=6, tm_min=53, tm_sec=40, tm_wday=2, tm_yday=88, tm_isdst=-1)
     #将"2012-03-28 06:53:40"转化为时间戳
     s = time.mktime(time.strptime(dt, '%Y-%m-%d %H:%M:%S'))
     return int(s)
     
def timestamp_datetime(value):
    format = '%Y-%m-%d %H:%M:%S'
    # value为传入的值为时间戳(整形)，如：1332888820
    value = time.localtime(value)
    ## 经过localtime转换后变成
    ## time.struct_time(tm_year=2012, tm_mon=3, tm_mday=28, tm_hour=6, tm_min=53, tm_sec=40, tm_wday=2, tm_yday=88, tm_isdst=0)
    # 最后再经过strftime函数转换为正常日期格式。
    dt = time.strftime(format, value)
    return dt
    
def BeginSendRequest(fromDate, toDate):
    filePath = os.getcwd()+os.path.sep
    fileName = fromDate+"-"+toDate+".html"
    toDate = toDate + " 23:59:59"
    
    print("FileName: " + fileName)
    print("toDate:" + toDate)
    pageIndex = 1
    
    lateDatetime = fromDate
    
    newsList = GetPage(1)
    with open(filePath+fileName,'a') as newslistFile:
        newslistFile.write('<html><meta http-equiv="Content-Type" content="text/html; charset=GBK" /><body>')
        for item in newsList:
            currentItemDatetime = timestamp_datetime(int(item[2]))
            lateDatetime = currentItemDatetime
            if (currentItemDatetime > fromDate and currentItemDatetime < toDate):
                #newslistFile.write(fromDate + "-" + toDate + "," + currentItemDatetime + "--" + str(currentItemDatetime > fromDate and currentItemDatetime < toDate) + '\n')
                newslistFile.write(timestamp_datetime(int(item[2]))+"|"+"<a href='"+item[1]+"' target='_blank'>"+item[0]+"</a><br/>\n")
    
    while lateDatetime > fromDate and lateDatetime < toDate:
        pageIndex = pageIndex + 1
        newsList = GetPage(pageIndex)
        with open(filePath+fileName,'a') as newslistFile:
            for item in newsList:
                currentItemDatetime = timestamp_datetime(int(item[2]))
                lateDatetime = currentItemDatetime
                if (currentItemDatetime > fromDate and currentItemDatetime < toDate):
                    #newslistFile.write(fromDate + "-" + toDate + "," + currentItemDatetime + "--" + str(currentItemDatetime > fromDate and currentItemDatetime < toDate) + '\n')
                    newslistFile.write(timestamp_datetime(int(item[2]))+"|"+"<a href='"+item[1]+"' target='_blank'>"+item[0]+"</a><br/>\n")
    
            newslistFile.write('</body></html>')

GetNewsController()
