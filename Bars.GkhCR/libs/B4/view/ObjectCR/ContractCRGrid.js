Ext.define('B4.view.objectcr.ContractCrGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.Url'
    ],

    title: 'Договоры на услуги',
    alias: 'widget.objectcrcontractgrid',
    closable: true,

    initComponent: function () {
        var me = this,
        store = Ext.create('B4.store.objectcr.ContractCr');

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 150,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 100,
                    text: 'Номер'
                },
                 {
                     xtype: 'datecolumn',
                     format: 'd.m.Y',
                     dataIndex: 'DateFrom',
                     flex: 1,
                     text: 'Дата'
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 3,
                    text: 'Подрядная организация'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumContract',
                    width: 120,
                    text: 'Сумма договора (руб.)',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                },
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