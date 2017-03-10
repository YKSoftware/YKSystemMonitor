namespace YKSystemMonitor.Models
{
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// CPU に関する情報を管理するクラスを表します。
    /// </summary>
    internal class CpuCounterManager
    {
        /// <summary>
        /// 静的なコンストラクタ
        /// </summary>
        static CpuCounterManager()
        {
            var counterCategory = new PerformanceCounterCategory("Processor");
            _cpuCores = counterCategory.GetInstanceNames().Where(x => x != "_Total").Count();
        }

        static readonly int _cpuCores;
        /// <summary>
        /// CPU のコア数を取得します。
        /// </summary>
        public int CpuCores { get { return _cpuCores; } }

        static private readonly PerformanceCounterCategory _cpuCounterCategory = new PerformanceCounterCategory("Processor Information");
        /// <summary>
        /// Processor Information に関するパフォーマンスカテゴリを取得します。
        /// </summary>
        private PerformanceCounterCategory CpuCounterCategory { get { return _cpuCounterCategory; } }
    }
}
