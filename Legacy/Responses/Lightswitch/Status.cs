using System.Collections.Generic;

namespace Arcane_Launcher.Responses.Lightswitch
{
    public class LightswitchStatus
    {
        public string ServiceInstanceId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string MaintenanceUri { get; set; }
        public List<string> OverrideCatalogIds { get; set; }
        public List<string> AllowedActions { get; set; }
        public bool Banned { get; set; }
        public LauncherInfoDTO LauncherInfoDTO { get; set; }
    }

    public class LauncherInfoDTO
    {
        public string AppName { get; set; }
        public string CatalogItemId { get; set; }
        public string Namespace { get; set; }
    }
}