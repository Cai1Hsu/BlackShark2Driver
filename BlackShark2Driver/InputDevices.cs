using System.Collections.Generic;

namespace XOutput.Devices.Input
{
    public class InputDevices
    {

<<<<<<< HEAD
        private static readonly InputDevices instance = new InputDevices();
=======
        private static InputDevices instance = new InputDevices();
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
        /// <summary>
        /// Gets the singleton instance of the class.
        /// </summary>
        public static InputDevices Instance => instance;

        private readonly List<IInputDevice> inputDevices = new List<IInputDevice>();

        protected InputDevices()
        {

        }

        public void Add(IInputDevice inputDevice)
        {
            inputDevices.Add(inputDevice);
            Controllers.Instance.Update(inputDevices);
<<<<<<< HEAD
        }

=======
        }

        public void Remove(IInputDevice inputDevice)
        {
            inputDevices.Remove(inputDevice);
            Controllers.Instance.Update(inputDevices);
        }

>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
        public IEnumerable<IInputDevice> GetDevices()
        {
            return inputDevices.ToArray();
        }
    }
}
