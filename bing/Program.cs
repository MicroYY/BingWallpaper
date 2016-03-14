using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;

namespace bing
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string day = GetInputString("请输入查询天数：");  //-1~15
                string mkt = GetInputString("请输入国家代码：");  //JA-JP、ZH-CN、EN-IN、DE-DE、FR-FR、EN-GB、PT-BR、EN-CA、FR-CA、EN-US、EN-WW、EN-AU
                string content = GetBingData(day, mkt);
                Console.WriteLine("查询结果：" + content);
                string fileUrl = GetBingImageUrl(content);
                DownLoadImage(fileUrl);
                SetWallpaper(Directory.GetCurrentDirectory() + "\\" + Path.GetFileName(fileUrl));
            }
        }
        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        public static void SetWallpaper(string path)
        {
            SystemParametersInfo(20, 0, path, 0x01 | 0x02);
        }
        public static void DownLoadImage(string url)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, Path.GetFileName(url));
        }
        public static string GetBingImageUrl(string str)
        {
            string[] strArray = str.Split(new string[] { "地址：" }, StringSplitOptions.RemoveEmptyEntries);
            return strArray[1];
        }
        public static string GetBingData(string day, string mkt)
        {
            string url = "http://test.dou.ms/bing/day/" + day + "/mkt/" + mkt;
            return GetHttpData(url);
        }
        public static string GetHttpData(string uri)
        {
            Uri url = new Uri(uri);//初始化uri
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//初始化请求
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//得到响应
            Stream stream = response.GetResponseStream();//获取响应的主体
            StreamReader reader = new StreamReader(stream);//初始化读取器
            string result = reader.ReadToEnd();//读取，存储结果
            reader.Close();//关闭读取器，释放资源
            stream.Close();//关闭流，释放资源
            return result;//返回读取结果
        }
        public static string GetInputString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
