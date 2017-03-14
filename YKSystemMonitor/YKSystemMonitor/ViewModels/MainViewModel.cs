namespace YKSystemMonitor.ViewModels
{
    using System;
    using System.Collections.Generic;
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
            this._pcManager.Tick += OnTick_PcManager;

            this.CurrentProcessName = "firefox";
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
            if (!this.IsEnabled) return;
            if (this.PcManager.CurrentProcessCounter == null) return;

            #region プロセス名リスト更新

            foreach (var name in this.PcManager.AddedProcessNames)
            {
                this.AddedProcessNameList.Add(new KeyValuePair<int, string>(this.Count, name));
            }
            foreach (var name in this.PcManager.RemovedProcessNames)
            {
                this.RemovedProcessNameList.Add(new KeyValuePair<int, string>(this.Count, name));
            }

            #endregion プロセス名リスト更新

            #region グラフデータ更新

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
            this.VirtualBytes.YAxisData.Add(this.PcManager.CurrentProcessCounter.VirtualBytes);
            this.PageFaults.YAxisData.Add(this.PcManager.CurrentProcessCounter.PageFaults);
            this.TotalPageFaults.YAxisData.Add(this.PcManager.PageFaults);

            // 縦軸自動設定
            if (this.IsAutoScalingEnabled)
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

            #endregion グラフデータ更新

            this.Count++;
        }

        #endregion イベントハンドラ

        #region private メソッド

        /// <summary>
        /// グラフデータをクリアします。
        /// </summary>
        private void ClearData()
        {
            this.Count = 0;
            this.XAxisData.Clear();
            this.WorkingSet.YAxisData.Clear();
            this.PrivateWorkingSet.YAxisData.Clear();
            this.PageFaults.YAxisData.Clear();
            this.TotalPageFaults.YAxisData.Clear();

            this.AddedProcessNameList.Clear();
            this.RemovedProcessNameList.Clear();
        }

        #endregion private メソッド

        #region 公開プロパティ

        /// <summary>
        /// アプリケーションのタイトルを取得します。
        /// </summary>
        public string Title { get { return ProductInfo.Title + " Ver." + ProductInfo.VersionString; } }

        private bool _isEnabled = true;
        /// <summary>
        /// 監視が有効かどうかを取得または設定します。
        /// </summary>
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                if (SetProperty(ref this._isEnabled, value))
                {
                    if (this._isEnabled)
                    {
                        ClearData();
                    }
                }
            }
        }

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

        private bool _isAutoScalingEnabled;
        /// <summary>
        /// グラフ縦軸レンジを自動調整するかどうかを取得または設定します。
        /// </summary>
        public bool IsAutoScalingEnabled
        {
            get { return this._isAutoScalingEnabled; }
            set { SetProperty(ref this._isAutoScalingEnabled, value); }
        }

        private string _selectedProcessName;
        /// <summary>
        /// 選択しているプロセス名を取得または設定します。
        /// </summary>
        public string SelectedProcessName
        {
            get { return this._selectedProcessName; }
            set
            {
                if (SetProperty(ref this._selectedProcessName, value))
                {
                    if (!string.IsNullOrEmpty(this._selectedProcessName))
                    {
                        this.CurrentProcessName = this._selectedProcessName;
                    }
                }
            }
        }

        private string _currentProcessName;
        /// <summary>
        /// 選択中のプロセス名を取得または設定します。
        /// </summary>
        public string CurrentProcessName
        {
            get { return this._currentProcessName; }
            set
            {
                if (SetProperty(ref this._currentProcessName, value))
                {
                    this.SelectedProcessName = this._currentProcessName;
                    this.PcManager.CurrentProcessName = this._currentProcessName;

                    ClearData();
                }
            }
        }

        private CounterManager _pcManager;
        /// <summary>
        /// 各種情報を取得します。
        /// </summary>
        public CounterManager PcManager { get { return this._pcManager; } }

        private ObservableCollection<KeyValuePair<int, string>> _addedProcessNameList = new ObservableCollection<KeyValuePair<int, string>>();
        /// <summary>
        /// 追加されたプロセス名のリストを取得します。
        /// </summary>
        public ObservableCollection<KeyValuePair<int, string>> AddedProcessNameList { get { return this._addedProcessNameList; } }

        private ObservableCollection<KeyValuePair<int, string>> _removedProcessNameList = new ObservableCollection<KeyValuePair<int, string>>();
        /// <summary>
        /// 削除されたプロセス名のリストを取得します。
        /// </summary>
        public ObservableCollection<KeyValuePair<int, string>> RemovedProcessNameList { get { return this._removedProcessNameList; } }

        #region グラフデータ

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

        private GraphItem _virtualBytes = new GraphItem();
        /// <summary>
        /// 選択しているプロセスの仮想メモリ使用量のためのグラフ情報を取得します。
        /// </summary>
        public GraphItem VirtualBytes
        {
            get { return this._virtualBytes; }
            private set { SetProperty(ref this._virtualBytes, value); }
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

        #endregion グラフデータ

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
