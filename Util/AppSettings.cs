﻿using Config.Net;
namespace AudioReplacer2.Util
{
    public interface IAppSettings
    {
        [Option(Alias = "Theme", DefaultValue = 0)]
        int AppThemeSetting { get; set; }
        [Option(Alias = "TransparencyEffect", DefaultValue = 0)]
        int AppTransparencySetting { get; set; }
        [Option(Alias = "EnableUpdateChecks", DefaultValue = 1)]
        int AppUpdateCheck { get; set; }
        [Option(Alias = "RecordEndWaitTime", DefaultValue = 75)]
        int RecordEndWaitTime { get; set; }
        [Option(Alias = "NotificationTimeout", DefaultValue = 1750)]
        int NotificationTimeout { get; set; }
    }
}
