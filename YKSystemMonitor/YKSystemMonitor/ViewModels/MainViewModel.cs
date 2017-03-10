namespace YKSystemMonitor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Threading;
    using YKSystemMonitor.Models;
    using YKToolkit.Bindings;

    /// <summary>
    /// MainView ウィンドウに対する ViewModel を表します。
    /// </summary>
    internal class MainViewModel : NotificationObject
    {
        #region コンストラクタ

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MainViewModel()
        {
            this._pcManager = new CounterManager();
            this._pcManager.CurrentProcessNameChanged += OnCurrentProcessNameChanged_PcManager;
            this._pcManager.Tick += OnTick_PcManager;
        }

        #endregion コンストラクタ

        #region イベントハンドラ

        /// <summary>
        /// Tick イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnTick_PcManager(object sender, EventArgs e)
        {
            if (this.PcManager.CurrentProcessCounter == null) return;

            this.XAxisData.Add(this.Count);

            // 横軸自動設定
            //if ()
            {
                if ((this.XAxisData.Count > (int)(this.Axes.XMax - this.Axes.XMin)) && (this.Count <= this.Axes.XMax))
                {
                    var xMin = this.Axes.XMin + 1;
                    var xMax = this.Axes.XMax + 1;
                    this.Axes.SetRangeX(xMin, xMax);
                }
            }

            var workingSet = this.PcManager.CurrentProcessCounter.WorkingSet;
            var privateWorkingSet = this.PcManager.CurrentProcessCounter.PrivateWorkingSet;

            this.WorkingSet.YAxisData.Add(workingSet);
            this.PrivateWorkingSet.YAxisData.Add(privateWorkingSet);
            this.PageFaults.YAxisData.Add(this.PcManager.CurrentProcessCounter.PageFaults);
            this.TotalPageFaults.YAxisData.Add(this.PcManager.PageFaults);

            // 縦軸自動設定
            //if ()
            {
                var average = (workingSet + privateWorkingSet) / 2.0;
                var midLine = (this.Axes.YMax + this.Axes.YMin) / 2.0;
                var maxLine = midLine + 3 * _tickUnit;
                var minLine = midLine - 3 * _tickUnit;
                if ((average < minLine) || (maxLine < average))
                {
                    var ticks = _ticks.Select(x => Math.Abs(x - average)).ToList();
                    var index = ticks.IndexOf(ticks.Min());
                    var middle = _ticks[index];
                    var max = middle + 5 * _tickUnit;
                    var min = middle - 5 * _tickUnit;
                    var step = _tickUnit;
                    this.Axes.SetRangeY(min, max, step);
                }
            }

            this.Count++;
        }

        /// <summary>
        /// CurrentProcessNameChanged イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnCurrentProcessNameChanged_PcManager(object sender, EventArgs e)
        {
            this.Count = 0;
            this.XAxisData.Clear();
            this.WorkingSet.YAxisData.Clear();
            this.PrivateWorkingSet.YAxisData.Clear();
            this.PageFaults.YAxisData.Clear();
            this.TotalPageFaults.YAxisData.Clear();
        }

        #endregion イベントハンドラ

        #region 公開プロパティ

        /// <summary>
        /// アプリケーションのタイトルを取得します。
        /// </summary>
        public string Title { get { return ProductInfo.Title + " Ver." + ProductInfo.VersionString; } }

        private AxisData _axes = new AxisData();
        /// <summary>
        /// 軸設定を取得または設定します。
        /// </summary>
        public AxisData Axes
        {
            get { return this._axes; }
            set { SetProperty(ref this._axes, value); }
        }

        private ObservableCollection<double> _xAxisData = new ObservableCollection<double>();
        /// <summary>
        /// 横軸データを取得します。
        /// </summary>
        public ObservableCollection<double> XAxisData { get { return this._xAxisData; } }

        private CounterManager _pcManager;
        /// <summary>
        /// 各種情報を取得します。
        /// </summary>
        public CounterManager PcManager { get { return this._pcManager; } }

        private GraphItem _workingSet = new GraphItem();
        /// <summary>
        /// 選択しているプロセスのワーキングセットのためのグラフ情報を取得します。
        /// </summary>
        public GraphItem WorkingSet
        {
            get { return this._workingSet; }
            private set { SetProperty(ref this._workingSet, value); }
        }

        private GraphItem _privateWorkingSet = new GraphItem();
        /// <summary>
        /// 選択しているプロセスのプライベートワーキングセットのためのグラフ情報を取得します。
        /// </summary>
        public GraphItem PrivateWorkingSet
        {
            get { return this._privateWorkingSet; }
            private set { SetProperty(ref this._privateWorkingSet, value); }
        }

        private GraphItem _pageFaults = new GraphItem();
        /// <summary>
        /// 選択しているプロセスのページフォルトのためのグラフ情報を取得します。
        /// </summary>
        public GraphItem PageFaults
        {
            get { return this._pageFaults; }
            private set { SetProperty(ref this._pageFaults, value); }
        }

        private GraphItem _totalPageFaults = new GraphItem();
        /// <summary>
        /// メモリのページフォルトのためのグラフ情報を取得します。
        /// </summary>
        public GraphItem TotalPageFaults
        {
            get { return this._totalPageFaults; }
            private set { SetProperty(ref this._totalPageFaults, value); }
        }

        #endregion 公開プロパティ

        #region private フィールド

        /// <summary>
        /// 横軸カウントを取得または設定します。
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// 縦軸目盛用単位
        /// </summary>
        private const double _tickUnit = 25.6;

        /// <summary>
        /// 縦軸目盛用スケール
        /// </summary>
        private static readonly double[] _ticks = Enumerable.Range(0, 40).Select(x => _tickUnit * x).ToArray();

        #endregion private フィールド
    }
}
