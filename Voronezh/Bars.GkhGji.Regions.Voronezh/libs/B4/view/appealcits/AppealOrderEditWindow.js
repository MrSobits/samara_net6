Ext.define('B4.view.appealcits.AppealOrderEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 1200,
    minWidth: 800,
    height: 550,
    resizable: true,
    bodyPadding: 3,
    layout: 'form',
    itemId: 'appealcitsAppealOrderEditWindow',
    title: 'Форма редактирования обращения СОПР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.YesNoNotSet',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.view.appealcits.AppealOrderExecutantGrid',
        'B4.view.appealcits.AppealOrdeRealityObjectGrid',
        'B4.view.appealcits.AppealOrderFileGrid',
        'B4.store.dict.Inspector'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 130
            },
            items: [  
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ContragentName',
                            editable: false,
                            fieldLabel: 'Организация исполнитель',
                            flex: 2,
                            maxLength: 300,
                            labelWidth: 130
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            width: 250,
                            flex: 1,
                            editable: false,
                            disabled: true,
                            name: 'PerformanceDate',
                            fieldLabel: 'Дата исполнения',
                            format: 'd.m.Y'
                        }
                    ]
                },  
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Person',
                            editable: true,
                            flex:2,
                            allowBlank: false,
                            fieldLabel: 'Должностное лицо',
                            maxLength: 500,
                            labelWidth: 130
                        },
                        {
                            xtype: 'textfield',
                            name: 'PersonPhone',
                            editable: true,
                            flex: 1,
                            allowBlank: false,
                            fieldLabel: 'Телефон',
                            maxLength: 500,
                            labelWidth: 130
                        },
                    ]
                },          
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            editable: false,
                            fieldLabel: 'Номер обращения',
                            maxLength: 300,
                            labelWidth: 130
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            width: 250,
                            editable: false,
                            name: 'DateFrom',
                            fieldLabel: 'Дата обращения',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            width: 250,
                            editable: false,
                            name: 'OrderDate',
                            fieldLabel: 'Дата размещения в СОПР',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'combobox',
                            name: 'YesNoNotSet',
                            itemId: 'cbYesNoNotSet',
                            labelWidth: 130,
                            labelAlign: 'right',
                            fieldLabel: 'Отработано',
                            displayField: 'Display',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            editable: false
                        },
                    ]
                },
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    padding: '0 0 5 0',
                //    defaults: {
                //        labelAlign: 'right',
                //        flex: 1
                //    },
                //    items: [
                //        {
                //            xtype: 'textfield',
                //            name: 'Correspondent',
                //            editable: false,
                //            fieldLabel: 'Заявитель',
                //            maxLength: 300,
                //            labelWidth: 130
                //        },
                //        {
                //            xtype: 'textfield',
                //            name: 'CorrespondentAddress',
                //            editable: false,
                //            fieldLabel: 'Адрес заявителя',
                //            maxLength: 300,
                //            labelWidth: 130
                //        }
                //    ]
                //},
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    padding: '0 0 5 0',
                //    defaults: {
                //        labelAlign: 'right',
                //        flex: 1
                //    },
                //    items: [
                //        {
                //            xtype: 'textfield',
                //            name: 'Phone',
                //            editable: false,
                //            fieldLabel: 'Тел. заявителя',
                //            maxLength: 300,
                //            labelWidth: 130
                //        },
                //        {
                //            xtype: 'textfield',
                //            name: 'Email',
                //            editable: false,
                //            fieldLabel: 'Email',
                //            maxLength: 300,
                //            labelWidth: 130
                //        }
                //    ]
                //},
                {
                    xtype: 'combobox',
                    name: 'Confirmed',
                    itemId: 'cbConfirmed',
                    labelWidth: 130,
                    labelAlign: 'right',
                    fieldLabel: 'Принято инспектором',
                    displayField: 'Display',
                    store: B4.enums.YesNoNotSet.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    fileController: 'action/FileTransport',
                    downloadAction: 'GetFileFromPrivateServer',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'AppealText',
                    fieldLabel: 'Текст',
                    maxLength: 20000,
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    allowBlank: false,
                    fieldLabel: 'Отчет об исполнении',
                    maxLength: 20000,
                    flex: 1
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'appealCitizensTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'appealorderexecutantgrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealorderealityobjectgrid',
                            flex: 1
                        },
                        {
                            xtype: 'appealorderfilegrid',
                            flex: 1
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