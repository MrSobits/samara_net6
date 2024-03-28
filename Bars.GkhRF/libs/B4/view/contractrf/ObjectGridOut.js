Ext.define('B4.view.contractrf.ObjectGridOut', {
    extend: 'B4.ux.grid.Panel',
    requires: [
         'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.view.Control.GkhCustomColumn',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],
    alias: 'widget.objectcontractrfgridout',
    itemId: 'objectGridOut',
    store: 'contractrf.ObjectOut',
    title: 'Дома, исключенные из договора',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'MunicipalityName',
                        flex: 1,
                        text: 'Муниципальное образование',
                        filter: {
                            xtype: 'b4combobox',
                            operand: CondExpr.operands.eq,
                            storeAutoLoad: false,
                            hideLabel: true,
                            editable: false,
                            valueField: 'Name',
                            emptyItem: { Name: '-' },
                            url: '/Municipality/ListWithoutPaging'
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'RealityObjectName',
                        flex: 2,
                        text: 'Жилой дом',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'GkhCode',
                        width: 100,
                        text: 'Код дома',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ManOrgName',
                        flex: 1,
                        text: 'Управляющая организация',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        editor: 'datefield',
                        dataIndex: 'ExcludeDate',
                        width: 120,
                        text: 'Дата исключения из договора',
                        filter: {
                            xtype: 'datefield',
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Note',
                        width: 150,
                        text: 'Примечание',
                        filter: { xtype: 'textfield' },
                        editor: {
                            xtype: 'textfield',
                            maxLength: 255
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
                                        xtype: 'b4updatebutton'
                                    },
                                    {
                                        xtype: 'button',
                                        itemId: 'contractRfOutSaveButton',
                                        iconCls: 'icon-accept',
                                        text: 'Сохранить'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'icon-table-go',
                                        text: 'Экспорт',
                                        textAlign: 'left',
                                        itemId: 'btnExcludeContractRfObjExport'
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        xtype: 'b4pagingtoolbar',
                        displayInfo: true,
                        store: 'contractrf.ObjectOut',
                        dock: 'bottom'
                    }
                ]
        });

        me.callParent(arguments);
    }
});