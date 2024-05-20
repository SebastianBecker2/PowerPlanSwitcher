using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PowerPlanSwitcher // 确保这里是您的命名空间
{
    public class HotKey
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_F1 = 0x70;
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private static Guid[] powerPlanGuids;
        private static int currentPlanIndex = 0;

        // 初始化电源计划GUID数组
        public static void HotKeyGuid()
        {
                List<Guid> powerPlanGuidsList = new List<Guid>();

                // 遍历电源计划
                foreach (var (guid, name) in PowerManager.GetPowerSchemes())
                {
                    var setting = PowerSchemeSettings.GetSetting(guid);
                    if (setting is not null && !setting.Visible)
                    {
                        continue; // 跳过不可见的电源计划设置
                    }

                    // 将guid添加到列表中
                    powerPlanGuidsList.Add(guid);
                }

                // 将列表转换为数组
                powerPlanGuids = null;
                powerPlanGuids = powerPlanGuidsList.ToArray();
        }

        public static void StartListening()
        {
            _hookID = SetHook(_proc);
        }

        public static void StopListening()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == (int)VK_F1 && (Control.ModifierKeys == Keys.Control))
                {
                    SwitchPowerPlan();
                    return (IntPtr)1; // 阻止事件继续传递
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void SwitchPowerPlan()
        {
            currentPlanIndex = (currentPlanIndex + 1) % powerPlanGuids.Length;
            Guid planGuid = powerPlanGuids[currentPlanIndex];

            // 假设我们有一个PowerManager类来设置电源计划
            // 这里需要您自己实现SetActivePowerScheme方法
            PowerManager.SetActivePowerScheme(planGuid);
        }
    }
}
