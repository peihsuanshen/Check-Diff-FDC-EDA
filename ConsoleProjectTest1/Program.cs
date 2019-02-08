using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;


namespace ConsoleProjectTest1
{
    class ParseContextDic
    {
        public Dictionary<String, XmlElement> ReadContent3(String filePath)
        {
            //Console.WriteLine("GetFolderPath: {0}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Dictionary<string, XmlElement> cntxtDic = new Dictionary<string, XmlElement>();
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            XmlDocument doc = new XmlDocument();
            doc.Load(fs);

            foreach (XmlElement item in doc.GetElementsByTagName("type"))
            {
                Console.WriteLine("here comes a variable : {0}", item.FirstChild.InnerText);
                cntxtDic.Add(item.FirstChild.InnerText, item);
            }

            fs.Close();
            return cntxtDic;
        }
        public void AppendVariable(String filePath, XmlNode element)
        {
            //FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList typeList = doc.DocumentElement.GetElementsByTagName("type");
            XmlNode lastTypeNode = typeList.Item(typeList.Count-1);
            Console.WriteLine("last child is :{0}", lastTypeNode.FirstChild.InnerText);

            XmlNode importNode = doc.ImportNode(element, true);
            lastTypeNode.ParentNode.AppendChild(importNode);
            doc.Save(filePath);
            //fs.Close();
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            ParseContextDic context = new ParseContextDic();
            String filePathFDC = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AppTest/AppTest_P6/FDCContext.xml";
            Dictionary<String, XmlElement> FDCContext = context.ReadContent3(filePathFDC);

            String filePathEDA = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AppTest/AppTest_EDA_P6/EDAContext.xml";
            Dictionary<String, XmlElement> EDAContext = context.ReadContent3(filePathEDA);

            Console.WriteLine("number of value in FDC:{0}", FDCContext.Count);
            Console.WriteLine("number of value in EDA:{0}", EDAContext.Count);

            Boolean addCntxtFlag = false;
            foreach (String key in EDAContext.Keys)
            {
                Console.WriteLine("Key in EDA Context: {0}", key);
                if (!addCntxtFlag)
                {
                    File.Copy(filePathFDC, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AppTest/AppTest_P6/FDCContext.xml.bak." + DateTime.Today.ToString("yyyyMMdd"));
                    addCntxtFlag = true;
                    Console.WriteLine("Back up old context.sic");
                }
                if (!FDCContext.ContainsKey(key))
                {

                    context.AppendVariable(filePathFDC, EDAContext[key]);
                }

            }
        }
    }
}
