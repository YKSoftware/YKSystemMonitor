namespace YKSystemMonitor.Models
{
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// メモリに関する情報を管理するクラスを表します。
    /// </summary>
    internal class MemoryCounterManager
    {
        private static readonly PerformanceCounterCategory _memoryCounterCategory = new PerformanceCounterCategory("Memory");
        /// <summary>
        /// Memory に関するパフォーマンスカウンタカテゴリを取得します。
        /// </summary>
        private PerformanceCounterCategory MemoryCounterCategory { get { return _memoryCounterCategory; } }

        private PerformanceCounter _pageFaultCounter;
        /// <summary>
        /// Page Faults/sec のためのパフォーマンスカウンタを取得します。
        /// </summary>
        private PerformanceCounter PageFaultCounter
        {
            get
            {
                return this._pageFaultCounter ?? (this._pageFaultCounter = this.MemoryCounterCategory.GetCounters().FirstOrDefault(x => x.CounterName == "Page Faults/sec"));
            }
        }

        /// <summary>
        /// メモリ全体のページフォルトの発生頻度を取得します。
        /// </summary>
        /// <returns>1 秒間あたりのページフォルトの発生頻度</returns>
        public float GetPageFaults()
        {
            return this.PageFaultCounter.NextValue();
        }
    }
}
