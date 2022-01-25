using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    public static class ConsoleReadlineValueParse
    {
        public static DateTime ParseDate(string dateString, SendMessage Notify)
        {
            DateTime date;
            var resultDateStart = DateTime.TryParse(dateString, out date);
            if (!resultDateStart)
            {
                Notify("Ошибка: дата не распознана");
                return DateTime.MinValue;
            }
            return date;
        }
    }
}
