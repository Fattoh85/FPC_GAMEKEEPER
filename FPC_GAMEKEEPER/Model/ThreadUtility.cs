using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace FPC.Model
{
    public class ThreadUtility
    {
        private static AsyncLocal<string> _uid = new AsyncLocal<string>();

        public static string Uid
        {
            get => _uid.Value;
            set => _uid.Value = value;
        }


        public static string uidValue { get; set; }

        public ThreadUtility()
        {
            string TreadUid = null; bool newSession = true;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            string val = null;

            if ((Uid == null || Uid == "") & newSession == true)
            {
                val = RandomString(12);
            }
            else
            {
                if (newSession & TreadUid == null)
                {
                    val = RandomString(12);
                }
                else
                {
                    val = TreadUid;
                }
            }

            if (val != null)
            {
                ThreadContext.Properties["uid"] = val;
                Uid = val;
            }
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public string GetCurrentMethod([System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            ThreadContext.Properties["uid"] = Uid;
            return $"[{methodName}] ";
        }

        public string BeautifyXml(string xmlData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);
            // Создаем XmlWriter с настройками форматирования
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t"; // Используем табуляцию для отступов
            settings.NewLineChars = "\n"; // Используем перенос строки для новой строки
            settings.NewLineHandling = NewLineHandling.Replace;
            // Записываем XML в строку с использованием XmlWriter
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    xmlDoc.WriteTo(xmlWriter);
                }
                return stringWriter.ToString();
            }
        }


    }
}
