namespace Bars.FIAS.Converter
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Drawing;

    public partial class ProgressForm : Form
    {
        public BackgroundWorker BackgroundWorker;

        public ProgressForm()
        {
            this.InitializeComponent();
        }
        
        public int MaximumProgress
        {
            get { return this.progressBarИндикатор.Maximum; }
            set { this.progressBarИндикатор.Maximum = value; }
        }

        public int Value
        {
            get { return this.progressBarИндикатор.Value; }
            set { this.progressBarИндикатор.Value = value; }
        }

        public string EventName
        {
            get
            {
                return this.labelEventName.Text;
            }

            set 
            {
                this.labelEventName.Text = value;
                this.Refresh();
                Application.DoEvents();
            }
        }
        
        public void ShowProgress()
        {
            this.Show();
            Application.DoEvents();
        }

        public void CloseProgress()
        {
            this.Close();
        }
        
        public void UpdateLocation()
        {
            if (this.Owner == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point
                {
                    X = this.Owner.Location.X + this.Owner.Size.Width / 2 - this.Size.Width / 2,
                    Y = this.Owner.Location.Y + this.Owner.Size.Height / 2 - this.Size.Height / 2
                };
            }
        }
        
        private void ButtinCancelClick(object sender, EventArgs e)
        {
            if (this.BackgroundWorker != null)
            {
                this.BackgroundWorker.CancelAsync();
            }
        }
        
        private void ProgressFormLoad(object sender, EventArgs e)
        {
            this.UpdateLocation();
        }
    }
}