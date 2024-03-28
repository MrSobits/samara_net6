Ext.define('B4.view.actsurvey.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'actSurveyEditPanel',
    title: 'Акт обследования',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.actsurvey.OwnerGrid',
        'B4.view.actsurvey.PhotoGrid',
        'B4.view.actsurvey.AnnexGrid',
        'B4.view.actsurvey.InspectedPartGrid',
        
        'B4.enums.SurveyResult'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        border: false,
                        labelWidth: 170,
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '10px 15px 5px 15px',
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    itemId: 'dfDocumentDate',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    width: 295,
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '0 15px 20px 15px',
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 50,
                                    width: 200,
                                    hideTrigger: true
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 295,
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
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'actSurveyTabPanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            autoScroll: true,
                            defaults: {
                                labelWidth: 100
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            itemId: 'actSurveyAddressTextField',
                                            name: 'Address',
                                            fieldLabel: 'Адрес',
                                            readOnly: true,
                                            flex: 0.7
                                        },
                                        {
                                            xtype: 'textfield',
                                            flex: 0.3,
                                            name: 'Flat',
                                            fieldLabel: 'Квартира',
                                            itemId: 'tfFlat',
                                            maxLength: 10
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '5 0 5 0',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'actSurveyInspectors',
                                            itemId: 'trigfInspectors',
                                            fieldLabel: 'Инспекторы',
                                            allowBlank: false,
                                            flex: 0.7
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'Area',
                                            flex: 0.3,
                                            fieldLabel: 'Площадь, кв.м',
                                            itemId: 'nfArea'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'actSurveyOwnerGrid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    height: 250
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '5 0 5 0',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox', editable: false,
                                            floating: false,
                                            name: 'FactSurveyed',
                                            itemId: 'cbFactSurveyed',
                                            fieldLabel: 'Факт проверки',
                                            displayField: 'Display',
                                            store: B4.enums.SurveyResult.getStore(),
                                            valueField: 'Value'
                                        },
                                        {
                                            xtype: 'textfield',
                                            itemId: 'tfReason',
                                            name: 'Reason',
                                            fieldLabel: 'Причина',
                                            maxLength: 300
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    labelWidth: 100,
                                    labelAlign: 'right',
                                    itemId: 'taDescription',
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            xtype: 'actSurveyInspectedPartGrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            flex: 1
                        },
                        {
                            xtype: 'actSurveyPhotoGrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            flex: 1
                        },
                        {
                            xtype: 'actSurveyAnnexGrid',
                            bodyStyle: 'backrgound-color:transparent;',
                            flex: 1
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