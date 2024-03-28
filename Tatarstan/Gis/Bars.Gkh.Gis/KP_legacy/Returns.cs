// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Global.Returns
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Retcode", Namespace = "http://www.stcline.ru")]
    public struct Returns
    {
        [DataMember(Name = "result", Order = 0)]
        public bool result;
        [DataMember(Name = "text", Order = 1)]
        public string text;
        [DataMember(Name = "tag", Order = 2)]
        public int tag;
        [DataMember(Name = "sql_error", Order = 3)]
        public string sql_error;

        public Returns(bool _result, string _text, int _tag)
        {
            this.result = _result;
            this.text = _text;
            this.sql_error = "";
            this.tag = _tag;
        }

        public Returns(bool _result, string _text)
        {
            this.result = _result;
            this.text = _text;
            this.sql_error = "";
            this.tag = 0;
        }

        public Returns(bool _result)
        {
            this.result = _result;
            this.text = "";
            this.sql_error = "";
            this.tag = 0;
        }
    }
}
