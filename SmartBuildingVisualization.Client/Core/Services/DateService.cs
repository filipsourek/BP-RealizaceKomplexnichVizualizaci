using SmartBuildingVisualization.Client.Core.Interfaces;
using SmartBuildingVisualization.Shared.Models;

namespace SmartBuildingVisualization.Client.Core.Services
{
    public class DateService : IDateService
    {
        /// <summary>
        /// Přidá zadaný časový posun k datu na základě specifikovaného formátu času (TimeFormat).
        /// </summary>
        /// <param name="date">Výchozí datum, ke kterému se přidává offset.</param>
        /// <param name="tf">Formát času (TimeFormat), který určuje, jaký typ offsetu bude přidán (minuty, hodiny, dny atd.).</param>
        /// <param name="offset">Počet jednotek, které se mají přidat. Výchozí hodnota je 1.</param>
        /// <returns>Datum s přidaným offsetem.</returns>
        public DateTime AddDate(DateTime date, TimeFormat tf, int offset = 1)
        {
           switch (tf)
            {
                case TimeFormat.Minute:
                    return date.AddMinutes(offset);
                case TimeFormat.Hour:
                    return date.AddHours(offset);
                case TimeFormat.Day:
                    return date.AddDays(offset);
                case TimeFormat.Month:
                    return date.AddMonths(offset);
                case TimeFormat.Year:
                    return date.AddYears(offset);
                default:
                    return date;
            }
        }

        /// <summary>
        /// Resetuje část data na základě zadaného formátu času (TimeFormat) a směru resetování.
        /// </summary>
        /// <param name="date">Datum, které má být resetováno.</param>
        /// <param name="tf">Formát času (TimeFormat), který určuje, jaká část data má být resetována.</param>
        /// <param name="down">Určuje, zda se má datum resetovat na začátek (true) nebo konec (false) časového intervalu.</param>
        /// <returns>Resetovaný datum</returns>
        public DateTime ResetDate(DateTime date, TimeFormat tf, bool down)
        {
            switch (tf)
            {
                case TimeFormat.Hour:
                    return down ? new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0) : new DateTime(date.Year, date.Month, date.Day, date.Hour, 59, 59);
                case TimeFormat.Day:
                    return down ? new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) : new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                case TimeFormat.Month:
                    return down ? new DateTime(date.Year, date.Month, 1, 0, 0, 0) : new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
                case TimeFormat.Year:
                    return down ? new DateTime(date.Year, 1, 1, 0, 0, 0) : new DateTime(date.Year, 12, 31, 23, 59, 59);
                default:
                    return date;
            }
        }

        /// <summary>
        /// Převede zadaný datum na řetězec ve formátu odpovídajícím zadanému formátu času (TimeFormat).
        /// </summary>
        /// <param name="date">Datum, které má být převedeno na řetězec.</param>
        /// <param name="tf">Formát času (TimeFormat), který určuje, jaký formát řetězce bude použit.</param>
        /// <returns>Datum v tetxovém řetězci.</returns>
        public string DateToString(DateTime date, TimeFormat tf)
        {
            return tf switch
            {
                TimeFormat.Minute => date.ToString("dd.MM HH:mm"),
                TimeFormat.Hour => date.ToString("dd.MM HH:mm"),
                TimeFormat.Day => date.ToString("dd.MM"),
                TimeFormat.Month => date.ToString("MMM yyyy"),
                TimeFormat.Year => date.ToString("yyyy"),
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Převede zadaný datum na řetězec v dlouhém formátu odpovídajícím zadanému formátu času (TimeFormat).
        /// </summary>
        /// <param name="date">Datum, které má být převedeno na řetězec v dlouhém formátu.</param>
        /// <param name="tf">Formát času (TimeFormat), který určuje, jaký formát řetězce bude použit.</param>
        /// <returns>Datum v dlouhém tetxovém řetězci.</returns>
        public string DateToStringLongFormat(DateTime date, TimeFormat tf)
        {
            return tf switch
            {
                TimeFormat.Hour => date.ToString("HH:mm dd. MMMM yyyy"),
                TimeFormat.Day => date.ToString("dd. MMMMM yyyy"),
                TimeFormat.Month => date.ToString("MMMM yyyy"),
                TimeFormat.Year => date.ToString("yyyy"),
                _ => string.Empty,
            };
        }


    }
}
