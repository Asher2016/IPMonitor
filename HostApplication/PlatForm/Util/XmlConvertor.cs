using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PlatForm.Util
{
    public static class XmlConvertor
    {
        public static T XmlToObject<T>(string xml, Encoding encoding = null)
        {
            T obj = (T)XmlToObject(typeof(T), xml, encoding);
            return obj;
        }

        public static object XmlToObject(Type type, string xml, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            object obj = null;
            if (!string.IsNullOrEmpty(xml))
            {
                xml = xml.Trim();
                object result;
                using (MemoryStream stream = new MemoryStream(encoding.GetBytes(xml)))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);

                    result = serializer.Deserialize(stream);
                    obj = result;
                }
            }

            return obj;
        }


        public static string ObjectToXml(object obj, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            return ObjectToXml(obj, true, encoding);
        }

        public static string ObjectToXml(object obj)
        {
            Encoding encoding = Encoding.Unicode;
            return ObjectToXml(obj, true, encoding);
        }

        public static string ObjectToXml(object obj, bool isIndented)
        {
            return ObjectToXml(obj, isIndented, Encoding.Unicode);
        }

        public static string ObjectToXml(object obj, bool isIndented, Encoding encoding)
        {
            string xml = null;
            if (obj != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = isIndented;
                    settings.Encoding = encoding;
                    using (XmlWriter writer = XmlWriter.Create(stream, settings))
                    {
                        XmlSerializer serializer = new XmlSerializer(obj.GetType());
                        serializer.Serialize(writer, obj);
                    }

                    xml = GetNoBomString(encoding, stream.ToArray());
                }
            }

            return xml;
        }

        private static string GetNoBomString(Encoding encoding, byte[] buffer)
        {
            byte[] bom = encoding.GetPreamble();
            byte[] testBom = buffer.Take(bom.Length).ToArray();

            bool bomEquals = true;
            for (int i = 0; i < bom.Length; i++)
            {
                bomEquals = bom[i] == testBom[i];
                if (!bomEquals)
                {
                    break;
                }
            }

            byte[] noBomBuffer = buffer;
            if (bomEquals)
            {
                noBomBuffer = buffer.Skip(bom.Length).ToArray();
            }

            string noBomResult = encoding.GetString(noBomBuffer);

            return noBomResult;
        }
    }
}