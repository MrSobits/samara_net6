Ext.define('B4.view.manorg.contract.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorgContractGrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.ux.grid.column.Enum',
        'B4.form.ComboBox',
        'B4.enums.TypeContractManOrgRealObj',
        'B4.enums.YesNoNotSet',
        'B4.enums.ContractStopReasonEnum',
        'B4.store.manorg.contract.Base'
    ],

    title: 'Управление домами',
    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.manorg.contract.Base');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContractString',
                    flex: 1,
                    text: 'Тип договора',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContractManOrgRealObj',
                    text: 'Вид управления',
                    width: 200,
                    hidden: true,
                    itemId: 'typeContractManOrgRealObjGridColumn',
                    renderer: function(val) {
                        return B4.enums.TypeContractManOrgRealObj.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeContractManOrgRealObj.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    itemId: 'addressGridColumn',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsTransferredManagement',
                    flex: 1,
                    text: 'Передано управление',
                    itemId: 'gcIsTransManag',
                    renderer: function(val) {
                        return B4.enums.YesNoNotSet.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.YesNoNotSet.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaMkd',
                    flex: 1,
                    hidden: true,
                    text: 'Общая площадь МКД',
                    filter: { xtype: 'gkhdecimalfield' },
                    renderer: function(val) {
                        return val && Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLivingNotLivingMkd',
                    flex: 1,
                    hidden: true,
                    text: 'Общая площадь жилых и нежилых помещений МКД',
                    filter: { xtype: 'gkhdecimalfield' },
                    renderer: function(val) {
                        return val && Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateLicenceRegister',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    text: 'Дата внесения в реестр лицензий',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegisterReason',
                    flex: 1,
                    hidden: true,
                    text: 'Основание включения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateLicenceDelete',
                    flex: 1,
                    hidden: true,
                    text: 'Дата исключения из реестра лицензий',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DeleteReason',
                    flex: 1,
                    hidden: true,
                    text: 'Основание исключения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ContractStopReasonEnum',
                    dataIndex: 'ContractStopReason',
                    header: 'Основание завершения обслуживания',
                    hidden: true,
                    width: 250,
                    filter: true,
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TerminationDate',
                    text: 'Дата расторжения',
                    format: 'd.m.Y',
                    hidden: true,
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    actionName: 'addJskTsj',
                                    text: 'Добавить',
                                    itemId: 'btnAddJskTsj',
                                    visible: false,
                                    hidden: true
                                },
                                {
                                    xtype: 'b4addbutton',
                                    actionName: 'addManOrgOwners',
                                    text: 'Добавить',
                                    itemId: 'btnAddManOrgOwners',
                                    visible: false,
                                    hidden: true
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    text: 'Обновить'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'cbShowNotValid',
                                    checked: false,
                                    boxLabel: 'Показать недействующие'
                                }
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