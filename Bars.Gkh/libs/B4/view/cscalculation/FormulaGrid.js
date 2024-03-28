Ext.define('B4.view.cscalculation.FormulaGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.cscalculationformulagrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.store.cscalculation.CSFormula'
    ],

    closable: true,
    title: 'Формулы расчета платы за ЖКУ',
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.cscalculation.CSFormula');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    header: 'Формула',
                    dataIndex: 'Formula',
                    flex: 3
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function () {
                                            store.load();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                { xtype: 'b4pagingtoolbar', store: store, dock: 'bottom', displayInfo: true }
            ]
        });

        me.callParent(arguments);
    }
});