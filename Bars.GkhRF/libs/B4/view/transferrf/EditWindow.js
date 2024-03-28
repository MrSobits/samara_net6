Ext.define('B4.view.transferrf.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.ContractRf',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.ux.grid.Panel',
        'B4.view.transferrf.RecordGrid'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    maximized: true,
    bodyPadding: 5,
    itemId: 'transferRfEditWindow',
    title: 'Перечисление средств в фонд',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'container',
                    height: 35,
                    layout: {
                        type: 'anchor'
                    },
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'ContractRf',
                            itemId: 'ContractRf',
                            textProperty: 'DocumentNum',
                            fieldLabel: 'Договор',
                            windowContainerSelector: '#transferRfEditWindow',
                            store: 'B4.store.ContractRf',
                            padding: '5px 0 5px 0',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'MunicipalityName',
                                    width: 155,
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
                                    dataIndex: 'DocumentNum',
                                    width: 73,
                                    text: '№ договора',
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'DocumentDate',
                                    width: 85,
                                    text: 'Дата договора',
                                    filter: {
                                        xtype: 'datefield',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ManagingOrganizationName',
                                    flex:1,
                                    text: 'Наименование УК(ТСЖ)',
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ContractRfObjectsCount',
                                    width: 120,
                                    text: 'Кол-во объектов МКД',
                                    filter: {
                                        xtype: 'numberfield',
                                        hideTrigger: true,
                                        operand: CondExpr.operands.eq
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'transferrfrecordgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});