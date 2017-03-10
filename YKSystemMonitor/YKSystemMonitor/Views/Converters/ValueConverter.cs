namespace YKSystemMonitor.Views.Converters
{
    using System.Windows.Data;

    /// <summary>
    /// 数値をスケーリングするためのコンバータを表します。
    /// </summary>
    internal class ValueConverter : IValueConverter
    {
        /// <summary>
        /// 指定されたパラメータでスケーリングします。
        /// </summary>
        /// <param name="value">元の数値を指定します。</param>
        /// <param name="targetType">対象の型情報を指定します。</param>
        /// <param name="parameter">スケーリングファクタを指定します。</param>
        /// <param name="culture">CultureInfo を指定します。</param>
        /// <returns></returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var scale = double.Parse(parameter as string);
            return (int)value / scale;
        }

        /// <summary>
        /// スケーリングされた値を戻します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
