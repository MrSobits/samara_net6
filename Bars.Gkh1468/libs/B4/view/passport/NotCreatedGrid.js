Ext.define('B4.view.passport.NotCreatedGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.notcreatedgrid',
    closable: true,

    title: 'Контрагенты, не создавшие паспорта',

    requires: [
        //'B4.view.passport.house.info.Grid',
        //'B4.view.passport.house.info.PassportPanel'
        'B4.store.Contragent'
    ],
    
    initComponent: function() {
        var me = this;
        var store = Ext.create('B4.store.Contragent');
        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                }
            ]
        });

        me.callParent(arguments);
    }
});