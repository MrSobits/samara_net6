namespace Bars.Gkh.RegionLoader
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using Base;

    public partial class MainWindow
    {
        private string _region;

        private readonly RegionLoadManager _regionloadManager;

        public MainWindow()
        {
            InitializeComponent();

            _regionloadManager = new RegionLoadManager();

            try
            {
                _regionloadManager.InitializeLoader();

                Dispatcher.Invoke((Action)(() =>
                {
                    cmbxRegions.ItemsSource = _regionloadManager.Regions;

                    var itemIndex = GetItemIndex(cmbxRegions.Items);

                    if (itemIndex == -1)
                    {
                        if (!string.IsNullOrEmpty(_regionloadManager.Region))
                        {
                            MessageBox.Show("Регион заданный в файле сборки отстутствует в modules.json", "Внимание", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            statusBar.Text = "Инициализация конфигуратора завершилась с предупреждением: Неизвестный регион";
                        }
                    }
                    else
                    {
                        cmbxRegions.SelectedIndex = itemIndex;
                    }

                    statusBar.Text = "Конфигуратор был успешно инициализирован. " +
                                     (string.IsNullOrEmpty(_regionloadManager.Region)
                                         ? "Вы можете выбрать регион"
                                         : "Текущий регион сборки - "
                                           + _regionloadManager.Region);
                }));
            }
            catch (Exception e)
            {
                MessageBox.Show("При инициализации лоадера произошла ошибка\r\n" + e, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                frmMain.Close();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            _region = (string)e.AddedItems[0];
            statusBar.Text = "Выбрана конфигурация для региона: " + _region;
        }

        private void OnSetRegion(object sender, RoutedEventArgs e)
        {
            try
            {
                _regionloadManager.SetRegion(_region);

                statusBar.Text = "Установлен регион сборки - " + _regionloadManager.Region; 
            }
            catch (Exception exc)
            {
                MessageBox.Show("При установке региона произошла ошибка\r\n" + exc, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private int GetItemIndex(ICollection items)
        {
            var index = -1;
            for (var i = 0; i < items.Count; i++)
            {
                if (string.Compare(
                    (string)cmbxRegions.Items[i], 
                    _regionloadManager.Region, 
                    StringComparison.OrdinalIgnoreCase) != -1)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
    }
}
