using SmartBuildingVisualization.Shared.Models;
using System.ComponentModel;

namespace SmartBuildingVisualization.Client.Core.Interfaces
{
    public interface IDateService
    {
        DateTime AddDate(DateTime date, TimeFormat tf, int offset = 1);

        DateTime ResetDate(DateTime date, TimeFormat timeFormat, bool down);

        string DateToString(DateTime date, TimeFormat timeFormat);

        string DateToStringLongFormat(DateTime date, TimeFormat tf);

    }
}
