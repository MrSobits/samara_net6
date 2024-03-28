Ext.define('B4.view.actaction.ActActionSelectionGrid', {
   extend: 'B4.ux.grid.Panel',
   requires: [
      'B4.ux.grid.toolbar.Paging',
      'B4.enums.ActCheckActionType'
   ],

   alias: 'widget.actactionselectiongrid',

   initComponent: function () {
      var me = this;

      Ext.applyIf(me, {
         columnLines: true,
         columns: [
            {
               xtype: 'b4enumcolumn',
               dataIndex: 'ActionType',
               enumName: 'B4.enums.ActCheckActionType',
               text: 'Вид действия',
               flex: 2
            },
            {
               xtype: 'gridcolumn',
               dataIndex: 'Number',
               text: 'Номер документа',
               flex: 1
            },
            {
               xtype: 'datecolumn',
               dataIndex: 'Date',
               format: 'd.m.Y',
               text: 'Дата документа',
               flex: 1
            },
            {
               xtype: 'gridcolumn',
               dataIndex: 'CreationPlace',
               text: 'Место составления документа',
               flex: 2,
               renderer: function(value){
                  return value != null ? value.AddressName : value;
               }
            }
         ],
         viewConfig: {
            loadMask: true
         },
         dockedItems: [
            {
               xtype: 'b4pagingtoolbar',
               displayInfo: true,
               store: this.store,
               dock: 'bottom'
            }
         ]
      });

      me.callParent(arguments);
   }
});