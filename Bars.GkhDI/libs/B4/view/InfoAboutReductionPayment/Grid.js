Ext.define('B4.view.infoaboutreductionpayment.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.infreductpaymgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField'
    ],

    title: 'Сведения о случаях снижения платы',
    store: 'InfoAboutReductionPayment',
    itemId: 'infoAboutReductionPaymentGrid',
    closable: false,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeGroupServiceDi',
                    flex: 1,
                    text: 'Группа услуги',
                    hidden: true,
                    renderer: function (val) {
                        switch (val) {
                            case 10:
                                return 'Коммунальные';
                            case 20:
                                return 'Жилищные';
                            default:
                                return 'Прочие';
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseServiceName',
                    flex: 1,
                    text: 'Наименование услуги',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReasonReduction',
                    flex: 1,
                    text: 'Причина снижения',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'OrderDate',
                    itemId: 'dcOrderDate',
                    flex: 1,
                    text: 'Дата приказа',
                    editor:
                    {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    filter:
                    {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrderNum',
                    itemId: 'dcOrderNum',
                    flex: 1,
                    text: 'Номер приказа',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecalculationSum',
                    flex: 1,
                    text: 'Сумма пересчета',
                    editor: 'gkhdecimalfield',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Примечание (номера квартир)',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })],
            features: [Ext.create('Ext.grid.feature.Grouping', {
                groupHeaderTpl: '{name}'
            })],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});