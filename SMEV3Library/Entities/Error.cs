using System;

namespace SMEV3Library.Entities
{
    public class Error
    {
        public string Text { get; }

        public Exception Exception { get; }

        public Error(Exception e)
        {
            Exception = e;
            Text = e.Message;
        }

        public Error(string text)
        {
            this.Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
