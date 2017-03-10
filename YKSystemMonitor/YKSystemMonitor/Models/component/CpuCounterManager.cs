namespace YKSystemMonitor.Models
{
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// CPU に関する情報を管理するクラスを表します。
    /// </summary>
    internal class CpuCounterManager
    {
        #region コンストラクタ

        /// <summary>
        /// 静的なコンストラクタ
        /// </summary>
        static CpuCounterManager()
        {
            var counterCategory = new PerformanceCounterCategory("Processor");
            _cpuCores = counterCategory.GetInstanceNames().Where(x => x != "_Total").Count();

            _totalIdleTimeCounter = _cpuCounterCategory.GetCounters("_Total").First(x => x.CounterName == "% Idle Time");
        }

        #endregion コンストラクタ

        #region 公開メソッド

        /// <summary>
        /// CPU 全体のアイドル時間の割合を取得します。
        /// </summary>
        /// <returns>CPU 全体のアイドル時間の割合</returns>
        public float GetTotalIdleTime()
        {
            return this.TotalIdleTimeCounter.NextValue();
        }

        #endregion 公開メソッド

        #region 公開プロパティ

        static readonly int _cpuCores;
        /// <summary>
        /// CPU のコア数を取得します。
        /// </summary>
        public int CpuCores { get { return _cpuCores; } }

        #endregion 公開プロパティ

        #region private プロパティ

        private static readonly PerformanceCounterCategory _cpuCounterCategory = new PerformanceCounterCategory("Processor Information");
        /// <summary>
        /// Processor Information に関するパフォーマンスカテゴリを取得します。
        /// </summary>
        private PerformanceCounterCategory CpuCounterCategory { get { return _cpuCounterCategory; } }

        private static readonly PerformanceCounter _totalIdleTimeCounter;
        /// <summary>
        /// アイドル時間の割合を示す値を取得するためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter TotalIdleTimeCounter { get { return _totalIdleTimeCounter; } }

        #endregion private プロパティ
    }
}
