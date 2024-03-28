Ext.define('B4.view.cscalculation.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.cscalculationGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar'
    ],

    store: 'cscalculation.CSCalculation',
    title: 'Расчет платы ЖКУ',
    closable: true,
    enableColumnHide: true,

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
                    text: 'Наименование',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CSFormula',
                    text: 'Формула',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectEditDate',
                    text: 'Дата расчета',
                    format: 'd.m.Y',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                }, 
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CalcDate',
                    text: 'Расчет на дату',
                    format: 'd.m.Y',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },           
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    text: 'Описание',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Result',
                    text: 'Результат',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'b4updatebutton'
                        },     
                        {
                            xtype: 'datefield',
                            labelWidth: 60,
                            fieldLabel: 'Дата с',
                            width: 160,
                            itemId: 'dfDateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 30,
                            fieldLabel: 'по',
                            width: 130,
                            itemId: 'dfDateEnd',
                            value: new Date(new Date().getFullYear(), 11, 31)
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