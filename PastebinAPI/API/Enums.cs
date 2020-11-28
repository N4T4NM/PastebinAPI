using System.ComponentModel;

namespace PastebinAPI.API
{
    public enum Visibility
    {
        Public = 0,
        Unlisted = 1,
        Private = 2
    }

    public enum ExpireTime
    {
        [Description("N")]
        Never,
        [Description("10M")]
        TenMinutes,
        [Description("1H")]
        OneHour,
        [Description("1D")]
        OneDay,
        [Description("1W")]
        OneWeek,
        [Description("2W")]
        TwoWeeks,
        [Description("1M")]
        OneMonth,
        [Description("6M")]
        SixMonths,
        [Description("1Y")]
        OneYear
    }
}
