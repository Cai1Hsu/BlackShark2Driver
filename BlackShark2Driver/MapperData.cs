using Newtonsoft.Json;
using System;

namespace XOutput.Devices.Mapper
{
    /// <summary>
    /// Contains mapping data to Xinput conversion.
    /// </summary>
    public class MapperData
    {
        /// <summary>
        /// From data device
        /// </summary>
        public string InputDevice { get; set; }
        /// <summary>
        /// From data type
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// Data source
        /// </summary>
        [JsonIgnore]
        public InputSource Source
        {
            get => source;
            set
            {
<<<<<<< HEAD
                InputSource newValue = value ?? DisabledInputSource.Instance;
=======
                var newValue = value ?? DisabledInputSource.Instance;
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
                if (newValue != source)
                {
                    source = newValue;
                    InputType = source.Offset.ToString();
                    InputDevice = source.InputDevice?.UniqueId;
                }
            }
        }
        /// <summary>
        /// Minimum value
        /// </summary>
        public double MinValue { get; set; }
        /// <summary>
        /// Maximum value
        /// </summary>
        public double MaxValue { get; set; }
        /// <summary>
        /// Deadzone
        /// </summary>
        public double Deadzone { get; set; }

<<<<<<< HEAD
        private InputSource source;
=======
        InputSource source;
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09

        public MapperData()
        {
            InputType = "0";
            source = DisabledInputSource.Instance;
            MinValue = 0;
            MaxValue = 0;
            Deadzone = 0;
        }

        /// <summary>
        /// Gets the value based on minimum and maximum values.
        /// </summary>
        /// <param name="value">Measured data to convert</param>
        /// <returns>Mapped value</returns>
        public double GetValue(double value)
        {
            double range = MaxValue - MinValue;
            double mappedValue;
            if (Math.Abs(range) < 0.0001)
            {
                mappedValue = MinValue;
            }
            else
            {
<<<<<<< HEAD
                double readvalue = value;
=======
                var readvalue = value;
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
                if (Math.Abs(value - 0.5) < Deadzone)
                {
                    readvalue = 0.5;
                }

                mappedValue = (readvalue - MinValue) / range;
                if (mappedValue < 0)
                {
                    mappedValue = 0;
                }
                else if (mappedValue > 1)
                {
                    mappedValue = 1;
                }
            }
            return mappedValue;
        }
<<<<<<< HEAD
        public void SetSourceWithoutSaving(InputSource value)
        {
            InputSource newValue = value ?? DisabledInputSource.Instance;
=======
        public void SetSourceWithoutSaving(InputSource value) 
        {
            var newValue = value ?? DisabledInputSource.Instance;
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
            if (newValue != source)
            {
                source = newValue;
            }
        }
    }
}
