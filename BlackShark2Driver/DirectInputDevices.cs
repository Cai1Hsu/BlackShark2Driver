using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XOutput.Devices.Input.DirectInput
{
    /// <summary>
    /// General DirectInput device manager class.
    /// </summary>
    public sealed class DirectInputDevices : IDisposable
    {
        /// <summary>
        /// Id of the emulated SCP device
        /// </summary>
        private const string EmulatedSCPID = "028e045e-0000-0000-0000-504944564944";

<<<<<<< HEAD
        private readonly SharpDX.DirectInput.DirectInput directInput = new SharpDX.DirectInput.DirectInput();

=======
        private readonly SharpDX.DirectInput.DirectInput directInput = new SharpDX.DirectInput.DirectInput();
        
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
        ~DirectInputDevices()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes all resources.
        /// </summary>
        public void Dispose()
        {
            directInput.Dispose();
        }

        /// <summary>
        /// Gets the current available DirectInput devices.
        /// </summary>
        /// <param name="allDevices">No filter</param>
        /// <returns>List of devices</returns>
        public IEnumerable<DeviceInstance> GetInputDevices(bool allDevices)
        {
            if (allDevices)
            {
                return directInput.GetDevices().Where(di => di.Type != DeviceType.Keyboard && di.Type != DeviceType.Mouse);
            }
            else
            {
                return directInput.GetDevices().Where(di => di.Type == DeviceType.Joystick || di.Type == DeviceType.Gamepad || di.Type == DeviceType.FirstPerson);
            }
        }

        /// <summary>
        /// Creates a wrapper for the native <paramref name="deviceInstance"/>.
        /// </summary>
        /// <param name="deviceInstance">native instance</param>
        /// <returns>Wrapped instance</returns>
        public DirectDevice CreateDirectDevice(DeviceInstance deviceInstance)
        {
            try
            {
<<<<<<< HEAD
                Joystick joystick = new Joystick(directInput, deviceInstance.InstanceGuid);
=======
                var joystick = new Joystick(directInput, deviceInstance.InstanceGuid);
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
                if (joystick.Information.ProductGuid.ToString() == EmulatedSCPID || (joystick.Capabilities.AxeCount < 1 && joystick.Capabilities.ButtonCount < 1))
                {
                    joystick.Dispose();
                    return null;
                }
                joystick.Properties.BufferSize = 128;
<<<<<<< HEAD
                DirectDevice device = new DirectDevice(deviceInstance, joystick);
=======
                var device = new DirectDevice(deviceInstance, joystick);
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
                InputDevices.Instance.Add(device);
                return device;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[❌] Failed to create device " + deviceInstance.InstanceGuid + " " + deviceInstance.InstanceName + ex.ToString());
                return null;
            }
        }
    }
}
