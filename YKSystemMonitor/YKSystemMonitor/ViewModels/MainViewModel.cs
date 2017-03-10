namespace YKSystemMonitor.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Threading;
    using YKSystemMonitor.Models;
    using YKToolkit.Bindings;

    /// <summary>
    /// MainView ウィンドウに対する ViewModel を表します。
    /// </summary>
    internal class MainViewModel : NotificationObject
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MainViewModel()
        {
            this.ProcessNames = this._pcManager.GetProcessNames().ToArray();

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
            var newProcessNames = this._pcManager.GetProcessNames().ToArray();
            this.AddedProcessNames = newProcessNames.Except(this.ProcessNames).ToArray();
            this.RemovedProcessNames = this.ProcessNames.Except(newProcessNames).ToArray();
            this.ProcessNames = newProcessNames;
        }

        private DispatcherTimer _updateTimer;

        /// <summary>
        /// プロセスに関するパフォーマンスカウンタ管理のインスタンス
        /// </summary>
        private ProcessCounterManager _pcManager = new ProcessCounterManager();

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
    }
}
