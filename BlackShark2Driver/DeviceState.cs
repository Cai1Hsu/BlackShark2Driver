﻿using System.Collections.Generic;
using System.Linq;

namespace XOutput.Devices
{
    /// <summary>
    /// Holds a current state of the device.
    /// </summary>
    public class DeviceState
    {
        /// <summary>
        /// Gets the current values.
        /// </summary>
        public IEnumerable<InputSource> Values => values;
        /// <summary>
        /// Gets the current DPad values.
        /// </summary>
        public IEnumerable<DPadDirection> DPads => dPads;
        protected IEnumerable<InputSource> values;
        protected DPadDirection[] dPads;
        /// <summary>
        /// Created once not to create memory waste.
        /// </summary>
        protected readonly List<InputSource> changedSources;
        protected readonly List<int> changedDpad;
        protected readonly List<int> allDpads;

        public DeviceState(IEnumerable<InputSource> types, int dPadCount)
        {
            values = types.ToArray();
            changedSources = new List<InputSource>(types.Count());
            dPads = new DPadDirection[dPadCount];
            allDpads = Enumerable.Range(0, dPads.Length).ToList();
            changedDpad = new List<int>();
        }

        /// <summary>
        /// Sets new DPad values.
        /// </summary>
        /// <param name="newDPads">new values</param>
        /// <returns>changed DPad indices</returns>
        public bool SetDPad(int i, DPadDirection newValue)
        {
            var oldValue = dPads[i];
            if (newValue != oldValue)
            {
                dPads[i] = newValue;
                changedDpad.Add(i);
                return true;
            }
            return false;
        }

        public void ResetChanges()
        {
            changedSources.Clear();
            changedDpad.Clear();
        }

        public void MarkChanged(InputSource source)
        {
            changedSources.Add(source);
        }

        public IEnumerable<InputSource> GetChanges(bool force = false)
        {
            return force ? values : changedSources;
        }

        public IEnumerable<int> GetChangedDpads(bool force = false)
        {
            return force ? allDpads : changedDpad;
        }
    }
}
