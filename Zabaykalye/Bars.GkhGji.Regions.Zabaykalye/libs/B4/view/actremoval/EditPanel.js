Ext.define('B4.view.actremoval.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch'},
    itemId: 'actRemovalEditPanel',
    title: 'Акт проверки предписания',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.actremoval.ViolationGrid',
        'B4.view.actremoval.ViolationGroupGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    height: 230,
                    layout: { type: 'vbox', align: 'stretch' },
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyPadding: 5,
                    frame: true,
                    items: [
                        {
                            xtype: 'container',
                            border: false,
                            layout: 'hbox',
                            padding: '0 15px 5px 15px',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    itemId: 'dfDocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 150
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300,
                                    labelWidth: 140,
                                    width: 240
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            border: false,
                            layout: 'hbox',
                            padding: '0 15px 5px 15px',
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 50,
                                    width: 150,
                                    hideTrigger: true
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 240,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'actRemovalBaseTextField',
                            name: 'BaseName',
                            fieldLabel: 'Предписание',
                            readOnly: true,
                            labelWidth: 140,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 140,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'actRemovalInspectors',
                                    itemId: 'actRemovalInspectorsTrigerField',
                                    fieldLabel: 'Инспекторы',
                                    allowBlank: false,
                                    flex: 0.7
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Area',
                                    fieldLabel: 'Площадь',
                                    allowBlank: false,
                                    itemId: 'nfAreaActRemoval',
                                    flex: 0.3,
                                    labelWidth: 80
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            //flex: 1,
                            padding: '5px 0 5px 0',
                            defaults: {
                                labelWidth: 140,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    editable: false,
                                    name: 'TypeRemoval',
                                    itemId: 'cbTypeRemoval',
                                    fieldLabel: 'Нарушения устранены',
                                    displayField: 'Display',
                                    items: B4.enums.YesNoNotSet.getItems(),
                                    valueField: 'Value',
                                    flex: 0.35
                                },
                                {
                                    xtype: 'component',
                                    flex: 0.35
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Flat',
                                    fieldLabel: 'Квартира',
                                    itemId: 'nfFlatActRemoval',
                                    flex: 0.3,
                                    maxLength: 250,
                                    labelWidth: 80
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            labelAlign: 'right',
                            labelWidth: 140,
                            itemId: 'taDescriptionActRemoval',
                            flex: 1,
                            maxLength: 2000
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    layout: 'fit',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'actRemovalViolationGrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            padding: '5 5 5 5'
                        },
                        {
                            xtype: 'actremovalViolationGroupGrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            padding: '5 5 5 5'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                //ToDo ГЖИ после перехода на правила выпилить
                                /*
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnCreateDocument',
                                    text: 'Сформировать',
                                    menu: [
                                        {
                                            text: 'Предписание',
                                            textAlign: 'left',
                                            itemId: 'btnCreateActRemovalToPrescription',
                                            actionName: 'createActRemovalToPrescription'
                                        },
                                        {
                                            text: 'Протокол',
                                            textAlign: 'left',
                                            itemId: 'btnCreateActRemovalToProtocol',
                                            actionName: 'createActRemovalToProtocol'
                                        }
                                    ]
                                },
                                */
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
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
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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