Ext.define('B4.view.dictionaries.RecordGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.recordgrid',
    columnLines: true,

    requires: [
        'B4.store.DictionaryRecord'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.DictionaryRecord');

        Ext.applyIf(me,
            {
                store: store,
                columns: [
                    {
                        text: 'Запись внешней системы',
                        dataIndex: 'ExternalName',
                        flex: 1
                    },
                    {
                        text: 'Запись ГИС ЖКХ',
                        dataIndex: 'GisName',
                        flex: 1
                    }
                ]
            });

        me.callParent(arguments);
    }
});
