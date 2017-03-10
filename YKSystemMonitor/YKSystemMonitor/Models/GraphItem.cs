namespace YKSystemMonitor.Models
{
    using System.Collections.ObjectModel;
    using YKToolkit.Bindings;

    /// <summary>
    /// グラフデータを表します。
    /// </summary>
    internal class GraphItem : NotificationObject
    {
        //private ObservableCollection<double> _xAxisData = new ObservableCollection<double>();
        ///// <summary>
        ///// 横軸データを取得します。
        ///// </summary>
        //public ObservableCollection<double> XAxisData { get { return this._xAxisData; } }

        private ObservableCollection<double> _yAxisData = new ObservableCollection<double>();
        /// <summary>
        /// 縦軸データを取得します。
        /// </summary>
        public ObservableCollection<double> YAxisData { get { return this._yAxisData; } }
    }
}
