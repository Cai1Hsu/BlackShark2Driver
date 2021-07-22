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
        public XOutputDevice XInput { get; }
        /// <summary>
        /// Gets the mapping of the input device.
        /// </summary>
        public InputMapper Mapper { get; }
        /// <summary>
        /// Gets the name of the input device.
        /// </summary>
        public string DisplayName => Mapper.Name;
        /// <summary>
        /// Gets the number of the controller.
        /// </summary>
        public int ControllerCount { get; private set; } = 0;
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

        private readonly IXOutputInterface xOutputInterface;
        private Thread thread;
        private bool running;

        public GameController(InputMapper mapper)
        {
            this.Mapper = mapper;

            xOutputInterface = CreateXOutput();
            XInput = new XOutputDevice(mapper);
            running = false;
        }

        private IXOutputInterface CreateXOutput()
        {
            if (VigemDevice.IsAvailable())
            {
                Console.WriteLine("[OK] ViGEm devices are working.");
                return new VigemDevice();
            }
            else
            {
                Console.WriteLine("\a[!] ViGEm can not be used.");
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
            XInput?.Dispose();
            xOutputInterface?.Dispose();
        }

        /// <summary>
        /// Starts the emulation of the device
        /// </summary>
        public int Start(Action onStop = null)
        {
            if (!HasXOutputInstalled)
            {
                return 0;
            }
            ControllerCount = Controllers.Instance.GetId();

            if (xOutputInterface.Unplug(ControllerCount))
            {
                // Wait for unplugging
                Thread.Sleep(100);
            }
            if (xOutputInterface.Plugin(ControllerCount))
            {
                thread = new Thread(() => ReadAndReportValues(onStop));
                running = true;
                thread.Name = $"Emulated controller {ControllerCount} output refresher";
                thread.IsBackground = true;
                thread.Start();
                Console.WriteLine($"[OK] Emulation started on {ToString()}.");
                Console.WriteLine("[!] Press any key to exit.\nStatus: ");
            }
            else
            {
                resetId();
            }
            return ControllerCount;
        }

        /// <summary>
        /// Stops the emulation of the device
        /// </summary>
        public void Stop()
        {
            if (running)
            {
                running = false;
                XInput.InputChanged -= XInputInputChanged;
                xOutputInterface?.Unplug(ControllerCount);
                Console.WriteLine($"Emulation stopped on {ToString()}.");
                resetId();
                thread?.Interrupt();
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private void ReadAndReportValues(Action onStop)
        {
            try
            {
                XInput.InputChanged += XInputInputChanged;
                Console.CursorVisible = false;
                while (running)
                {
                    var outputStatus = XInput.GetValues();
                    BlackShark2Driver.ControllerDrawer.Print(outputStatus);
                    Thread.Sleep(100);
                }
            }
            catch (ThreadInterruptedException)
            { }
            finally
            {
                onStop?.Invoke();
                Stop();
            }
            Stop();
        }

        private void XInputInputChanged(object sender, DeviceInputChangedEventArgs e)
        {
            if (!xOutputInterface.Report(ControllerCount, XInput.GetValues()))
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
            if (ControllerCount != 0)
            {
                Controllers.Instance.DisposeId(ControllerCount);
                ControllerCount = 0;
            }
        }
    }
}
