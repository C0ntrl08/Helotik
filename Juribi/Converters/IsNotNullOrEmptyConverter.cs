using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Juribi.Converters
{
    /// <summary>Returns <c>true</c> when the bound string has a non-whitespace value.</summary>
    public sealed class IsNotNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => !string.IsNullOrWhiteSpace(value as string);

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
