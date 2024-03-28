Ext.define('B4.view.subkpkr.KEGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.subkpkr.SubProgramKPKRKE',
        'Ext.ux.RowExpander'
    ],
    alias: 'widget.subkpkrkkegrid',
    title: 'Конструктивные элементы',
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.subkpkr.SubProgramKPKRKE');      

        store.on('beforeload', function (store, operation) {
            var editfrm = me.up('versmakesubkpkrwin');

            operation = operation || {};
            operation.params = operation.params || {};
            operation.params.StartYear = editfrm.down('numberfield[name=StartYear]').getValue();
            operation.params.YearCount = editfrm.down('numberfield[name=YearCount]').getValue();
            operation.params.FirstYearPSD = editfrm.down('checkbox[name=FirstYearPSD]').getValue();
            operation.params.FirstYearWithoutWork = editfrm.versionId;
        }, me);

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columns: [
                {
                    header: 'КЭ',
                    dataIndex: 'KE',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    header: 'Адрес',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    header: 'Сумма',
                    dataIndex: 'Sum',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield'
                    },
                },
            ],
            plugins: [
                Ext
                .create(
                    'B4.ux.grid.plugin.HeaderFilters'
                )
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    }
});