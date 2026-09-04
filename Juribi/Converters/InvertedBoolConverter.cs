using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Juribi.Converters
{
    /// <summary>Returns the logical negation of a boolean value.</summary>
    public sealed class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && !b;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && !b;
    }
}
