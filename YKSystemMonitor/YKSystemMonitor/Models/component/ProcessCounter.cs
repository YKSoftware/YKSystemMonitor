namespace YKSystemMonitor.Models
{
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// プロセスのパフォーマンスカウンタに関する操作をおこなうクラスを表します。
    /// </summary>
    internal class ProcessCounter
    {
        #region コンストラクタ

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="instanceName">Process カテゴリにあるインスタンス名を指定します。</param>
        public ProcessCounter(string instanceName)
        {
            var category = new PerformanceCounterCategory("Process");
            if (category.GetInstanceNames().Contains(instanceName ?? ""))
            {
                this._counters = category.GetCounters(instanceName);
            }
        }

        #endregion コンストラクタ

        #region パフォーマンスカウンタ

        /// <summary>
        /// すべてのパフォーマンスカウンタ
        /// </summary>
        private PerformanceCounter[] _counters = new PerformanceCounter[0];

        private PerformanceCounter _workingSetCounter;
        /// <summary>
        /// Working Set のためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter WorkingSetCounter { get { return this._workingSetCounter ?? (this._workingSetCounter = this._counters.FirstOrDefault(x => x.CounterName == "Working Set")); } }

        private PerformanceCounter _privateWorkingSetCounter;
        /// <summary>
        /// Working Set - Private のためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter PrivateWorkingSetCounter { get { return this._privateWorkingSetCounter ?? (this._privateWorkingSetCounter = this._counters.FirstOrDefault(x => x.CounterName == "Working Set - Private")); } }

        private PerformanceCounter _virtualBytesCounter;
        /// <summary>
        /// Virtual Bytes のためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter VirtualBytesCounter { get { return this._virtualBytesCounter ?? (this._virtualBytesCounter = this._counters.FirstOrDefault(x => x.CounterName == "Virtual Bytes")); } }

        private PerformanceCounter _threadCounter;
        /// <summary>
        /// Thread Count のためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter ThreadCounter { get { return this._threadCounter ?? (this._threadCounter = this._counters.FirstOrDefault(x => x.CounterName == "Thread Count")); } }

        private PerformanceCounter _pageFaultsCounter;
        /// <summary>
        /// Page Faults/sec のためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter PageFaultsCounter { get { return this._pageFaultsCounter ?? (this._pageFaultsCounter = this._counters.FirstOrDefault(x => x.CounterName == "Page Faults/sec")); } }

        #endregion パフォーマンスカウンタ

        #region 公開メソッド

        /// <summary>
        /// データを更新します。
        /// </summary>
        public void UpdateData()
        {
            try
            {
                this.WorkingSet = this.WorkingSetCounter != null ? this.WorkingSetCounter.NextValue() / 1024.0 / 1024.0 : 0.0;
                this.PrivateWorkingSet = this.PrivateWorkingSetCounter != null ? this.PrivateWorkingSetCounter.NextValue() / 1024.0 / 1024.0 : 0.0;
                this.VirtualBytes = this.VirtualBytesCounter != null ? this.VirtualBytesCounter.NextValue() / 1024.0 / 1024.0 : 0.0;
                this.ThreadCount = this.ThreadCounter != null ? (int)this.ThreadCounter.NextValue() : 0;
                this.PageFaults = this.PageFaultsCounter != null ? (int)this.PageFaultsCounter.NextValue() : 0;
            }
            catch
            {
                this._counters = new PerformanceCounter[0];
            }
        }

        #endregion 公開メソッド

        #region 公開プロパティ

        private double _workingSet;
        /// <summary>
        /// 現在のワーキングセット [MB] を取得します。
        /// </summary>
        public double WorkingSet
        {
            get { return this._workingSet; }
            private set { this._workingSet = value; }
        }

        private double _privateWorkingSet;
        /// <summary>
        /// 現在のプライベートワーキングセット [MB] を取得します。
        /// </summary>
        public double PrivateWorkingSet
        {
            get { return this._privateWorkingSet; }
            private set { this._privateWorkingSet = value; }
        }

        private double _virtualBytes;
        /// <summary>
        /// 現在の仮想メモリ使用量 [MB] を取得します。
        /// </summary>
        public double VirtualBytes
        {
            get { return this._virtualBytes; }
            private set { this._virtualBytes = value; }
        }

        private int _threadCount;
        /// <summary>
        /// 現在のスレッド数を取得します。
        /// </summary>
        public int ThreadCount
        {
            get { return this._threadCount; }
            private set { this._threadCount = value; }
        }

        private int _pageFaults;
        /// <summary>
        /// 現在のページフォルト発生率を取得します。
        /// </summary>
        public int PageFaults
        {
            get { return this._pageFaults; }
            private set { this._pageFaults = value; }
        }

        #endregion 公開プロパティ
    }
}
