Ext.define('B4.view.version.VersionParamsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.versionparamsgrid',

    requires: [
        'B4.ux.button.Close',
        'B4.store.version.Params',
        'B4.store.program.PriorityParam'
    ],

    initComponent: function () {
        var me = this,
            pStore = Ext.create('B4.store.program.PriorityParam'),
            store = Ext.create('B4.store.version.Params');
        
        pStore.load();

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
               {
                   header: 'Параметр',
                   dataIndex: 'Code',
                   flex: 1,
                   renderer: function (value) {
                       var record = pStore.findRecord('Code', value);
                       if (record) {
                           return record.get('Name');
                       }
                   }
               },
               {
                   header: 'Вес',
                   dataIndex: 'Weight',
                   flex: 1
               }
            ],
            tbar: {
                items: [
                    '->',
                    { xtype: 'b4closebutton' }
                ]
            }
        });

        me.callParent(arguments);
    }
});