Ext.define('B4.view.dict.OSP.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Отделения судебных приставов',
    store: 'dict.OSP',
    alias: 'widget.oSPGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShortName',
                    flex: 1,
                    text: 'Краткое наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Town',
                    flex: 1,
                    text: 'Населенный пункт'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Street',
                    flex: 1,
                    text: 'Улица'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BankAccount',
                    flex: 1,
                    text: 'Расчетный счет'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreditOrg',
                    flex: 1,
                    text: 'Банк'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OKTMO',
                    flex: 1,
                    text: 'ОКТМО'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
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