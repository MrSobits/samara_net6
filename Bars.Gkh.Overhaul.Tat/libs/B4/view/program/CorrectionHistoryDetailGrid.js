Ext.define('B4.view.program.CorrectionHistoryDetailGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKind',
        'B4.store.program.CorrectionHistoryDetail'
    ],

    alias: 'widget.progcorrecthistorydetgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.program.CorrectionHistoryDetail');

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                { text: 'Наименование атрибута', dataIndex: 'PropertyName', flex: 2 },
                {
                    text: 'Старое значение', dataIndex: 'OldValue', flex: 2,
                    renderer: function (val, metadata, record) {
                        if (record.data.Type) {
                            switch (record.data.Type) {
                                case 'Decimal':
                                    return val ? Ext.util.Format.currency(val) : '';
                            }
                        }

                        return val;
                    }
                },
                {
                    text: 'Новое значание',
                    dataIndex: 'NewValue',
                    flex: 2,
                    renderer: function (val, metadata, record) {
                        if (record.data.Type) {
                            switch (record.data.Type) {
                                case 'Decimal':
                                    return val ? Ext.util.Format.currency(val) : '';
                            }
                        }

                        return val;
                    }
                }
            ],
            
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

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