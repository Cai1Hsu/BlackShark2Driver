using Newtonsoft.Json;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using XOutput.Devices;
using XOutput.Devices.Input.DirectInput;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput.Vigem;

namespace BlackShark2Driver
{
    internal class Program
    {
        private static DirectInputDevices directInputDevices = new DirectInputDevices();
		
		private static string mapper = "{\"Name\":\"BlackShark2\",\"Id\":\"e86fa9f2-5834-4e9b-b391-42df5cfa917d\",\"ForceFeedbackDevice\":\"####\",\"Mappings\":{\"A\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"46\",\"MinValue\":0.0,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"B\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"44\",\"MinValue\":0.0,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"X\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"47\",\"MinValue\":0.0,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"Y\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"45\",\"MinValue\":0.0,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"L1\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"50\",\"MinValue\":0.0,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"R1\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"48\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"L3\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"48\",\"MinValue\":0.0,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"R3\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"Start\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"Back\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"Home\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"LX\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"16\",\"MinValue\":1.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"LY\":{\"Mappers\":[{\"InputDevice\":\"####\",\"InputType\":\"20\",\"MinValue\":0.00,\"MaxValue\":1.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"RX\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.5,\"MaxValue\":0.5,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"RY\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.5,\"MaxValue\":0.5,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"L2\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"R2\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"UP\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"DOWN\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"LEFT\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0},\"RIGHT\":{\"Mappers\":[{\"InputDevice\":null,\"InputType\":\"0\",\"MinValue\":0.0,\"MaxValue\":0.0,\"Deadzone\":0.0}],\"CenterPoint\":0.0}}}";
		
        private static void Main(string[] args)
        {
            Console.WriteLine("\n\n\n The Controller will be displaying here.\n\n\n\n黑鲨二代手柄的非官方驱动Unofficial driver for BlackShark2 Controller(Left)");
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
            }

            Console.WriteLine("Devices list:");
            DirectInput directinput = new DirectInput();

            List<DeviceInstance> inputdevices = new List<DeviceInstance>();
            int index = 0;
            IList<DeviceInstance> devices = directinput.GetDevices();
            foreach (DeviceInstance i in devices)
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
            InputMapper inputMapper = JsonConvert.DeserializeObject<InputMapper>(mapper.Replace("####", controller.InstanceGuid.ToString()));

            GameController emulatedController = new GameController(inputMapper);
            Controllers.Instance.Add(emulatedController);
            Console.WriteLine("[...] Starting");
			
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                emulatedController.Stop();
                Console.CursorVisible = true;
                e.Cancel = true;
            };

            emulatedController.Start();
        }
    }
}
