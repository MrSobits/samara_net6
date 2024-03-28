Ext.define('B4.view.prescription.OfficialReportEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 1000,
    minHeight: 300,
    bodyPadding: 5,
    itemId: 'prescriptionfficialReportEditWindow',
    title: 'Форма приложения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.YesNo',
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.view.prescription.OfficialReportViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '5 0 5 0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'OfficialReportType',
                            fieldLabel: 'Тип документа',
                            displayField: 'Display',
                            itemId: 'cbOfficialReportType',
                            flex: 1,
                            store: B4.enums.OfficialReportType.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        }
                    ]
                }, 
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '5 0 5 0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            flex: 1,
                            fieldLabel: 'Номер документа',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y'
                        }                       
                    ]
                },  
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '5 0 5 0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'YesNo',
                            fieldLabel: 'Устранены',
                            displayField: 'Display',
                            itemId: 'cbYesNo',
                            flex: 1,
                            store: B4.enums.YesNo.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'ViolationDate',
                            itemId: 'dfViolationDate',
                            fieldLabel: 'Дата устранения',
                            format: 'd.m.Y'
                        }
                    ]
                },   
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '5 0 5 0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ExtensionViolationDate',
                            itemId: 'dfExtensionViolationDate',
                            fieldLabel: 'Продлить до',
                            format: 'd.m.Y'
                        }
                    ]
                },   
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Inspector',
                    fieldLabel: 'Инспектор',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    editable: false,
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    itemId: 'sfInspector'
                },
                {
                    xtype: 'prescriptionoffrepviolationgrid',
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
                            xtype: 'buttongroup',
                            items: [
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