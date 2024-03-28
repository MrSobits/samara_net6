Ext.define('B4.view.contractrf.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.store.ManagingOrganization',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.contractrf.ObjectGridOut',
        'B4.view.contractrf.ObjectGridIn',
        
        'B4.enums.TypeManagementManOrg'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 1000,
    minWidth: 600,
    height: 700,
    minHeight: 400,
    maxHeight: 800,
    bodyPadding: 5,
    itemId: 'contractRfEditWindow',
    title: 'Договор с управляющей компанией',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    height: 200,
                    layout: 'anchor',
                    defaults: {
                        labelWidth: 170,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'column',
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        allowBlank: false,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNum',
                                            fieldLabel: 'Номер',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateBegin',
                                            fieldLabel: 'Дата начала действия',
                                            format: 'd.m.Y'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        anchor: '100%',
                                        format: 'd.m.Y'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            allowBlank: false,
                                            fieldLabel: 'Дата договора'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEnd',
                                            fieldLabel: 'Дата окончания действия'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'ManagingOrganization',
                            fieldLabel: 'Управляющая организация',
                           

                            store: 'B4.store.ManagingOrganization',
                            allowBlank: false,
                            editable: false,
                            textProperty: 'ContragentName',
                            columns: [
                                { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    text: 'Тип управления', dataIndex: 'TypeManagement', flex: 1,
                                    renderer: function (val) {
                                        return B4.enums.TypeManagementManOrg.displayRenderer(val);
                                    },
                                    filter: {
                                        xtype: 'b4combobox',
                                        items: B4.enums.TypeManagementManOrg.getItemsWithEmpty([null, '-']),
                                        editable: false,
                                        operand: CondExpr.operands.eq,
                                        valueField: 'Value',
                                        displayField: 'Display'
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл'
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            title: 'Реквизиты соглашения о расторжении договора',
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    anchor: '100%',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 160,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TerminationContractNum',
                                            fieldLabel: 'Номер',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'TerminationContractDate',
                                            fieldLabel: 'Дата расторжения договора'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: {
                                        type: 'anchor'
                                    },
                                    defaults: {
                                        labelWidth: 160,
                                        labelAlign: 'right',
                                        anchor: '100%',
                                        format: 'd.m.Y'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4filefield',
                                            name: 'TerminationContractFile',
                                            fieldLabel: 'Файл'
                                        }
                                    ]
                                }
                            ]
                        }
                        
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tabObjectContractRfGridInOut',
                    flex: 1,
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'objectcontractrfgridin'
                        },
                        {
                            xtype: 'objectcontractrfgridout'
                        }
                    ]
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