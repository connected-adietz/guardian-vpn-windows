// <copyright file="Devices.cs" company="Mozilla">
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, you can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.Windows.Data;

namespace FirefoxPrivateNetwork.FxA
{
    /// <summary>
    /// Determines if there is at least one device.
    /// </summary>
    internal class RangeConverter : IValueConverter
    {
        /// <summary>
        /// Converters the Boolean value to Visibility inversely
        /// </summary>
        /// <param name="value">The Boolean value</param>
        /// <param name="targetType">The target value</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture of the value</param>
        /// <returns>Returns the a Visibility Type Value</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int numDevices = (int)value;

            if (numDevices > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converters the Visibility value to Boolean inversely
        /// </summary>
        /// <param name="value">The Boolean value</param>
        /// <param name="targetType">The target value</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture of the value</param>
        /// <returns>Returns the a Visibility Type Value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
