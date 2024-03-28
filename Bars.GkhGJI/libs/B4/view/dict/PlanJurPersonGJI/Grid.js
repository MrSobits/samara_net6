Ext.define('B4.view.dict.planjurpersongji.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Планы проверок юридических лиц',
    store: 'dict.PlanJurPersonGji',
    alias: 'widget.planJurPersonGjiGrid',
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
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        allowBlank: false
                    }
                },
                {
                    header: 'Регистрационный номер плана в едином реестре проверок',
                    flex: 1,
                    dataIndex: 'UriRegistrationNumber',
                    xtype: 'gridcolumn',
                    editor: {
                        xtype: 'numberfield',
                        allowDecimals: false,
                        operand: CondExpr.operands.eq,
                        minValue: 0
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateApproval',
                    text: 'Дата утверждения плана',
                    format: 'd.m.Y',
                    width: 140,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    width: 120,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 120,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        allowBlank: false
                    }
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
                })
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
                                    xtype: 'b4savebutton'
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