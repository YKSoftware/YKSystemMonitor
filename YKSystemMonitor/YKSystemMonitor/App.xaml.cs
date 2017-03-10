namespace YKSystemMonitor
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Threading;
    using YKSystemMonitor.ViewModels;
    using YKSystemMonitor.Views;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 起動処理
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += OnDispatcherUnhandledException;

            YKToolkit.Controls.ThemeManager.Instance.Initialize("Dark Orange");

            var w = new MainView() { DataContext = new MainViewModel() };
            w.Show();
        }

        /// <summary>
        /// 未処理例外発生イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter("log.txt");
                writer.Write(e.Exception);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }

            YKToolkit.Controls.MessageBox.Show("未処理例外が発生したのでアプリケーションを終了します。" + Environment.NewLine + "詳細は log.txt を参照してください。", "Oops...");
            this.Shutdown();
        }
    }
}
