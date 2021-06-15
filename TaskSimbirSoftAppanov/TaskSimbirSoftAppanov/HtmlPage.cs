using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace TaskSimbirSoftAppanov
{
    public class HtmlPage
    {
        private readonly string URL;
        private readonly string PageStr; 

        public HtmlPage(string URL)
        {
            this.URL = URL;

            try
            {
                WebRequest req = WebRequest.Create(URL);
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                PageStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private string GetParsePage()
        {
            string ParsePage = PageStr;

            int Position1 = ParsePage.IndexOf("<body");
            int Position2 = ParsePage.IndexOf("</body>");

            // Вытаскиваем тело
            ParsePage = ParsePage.Substring(Position1, Position2 - Position1);

            // Удаляем стили
            Position1 = ParsePage.IndexOf("<style");
            Position2 = ParsePage.LastIndexOf("</style>");

            while (Position1 != -1)
            {
                ParsePage = ParsePage.Remove(Position1, Position2 - Position1 + "</style>".Length);
                Position1 = ParsePage.IndexOf("<style");
                Position2 = ParsePage.IndexOf("</style>");
            }

            // Удаляем скрипты
            Position1 = ParsePage.IndexOf("<script");
            Position2 = ParsePage.LastIndexOf("</script>");

            while (Position1 != -1)
            {
                ParsePage = ParsePage.Remove(Position1, Position2 - Position1 + "</script>".Length);
                Position1 = ParsePage.IndexOf("<script");
                Position2 = ParsePage.IndexOf("</script>");
            }

            // Удаляем все теги
            string pattern = @"<(.|\n)*?>";
            ParsePage = Regex.Replace(ParsePage, pattern, string.Empty);

            // Удаляем лишние символы
            ParsePage = ParsePage.Replace("&nbsp;", string.Empty);
            ParsePage = ParsePage.Replace("&bull;", string.Empty);
            ParsePage = ParsePage.Replace(' ', '\n');

            return ParsePage;
        }

        public string GetStatisticsDictionary()
        {
            string ParsePage = GetParsePage();

            // Сделаем встречающиеся слова ключами, а их количество значениями
            Dictionary<string, int> dictPage = new Dictionary<string, int>();

            // Разбиваем текст на отдельные слова
            Char[] СharacterArray = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' };

            foreach (string str in ParsePage.Split(СharacterArray))
            {
                if (!(str == string.Empty))
                {
                    if (dictPage.ContainsKey(str))
                    {
                        dictPage[str]++;
                    }
                    else
                    {
                        dictPage.Add(str, 1);
                    }
                }
            }

            // Оформляем
            StringBuilder dictValueKey = new StringBuilder();
            dictValueKey.Append("Статистика страницы: \n");

            foreach (string key in dictPage.Keys)
            {
                dictValueKey.Append(key);
                dictValueKey.Append(" - ");
                dictValueKey.Append(dictPage[key]);
                dictValueKey.Append('\n');
            }

            return dictValueKey.ToString();
        }
    }
}
