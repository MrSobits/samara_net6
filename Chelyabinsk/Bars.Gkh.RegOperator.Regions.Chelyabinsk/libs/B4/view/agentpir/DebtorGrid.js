Ext.define('B4.view.agentpir.DebtorGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.enums.AgentPIRDebtorStatus'
    ],

    title: 'Должники агент ПИР',
    store: 'AgentPIRDebtor',
    alias: 'widget.agentpirdebtorGrid',
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
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    text: 'Номер ЛС ФКР',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnifiedAccountNumber',
                    flex: 1,
                    text: 'Номер ЛС ГИС ЖКХ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BasePersonalAccount',
                    flex: 1,
                    text: 'Абонент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    flex: 1,
                    text: 'Статус ЛС',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 2,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaShare',
                    text: 'Доля собственности',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    },
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.AgentPIRDebtorStatus',
                    dataIndex: 'Status',
                    flex: 1,
                    text: 'Статус',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    name: 'DebtBaseTariff',
                    dataIndex: 'DebtBaseTariff',
                    text: 'Основной долг',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PenaltyDebt',
                    dataIndex: 'PenaltyDebt',
                    text: 'Пени',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Credit',
                    dataIndex: 'Credit',
                    text: 'Зачтено',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
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
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    if (record.get('AreaShare') < 1) {
                        return 'back-yellow';
                    }

                    return '';
                }
            }
        });

        me.callParent(arguments);
    }
});