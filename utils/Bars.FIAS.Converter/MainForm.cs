namespace Bars.FIAS.Converter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.IO;
    using System.Linq;

    public partial class MainForm : Form
    {
        #region Конструктор

        public MainForm()
        {
            this.InitializeComponent();

            this.Converter = new Converter();

            var connectionString = (new RegistryHelper()).GetConnectionstring();
            this.tbConnectionString.Text = connectionString ?? Converter.DefaultConnectionString;
            this.cbDataBaseType.DataSource = Enum.GetValues(typeof(EnumTypeDataBase));
            this.cbDataBaseType.SelectedIndex = 0;

            this.cbTypeOLEDB.DataSource = Enum.GetValues(typeof(EnumTypeOLEDB));
            this.cbTypeOLEDB.SelectedIndex = 0;

            this.cbTypeLoad.SelectedIndex = 0;
        }

        #endregion

        #region Свойства

        public ProgressForm ProgressForm { get; private set; }

        public Converter Converter { get; private set; }

        #endregion

        #region Показать исключительную ситуацию

        public static void ShowError(string message, Exception exc)
        {
            string messageText = string.Format("{0}\n\n{1}", message, exc.Message);
            if (exc.InnerException != null)
            {
                messageText += "\n\n>>>>> Внутреннее исключение: " + "\n" + exc.InnerException.Message;
            }

            MessageBox.Show(messageText, "Исключительная ситуация", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #endregion

        #region Асинхронные обработчики

        #region Асинхронные обработчики для получения списка регионов

        private void RegionListDoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker)
            {
                var worker = sender as BackgroundWorker;
                this.ProgressForm.BackgroundWorker = worker;
                object[] arg = (object[])e.Argument;

                e.Result = this.Converter.GetRegionList((string)arg[0], (EnumTypeOLEDB)arg[1], worker, e);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void RegionListProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressForm.Value = e.ProgressPercentage;
        }

        private void RegionListRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ProgressForm.CloseProgress();
            if (e.Error != null)
            {
                MainForm.ShowError("Не удалось получить список регионов", e.Error);
            }
            else if (!e.Cancelled)
            {
                var regions = e.Result as SortedDictionary<string, string>;
                if (regions != null)
                {
                    foreach (var kv in regions)
                    {
                        this.regionList.Items.Add(kv.Key);
                    }
                }
            }
        }

        #endregion

        #region Асинхронные обработчики для загрузки ФИАС в БД
        private void bwLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker)
            {
                var worker = sender as BackgroundWorker;

                this.ProgressForm.BackgroundWorker = worker;

                object[] arg = (object[])e.Argument;

                var fiasRecords = this.Converter.GetLoadFiasRecords(worker, this.pathFias.Text, (EnumTypeOLEDB) arg[4], (List<string>) arg[0]);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }

                var fiasHouseRecords = this.Converter.GetLoadFiasHouseRecords(worker, this.pathFias.Text, (EnumTypeOLEDB)arg[4], (List<string>)arg[0]);
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }

                this.Converter.LoadFias(worker, (int)arg[1], (string)arg[3], fiasRecords, fiasHouseRecords);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void bwLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressForm.Value = e.ProgressPercentage;
        }

        private void bwLoaderRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ProgressForm.CloseProgress();
            if (e.Error != null)
            {
                MainForm.ShowError("Не удалось загрузить", e.Error);
            }
            else if (!e.Cancelled)
            {
                MessageBox.Show("Загрузка выполнена успешно");
            }
        }
        #endregion

        #endregion

        #region Методы

        private void SelectAllRegion(bool selected)
        {
            for (int i = 0; i < this.regionList.Items.Count; i++)
            {
                this.regionList.SetItemChecked(i, selected);
            }

            this.regionList.Refresh();
        }

        #endregion

        #region События формы

        private void MainFormLocationChanged(object sender, EventArgs e)
        {
            if (this.ProgressForm != null)
            {
                this.ProgressForm.UpdateLocation();
            }
        }

        #endregion

        #region Обработчики кнопок
        private void ButtonSelectAllClick(object sender, EventArgs e)
        {
            this.SelectAllRegion(true);
        }

        private void ButtonUnselectAllClick(object sender, EventArgs e)
        {
            this.SelectAllRegion(false);
        }

        private void ButtonPushFiasClick(object sender, EventArgs e)
        {
            var regions = (from object item in this.regionList.CheckedItems where this.Converter.Regions.ContainsKey((string) item) select (this.Converter.Regions[(string) item])).ToList();

            if (!regions.Any())
            {
                MessageBox.Show("Необходимо выбрать хотя бы один регион для загрузки.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                this.ProgressForm = new ProgressForm
                {
                    Owner = this,
                    MaximumProgress = 100,
                    Value = 0,
                    EventName = "Загрузка записей..."
                };
                this.ProgressForm.ShowProgress();

                this.bwLoader.RunWorkerAsync(new object[] { regions, this.cbTypeLoad.SelectedIndex, this.cbDataBaseType.SelectedIndex, this.tbConnectionString.Text, this.cbTypeOLEDB.SelectedIndex });
            }
            catch (Exception exc)
            {
                MainForm.ShowError("Не удалось загрузить записи ФИАС", exc);
            }


        }

        #endregion

        private void ButtonSelectFiasPath(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                var files = Directory.GetFiles(dlg.SelectedPath + @"\", Converter.MainFilePattern);
                if (!files.Any())
                {
                    throw new FileNotFoundException("Не удалось найти файл: " + Converter.MainFilePattern);
                }

                this.regionList.Items.Clear();
                this.pathFias.Text = dlg.SelectedPath;

                try
                {
                    this.ProgressForm = new ProgressForm
                    {
                        Owner = this,
                        MaximumProgress = 100,
                        Value = 0,
                        EventName = "Подготовка районов..."

                    };
                    this.ProgressForm.ShowProgress();

                    this.bwRegionList.RunWorkerAsync(new object[] {this.pathFias.Text, this.cbTypeOLEDB.SelectedIndex });
                }
                catch (Exception exc)
                {
                    MainForm.ShowError("Не удалось загрузить список регионов", exc);
                }

            }
            catch (Exception exc)
            {
                MainForm.ShowError("Не удалось осуществить выбор папки.", exc);
            }

        }

        private void TestConnectionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(this.Converter.TestConnection(this.tbConnectionString.Text)
                    ? "Подключение установлено"
                    : "Подключение не установлено");
            }
            catch (Exception exc)
            {
                MainForm.ShowError("Ошибка соединения", exc);
            }

        }

        private void saveConnectionBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show((new RegistryHelper()).SaveConnectionString(this.tbConnectionString.Text)
                ? "Настройки успешно сохранены"
                : "Не удалось сохранить настройки");
        }
    }
}