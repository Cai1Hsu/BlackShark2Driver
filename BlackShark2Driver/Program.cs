using System;
using System.Collections.Generic;
using XOutput.Devices;
using XOutput.Devices.Input.DirectInput;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput.Vigem;
using SharpDX.DirectInput;
using Newtonsoft.Json;

namespace BlackShark2Driver
{
    class Program
    {
        static DirectInputDevices directInputDevices = new DirectInputDevices();
        static void Main(string[] args)
        {
            Console.WriteLine("黑鲨二代手柄的非官方驱动Unofficial driver for BlackShark2 Controller(Left)");
            Console.WriteLine(@"Visit https://github.com/Cai1Hsu/BlackShark2Driver");
            Console.WriteLine("Author : Cai1Hsu (x1052819745@163.com)");
            Console.WriteLine("---------------------------------------");

            if (VigemDevice.IsAvailable())
            {
                Console.WriteLine("[OK] Vigem is installed.");
            }
            else
            {
                Console.WriteLine("\a[!]Vigem is NOT installed.");
                Console.WriteLine(@" Please visit https://github.com/ViGEm/ViGEmBus/releases/download/setup-v1.16.116/ViGEmBus_Setup_1.16.116.exe");
                return;
            }

            Console.WriteLine("Devices list:");
            DirectInput directinput = new DirectInput();

            List<DeviceInstance> inputdevices = new List<DeviceInstance>();
            int index = 0;
            var devices = directinput.GetDevices();
            foreach (var i in devices)
            {
                if (i.Usage == SharpDX.Multimedia.UsageId.GenericGamepad)
                {
                    Console.WriteLine($"\t({index++}) {i.Usage} : {i.InstanceName}");
                    inputdevices.Add(i);
                }
            }
            if (index == 0)
            {
                Console.WriteLine("\a[!] No device found.");
                Console.ReadKey(true);
                return;
            }
            int deviceNumber = -1;
            if (index == 1)
            {
                Console.WriteLine("[!] The only device was chosen by default.");
                deviceNumber = 0;
            }
            else
            {
            InputNumber:
                Console.Write($"[!] Choose your device number(0-{index - 1}): ");
                string s = Console.ReadLine();
                try
                {
                    int i = Convert.ToInt32(s);
                    deviceNumber = i;
                }
                catch (FormatException)
                {
                    Console.WriteLine("[!] Invalid number, please reinput.");
                    goto InputNumber;
                }
            }
            DeviceInstance controller = inputdevices[deviceNumber];
            directInputDevices.CreateDirectDevice(controller);

            Console.WriteLine($"Chosen controller : {controller.InstanceName} {controller.InstanceGuid}");
            InputMapper inputMapper = JsonConvert.DeserializeObject<InputMapper>(Properties.Resources.Mapper.Replace("####", controller.InstanceGuid.ToString()));

            GameController emulatedController = new GameController(inputMapper);
            Controllers.Instance.Add(emulatedController);
            Console.WriteLine("[...] Starting");

            System.Threading.Thread.Sleep(1000);
            emulatedController.Start(null);
            Console.ReadKey(true);
            emulatedController.Stop();
            Console.CursorVisible = true;
        }
    }
}
