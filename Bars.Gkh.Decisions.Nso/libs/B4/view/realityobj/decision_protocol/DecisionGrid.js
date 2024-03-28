Ext.define('B4.view.realityobj.decision_protocol.DecisionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.protodecisiongrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.realityobj.decision_protocol.Decision'
    ],

    title: 'Решения собственников',
    store: 'realityobj.decision_protocol.Decision',
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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Решение'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    flex: 1,
                    text: 'Дата ввода в действие',
                    format: 'd.m.Y'
                },

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsActual',
                    flex: 1,
                    text: 'Действующий',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                }
                //{
                //    xtype: 'datecolumn',
                //    dataIndex: 'StartDate',
                //    flex: 1,
                //    text: 'Дата окончания действия'
                //},
                //{
                //    xtype: 'b4deletecolumn',
                //    scope: me
                //}
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
                                { xtype: 'b4addbutton' },
                                {
                                    xtype: 'button',
                                    text: 'Действующие решения'
                                },
                                { xtype: 'b4updatebutton' }
                            ]
                        },
                        {
                            xtype: 'checkbox',
                            name: 'showNonActive',
                            fieldLabel: 'Показать неактуальные',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});