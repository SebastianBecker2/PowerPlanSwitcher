namespace PowerPlanSwitcher 
{
    using System.Runtime.InteropServices;
    
    public class BatteryMonitor
    {
        // 定义SYSTEM_POWER_STATUS结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_POWER_STATUS
        {
            public byte ACLineStatus; // AC 电源线状态：0 表示离线（电池供电），1 表示在线（插电）
            public byte BatteryFlag;  // 电池标志
            public byte BatteryLifePercent; // 电池剩余百分比
            public byte Reserved1;    // 保留
            public int BatteryLifeTime; // 电池剩余时间（以秒为单位）
            public int BatteryFullLifeTime; // 电池满电使用时间（以秒为单位）
        }
        
        // 引入GetSystemPowerStatus函数
        [DllImport("kernel32.dll")] //和GetSystemPowerStatus有关
        private static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS lpSystemPowerStatus);
        private static bool ToggleMark = true; // 切换标记
        // private static PowerRule ACListData;
        // private static PowerRule BatteryListData;
        private static Guid? BatteryGuid; // 电池供电时的电源计划GUID
        private static Guid? AcPowerGuid; // 插电时的电源计划GUID
        public  static PowerStatus BatteryNull;
        private static SYSTEM_POWER_STATUS powerStatus;
        
        public static void PlanValue()
        {
            BatteryNull = SystemInformation.PowerStatus;
            if (BatteryNull.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
            {  
                // 调用SettingsHelper.GetGuidsWithVisibility()方法并解构返回的元组
                var (_acGuid, _batteryGuid) = PowerSchemeSettings.GetGuidsWithVisibility();
                
                // 将解构得到的GUID值赋给对应的静态字段
                AcPowerGuid = _acGuid;
                BatteryGuid = _batteryGuid;   
            }
        }
        
        public static void BatteryMonitorInitialization()
        {
            PlanValue();
            MonitorBatterySwitcInitialization();
        }
        
        private static void MonitorBatterySwitcInitialization()
        {
            if (!ToggleMark && BatteryGuid != Guid.Empty && AcPowerGuid == Guid.Empty  && powerStatus.ACLineStatus == 1) // 插电状态 02
            {
                ToggleMark = true;
            }
            else if (ToggleMark && AcPowerGuid != Guid.Empty && BatteryGuid == Guid.Empty  && powerStatus.ACLineStatus == 0) //电池供电 01
            {
                ToggleMark = false;
            } 
        }
        
        // 监测电池状态并切换电源计划的方法
        public static void MonitorBatterySwitc()
        {
            if (BatteryNull.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
            {
                GetSystemPowerStatus(out powerStatus);
                var PMGAPSG = PowerManager.GetActivePowerSchemeGuid();
                
                if (!ToggleMark && AcPowerGuid != Guid.Empty && powerStatus.ACLineStatus == 1 && PMGAPSG != AcPowerGuid) // 插电状态 01 03
                {
                    PowerManager.SetActivePowerScheme(AcPowerGuid.Value); // 激活插电时的电源计划
                    ProcessMonitor.baselinePowerSchemeGuid = AcPowerGuid.Value; // 电池供电时的电源计划GUID 
                    ToggleMark = true;
                }
                else if (ToggleMark && BatteryGuid != Guid.Empty && powerStatus.ACLineStatus == 0 && PMGAPSG != BatteryGuid) //电池供电 02 03
                {
                    PowerManager.SetActivePowerScheme(BatteryGuid.Value); // 激活电池供电时的电源计划
                    ProcessMonitor.baselinePowerSchemeGuid = BatteryGuid.Value;
                    ToggleMark = false;
                }
                else
                {
                    MonitorBatterySwitcInitialization();
                }

            }
        }
    }
}
