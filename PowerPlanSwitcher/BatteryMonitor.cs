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
        public static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS lpSystemPowerStatus);

        // private static Guid _batteryPlanGuid = new Guid("381b4222-f694-41f0-9685-ff5bb260df2e"); // 电池供电时的电源计划GUID
        // private static Guid _acPlanGuid = new Guid("512a6ac6-efc1-4441-85c9-cccf29b69cd2"); // 插电时的电源计划GUID
        // private static Guid _batteryPlanGuid; // 电池供电时的电源计划GUID
        // private static Guid _acPlanGuid; // 插电时的电源计划GUID
        private static bool ToggleMark = true; // 切换标记
        // // private static bool BootMark = true; // 开机标记
        private static PowerRule ACListData;
        private static PowerRule BatteryListData;
        
        public static void PlanValue()
        {
            PowerStatus BatteryNull = SystemInformation.PowerStatus;
            if (BatteryNull.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
            {  
                // var applicableRule = default(PowerRule); // 初始化applicableRule为PowerRule类型的默认值（对于引用类型是null）
                // var rules = PowerRule.GetPowerRules(); // 先获取所有规则

                // foreach (var rule in rules) // 遍历规则集合
                // {
                    // if (rule.Type.ToString() == "PowerSupply") // 检查规则类型是否为"Power AC"
                    // {
                        // applicableRule = rule; // 如果是，将当前规则赋值给applicableRule
                        // break; // 找到后退出循环，因为只需要第一个匹配的规则
                    // }
                // }
                ACListData = PowerRule.GetPowerRules()
                    .FirstOrDefault(r => r.Type.ToString() == "PowerSupply");
                    
                // _acPlanGuid = ACListData.SchemeGuid;
                
                BatteryListData = PowerRule.GetPowerRules()
                    .FirstOrDefault(r => r.Type.ToString() == "Battery");
                    
                // _batteryPlanGuid = BatteryListData.SchemeGuid; 
            }
        }

        // 监测电池状态并切换电源计划的方法
        public static void MonitorBatterySwitc()
        {
            SYSTEM_POWER_STATUS powerStatus;
            PowerStatus BatteryNull = SystemInformation.PowerStatus;
            if (BatteryNull.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
            {
                GetSystemPowerStatus(out powerStatus);
                    // if (BatteryListData != null && powerStatus.ACLineStatus == 0 && ProcessMonitor.baselinePowerSchemeGuid != BatteryListData.SchemeGuid)
                    // {
                        // ProcessMonitor.baselinePowerSchemeGuid = BatteryListData.SchemeGuid;
                    // }
                    // if (BatteryListData != null && powerStatus.ACLineStatus == 0 && BatteryListData.SchemeGuid!=当前激活) // 电池供电
                    // {

                    // }
                    
                // if (ACListData != null && BatteryListData != null)
                // {
                    // HaploidDoubles = true;
                // }
                // if ((ACListData != null) ^ (BatteryListData != null))
                // {
                    // HaploidDoubles = false;
                // }
                var PMGAPSG = PowerManager.GetActivePowerSchemeGuid();
                
                
                
                if (!ToggleMark && ACListData != null && powerStatus.ACLineStatus == 1 && PMGAPSG != ACListData.SchemeGuid) // 插电状态 01 03
                {
                    PowerManager.SetActivePowerScheme(ACListData.SchemeGuid); // 激活插电时的电源计划
                    ProcessMonitor.baselinePowerSchemeGuid = ACListData.SchemeGuid; // 电池供电时的电源计划GUID 
                    ACListData.Active = true;
                    if (BatteryListData != null)
                    {
                        BatteryListData.Active = false;
                    }
                    ToggleMark = true;
                }
                else if (ToggleMark && BatteryListData != null && powerStatus.ACLineStatus == 0 && PMGAPSG != BatteryListData.SchemeGuid) //电池供电 02 03
                {
                    PowerManager.SetActivePowerScheme(BatteryListData.SchemeGuid); // 激活电池供电时的电源计划
                    ProcessMonitor.baselinePowerSchemeGuid = BatteryListData.SchemeGuid;
                    BatteryListData.Active = true;
                    if (ACListData != null)
                    {
                        ACListData.Active = false;
                    }
                    ToggleMark = false;
                }
                else if (!ToggleMark && BatteryListData != null && ACListData == null  && powerStatus.ACLineStatus == 1) // 插电状态 02
                {
                    BatteryListData.Active = false;
                    ToggleMark = true;
                }
                else if (ToggleMark && ACListData != null && BatteryListData == null  && powerStatus.ACLineStatus == 0) //电池供电 01
                {
                    ACListData.Active = false;
                    ToggleMark = false;
                }
                
               
            }
        }
    }
}
