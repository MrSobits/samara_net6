Ext.define('B4.view.prescription.CancelEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minHeight: 300,
    bodyPadding: 5,
    itemId: 'prescriptionCancelEditWindow',
    title: 'Решение',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypePrescriptionCancel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            labelWidth: 130
                        }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    name: 'SmolTypeCancel',
                    fieldLabel: 'Тип решения',
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false,
                    items: B4.enums.TypePrescriptionCancel.getItems()
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'IssuedCancel',
                    fieldLabel: 'ДЛ, вынесшее решение',
                    editable: false,
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    dockedItems: [
                       {
                           xtype: 'b4pagingtoolbar',
                           displayInfo: true,
                           store: 'B4.store.dict.Inspector',
                           dock: 'bottom'
                       }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            floating: false,
                            name: 'SmolPetitionNum',
                            fieldLabel: 'Номер ходатайства',
                            hidden: true
                        },
                        {
                            xtype: 'datefield',
                            name: 'SmolPetitionDate',
                            fieldLabel: 'Дата ходатайства',
                            format: 'd.m.Y',
                            labelWidth: 130,
                            hidden: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            editable: false,
                            floating: false,
                            name: 'IsCourt',
                            fieldLabel: 'Отменено судом',
                            displayField: 'Display',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateCancel',
                            fieldLabel: 'Дата отмены',
                            format: 'd.m.Y',
                            labelWidth: 130
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Reason',
                    fieldLabel: 'Причина',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    name: 'SmolDescriptionSet',
                    fieldLabel: 'Установлено',
                    maxLength: 2000,
                    flex: 1,
                    hidden: true
                },
                {
                    xtype: 'textarea',
                    name: 'SmolCancelResult',
                    fieldLabel: 'Результат решения',
                    maxLength: 2000,
                    flex: 1,
                    hidden: true
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
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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