// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces._OrderingField
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct _OrderingField
    {
        private string _fieldName;
        private OrderingDirection _orderingDirection;

        [DataMember]
        public string fieldName
        {
            get
            {
                return this._fieldName;
            }
            set
            {
                this._fieldName = value;
            }
        }

        [DataMember]
        public OrderingDirection orderingDirection
        {
            get
            {
                return this._orderingDirection;
            }
            set
            {
                this._orderingDirection = value;
            }
        }

        public _OrderingField(string field)
        {
            this._fieldName = field;
            this._orderingDirection = OrderingDirection.Ascending;
        }

        public _OrderingField(string field, OrderingDirection order)
        {
            this._fieldName = field;
            this._orderingDirection = order;
        }
    }
}
