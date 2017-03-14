namespace YKSystemMonitor.Views
{
    using System.Windows.Controls;
    using YKToolkit.Controls;

    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded) return;

            var combobox = sender as ComboBox;
            var width = 800;
            var height = 600;
            if (combobox.SelectedIndex < 2)
            {
                width = combobox.SelectedIndex == 0 ? 400 : 640;
                height = combobox.SelectedIndex == 0 ? 300 : 480;
            }
            this.Width = width;
            this.Height = height;

            this.leftPanel.Visibility = combobox.SelectedIndex == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            this.configDropDownButton.IsDropDownOpen = false;
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ThemeManager.Instance.SetTheme((e.AddedItems[0] as ComboBoxItem).Content as string);
            this.configDropDownButton.IsDropDownOpen = false;
        }
    }
}
