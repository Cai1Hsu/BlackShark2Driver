using System;
using System.Threading;
using XOutput.Devices.Input;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Devices.XInput.Vigem;


namespace XOutput.Devices
{
    /// <summary>
    /// GameController is a container for input devices, mappers and output devices.
    /// </summary>
    public sealed class GameController : IDisposable
    {
        /// <summary>
        /// Gets the output device.
        /// </summary>
        public XOutputDevice XInput => xInput;
        /// <summary>
        /// Gets the mapping of the input device.
        /// </summary>
        public InputMapper Mapper => mapper;
        /// <summary>
        /// Gets the name of the input device.
        /// </summary>
        public string DisplayName => mapper.Name;
        /// <summary>
        /// Gets the number of the controller.
        /// </summary>
        public int ControllerCount => controllerCount;
        /// <summary>
        /// Gets if any XInput emulation is installed.
        /// </summary>
        public bool HasXOutputInstalled => xOutputInterface != null;
        /// <summary>
        /// Gets if force feedback is supported.
        /// </summary>
        public bool ForceFeedbackSupported => xOutputInterface is VigemDevice;
        /// <summary>
        /// Gets the force feedback device.
        /// </summary>
        public IInputDevice ForceFeedbackDevice { get; set; }

        // private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(GameController));

        private readonly InputMapper mapper;
        private readonly XOutputDevice xInput;
        private readonly IXOutputInterface xOutputInterface;
        // private Thread thread;
        private bool running;
        private int controllerCount = 0;

        public GameController(InputMapper mapper)
        {
            this.mapper = mapper;
            
            xOutputInterface = CreateXOutput();
            xInput = new XOutputDevice(mapper);
            running = false;
        }

        private IXOutputInterface CreateXOutput()
        {
            if (VigemDevice.IsAvailable())
            {
                Console.WriteLine("ViGEm devices are used.");
                return new VigemDevice();
            }
            else
            {
                Console.WriteLine("[???] ViGEm can not be used.");
                return null;
            }
        }

        ~GameController()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes all used resources
        /// </summary>
        public void Dispose()
        {
            Stop();
            xInput?.Dispose();
            xOutputInterface?.Dispose();
        }

        /// <summary>
        /// Starts the emulation of the device
        /// </summary>
        public int Start()
        {
            if (!HasXOutputInstalled)
            {
                return 0;
            }
            controllerCount = Controllers.Instance.GetId();
            
            if (xOutputInterface.Unplug(controllerCount))
            {
                // Wait for unplugging
                Thread.Sleep(100);
            }
            if (xOutputInterface.Plugin(controllerCount))
            {
                running = true;
                Console.WriteLine($"Emulation started on {ToString()}.");
                Console.WriteLine("\n[!] Press Ctrl + C to exit.\n");
                ReadAndReportValues();
            }
            else
            {
                resetId();
            }
            return controllerCount;
        }

        /// <summary>
        /// Stops the emulation of the device
        /// </summary>
        public void Stop()
        {
            if (running)
            {
                XInput.InputChanged -= XInputInputChanged;
                xOutputInterface?.Unplug(controllerCount);
                Console.WriteLine($"Emulation stopped on {ToString()}.");
                resetId();
                running = false;
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private void ReadAndReportValues()
        {
            try
            {
                XInput.InputChanged += XInputInputChanged;
                Console.CursorVisible = false;
                while (running)
                {
                    BlackShark2Driver.ControllerDrawer.Print(XInput.GetValues());
                    Thread.Sleep(100);
                }
            }
            catch (ThreadInterruptedException)
            {}
            finally
            {
                Stop();
            }
        }

        private void XInputInputChanged(object sender, DeviceInputChangedEventArgs e)
        {
            if (!xOutputInterface.Report(controllerCount, XInput.GetValues()))
            {
                Stop();
            }
        }

        private void ControllerFeedbackReceived(object sender, Nefarius.ViGEm.Client.Targets.Xbox360.Xbox360FeedbackReceivedEventArgs e)
        {
            ForceFeedbackDevice?.SetForceFeedback((double)e.LargeMotor / byte.MaxValue, (double)e.SmallMotor / byte.MaxValue);
        }

        private void resetId()
        {
            if (controllerCount != 0)
            {
                Controllers.Instance.DisposeId(controllerCount);
                controllerCount = 0;
            }
        }
    }
}
