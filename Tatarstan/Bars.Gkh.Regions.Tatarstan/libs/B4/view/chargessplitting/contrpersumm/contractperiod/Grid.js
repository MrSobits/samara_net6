Ext.define('B4.view.chargessplitting.contrpersumm.contractperiod.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.contractperiodgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Отчетные периоды',
    store: 'chargessplitting.contrpersumm.ContractPeriod',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Отчетный период'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UoNumber',
                    flex: 1,
                    text: 'Количество управляющих организаций'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RsoNumber',
                    flex: 1,
                    text: 'Количество ресурсоснабжающих организаций'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoNumber',
                    flex: 1,
                    text: 'Количество домов'
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
                                },
                                {
                                    xtype: 'button',
                                    text: 'Актуализировать сведения',
                                    action: 'Actualize'
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