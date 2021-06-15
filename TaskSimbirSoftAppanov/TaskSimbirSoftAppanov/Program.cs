using System;

namespace TaskSimbirSoftAppanov
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите URL на HTML страницу в формате https://www.simbirsoft.com/");
            string URL = Console.ReadLine();

            HtmlPage page = new HtmlPage(URL);
            Console.WriteLine(page.GetStatisticsDictionary());
        }
    }
}
