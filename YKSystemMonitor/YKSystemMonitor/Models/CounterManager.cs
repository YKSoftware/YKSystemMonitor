namespace YKSystemMonitor.Models
{
    using System;
    using System.Linq;
    using System.Windows.Threading;
    using YKToolkit.Bindings;

    /// <summary>
    /// 各種情報を管理するクラスを表します。
    /// </summary>
    internal class CounterManager : NotificationObject
    {
        private static readonly string[] _defaultNames = new string[]
        {
            "firefox",
            "devenv",
        };

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public CounterManager()
        {
            this.ProcessNames = this._processManager.GetProcessNames().ToArray();
            foreach (var name in _defaultNames)
            {
                if (this.ProcessNames.Contains(name))
                {
                    this.CurrentProcessName = name;
                    break;
                }
            }

            this._updateTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            this._updateTimer.Tick += OnTick_UpdateTimer;
            this._updateTimer.Start();
        }

        /// <summary>
        /// 更新用タイマーイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnTick_UpdateTimer(object sender, EventArgs e)
        {
            #region CPU
            this._cpuTotalUseRate = 100.0 - this._cpuManager.GetTotalIdleTime();
            if (this._cpuTotalUseRate < 0.0) this._cpuTotalUseRate = 0.0;
            if (this._cpuTotalUseRate > 100.0) this._cpuTotalUseRate = 100.0;
            #endregion CPU

            #region メモリ
            this._pageFaults = (int)this._memoryManager.GetPageFaults();
            #endregion メモリ

            #region プロセス
            var newProcessNames = this._processManager.GetProcessNames().ToArray();

            var addedProcessNames = newProcessNames.Except(this.ProcessNames).ToArray();
            this._addedProcessNames = addedProcessNames;
            var removedProcessNames = this.ProcessNames.Except(newProcessNames).ToArray();
            this._removedProcessNames = removedProcessNames;

            this._processNames = newProcessNames;
            if (this.CurrentProcessCounter != null)
            {
                this.CurrentProcessCounter.UpdateData();
            }
            #endregion プロセス

            RaisePropertyChanged("");
            RaiseTick();
        }

        #region イベント

        /// <summary>
        /// 更新周期経過時に発生します。
        /// </summary>
        public event EventHandler<EventArgs> Tick;

        /// <summary>
        /// Tick イベントを発生します。
        /// </summary>
        private void RaiseTick()
        {
            var h = this.Tick;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// 更新周期経過時に発生します。
        /// </summary>
        public event EventHandler<EventArgs> CurrentProcessNameChanged;

        /// <summary>
        /// CurrentProcessNameChanged イベントを発生します。
        /// </summary>
        private void RaiseCurrentProcessNameChanged()
        {
            var h = this.CurrentProcessNameChanged;
            if (h != null) h(this, EventArgs.Empty);
        }

        #endregion イベント

        #region 公開プロパティ

        /// <summary>
        /// CPU のコア数を取得します。
        /// </summary>
        public int CpuCores { get { return this._cpuManager.CpuCores; } }

        private double _cpuTotalUseRate;
        /// <summary>
        /// CPU 使用率を取得します。
        /// </summary>
        public double CpuTotalUseRate
        {
            get { return this._cpuTotalUseRate; }
            private set { SetProperty(ref this._cpuTotalUseRate, value); }
        }

        private int _pageFaults;
        /// <summary>
        /// メモリのページフォルト発生頻度を取得します。
        /// </summary>
        public int PageFaults
        {
            get { return this._pageFaults; }
            private set { SetProperty(ref this._pageFaults, value); }
        }

        private string[] _processNames = new string[0];
        /// <summary>
        /// 実行中のプロセス名一覧を取得します。
        /// </summary>
        public string[] ProcessNames
        {
            get { return this._processNames; }
            private set { SetProperty(ref this._processNames, value); }
        }

        private string[] _addedProcessNames = new string[0];
        /// <summary>
        /// 追加されたプロセス名一覧を取得します。
        /// </summary>
        public string[] AddedProcessNames
        {
            get { return this._addedProcessNames; }
            private set { SetProperty(ref this._addedProcessNames, value); }
        }

        private string[] _removedProcessNames = new string[0];
        /// <summary>
        /// 削除されたプロセス名一覧を取得します。
        /// </summary>
        public string[] RemovedProcessNames
        {
            get { return this._removedProcessNames; }
            private set { SetProperty(ref this._removedProcessNames, value); }
        }

        private string _currentProcessName;
        /// <summary>
        /// 監視中のプロセス名を取得または設定します。
        /// </summary>
        public string CurrentProcessName
        {
            get { return this._currentProcessName; }
            set
            {
                if (SetProperty(ref this._currentProcessName, value))
                {
                    this._processManager.CurrentProcessName = this._currentProcessName;
                    RaisePropertyChanged("CurrentProcessCounter");
                    RaiseCurrentProcessNameChanged();
                }
            }
        }

        /// <summary>
        /// 監視中のプロセスカウンタを取得します。
        /// </summary>
        public ProcessCounter CurrentProcessCounter { get { return this._processManager.CurrentCounter; } }

        #endregion 公開プロパティ

        #region private フィールド

        /// <summary>
        /// 更新用タイマー
        /// </summary>
        private DispatcherTimer _updateTimer;

        /// <summary>
        /// CPU 関連情報
        /// </summary>
        private CpuCounterManager _cpuManager = new CpuCounterManager();

        /// <summary>
        /// メモリ関連情報
        /// </summary>
        private MemoryCounterManager _memoryManager = new MemoryCounterManager();

        /// <summary>
        /// メモリ関連情報
        /// </summary>
        private ProcessCounterManager _processManager = new ProcessCounterManager();

        #endregion private フィールド
    }
}
