namespace YKSystemMonitor.Models
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// プロセスに関する情報を管理するクラスを表します。
    /// </summary>
    internal class ProcessCounterManager
    {
        private static readonly PerformanceCounterCategory _processCounterCategory = new PerformanceCounterCategory("Process");
        /// <summary>
        /// Process に関するパフォーマンスカウンタカテゴリを取得します。
        /// </summary>
        private PerformanceCounterCategory ProcessCounterCategory { get { return _processCounterCategory; } }

        /// <summary>
        /// Process 名の一覧を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetProcessNames()
        {
            return this.ProcessCounterCategory.GetInstanceNames().Except(new string[] { "_Total" }).OrderBy(x => x);
        }

        private static readonly ProcessCounter _totalCounter = new ProcessCounter("_Total");
        /// <summary>
        /// プロセス全体のカウンタを取得します。
        /// </summary>
        public ProcessCounter TotalCounter { get { return _totalCounter; } }

        private ProcessCounter _currentCounter;
        /// <summary>
        /// 選択中のプロセスのカウンタを取得します。
        /// </summary>
        public ProcessCounter CurrentCounter
        {
            get { return this._currentCounter; }
            private set { this._currentCounter = value; }
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
                if (this._currentProcessName != value)
                {
                    this._currentProcessName = value;
                    this.CurrentCounter = new ProcessCounter(this._currentProcessName);
                }
            }
        }
    }
}
