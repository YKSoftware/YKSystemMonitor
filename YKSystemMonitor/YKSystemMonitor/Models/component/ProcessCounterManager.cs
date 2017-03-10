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
        static private readonly PerformanceCounterCategory _processCounterCategory = new PerformanceCounterCategory("Process");
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
    }
}
