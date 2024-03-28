Ext.define('B4.view.actisolated.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.actisolatededitpanel',
    title: '',

    requires: [
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.FiasSelectAddress',

        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhButtonPrint',

        'B4.view.actisolated.WitnessGrid',
        'B4.view.actisolated.PeriodGrid',
        'B4.view.actisolated.InspectedPartGrid',
        'B4.view.actisolated.DefinitionGrid',
        'B4.view.actisolated.AnnexGrid',
        'B4.view.actisolated.RealityObjectGrid',
        'B4.view.actisolated.ProvidedDocGrid',
        'B4.view.GjiDocumentCreateButton',
    ],

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        xtype: 'container',
                        border: false,
                        layout: 'hbox',
                        shrinkWrap: true,
                        margin: 6
                    },
                    items: [
                        {
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
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300,
                                    labelWidth: 140,
                                    width: 295
                                }
                            ]
                        },
                        {
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
                                    width: 200,
                                    hideTrigger: true
                                },
                                {
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
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
                    itemId: 'actIsolatedTabPanel',
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
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        allowBlank: false,
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'actIsolatedInspectors',
                                            itemId: 'trigfInspectors',
                                            fieldLabel: 'Инспекторы',
                                            flex: 5
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'Area',
                                            itemId: 'nfArea',
                                            fieldLabel: 'Площадь',
                                            allowBlank: true,
                                            labelWidth: 120,
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 0 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            flex: 5,
                                            defaults: {
                                                labelWidth: 150,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4fiasselectaddress',
                                                    name: 'DocumentPlaceFias',
                                                    fieldLabel: 'Место составления',
                                                    flex: 3,
                                                    fieldsRegex: {
                                                        tfHousing: {
                                                            regex: /^\d+$/,
                                                            regexText: 'В это поле можно вводить только цифры'
                                                        },
                                                        tfBuilding: {
                                                            regex: /^\d+$/,
                                                            regexText: 'В это поле можно вводить только цифры'
                                                        }
                                                    }
                                                },
                                                {
                                                    xtype: 'timefield',
                                                    fieldLabel: 'Время составления акта',
                                                    format: 'H:i',
                                                    altFormats: 'H:i:s',
                                                    increment: 60,
                                                    submitFormat: 'Y-m-d H:i:s',
                                                    itemId: 'tfDocumentTime',
                                                    name: 'DocumentTime',
                                                    flex: 1,
                                                    labelWidth: 200
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Flat',
                                            itemId: 'tfFlat',
                                            fieldLabel: 'Квартира',
                                            maxLength: 10,
                                            flex: 1,
                                            labelWidth: 120,
                                            labelAlign: 'right'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'actisolatedwitnessgrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                },
                                {
                                    xtype: 'actisolatedperiodgrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                }
                            ]
                        },
                        {
                            xtype: 'actisolatedrealityobjectgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actisolatedprovideddocgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actisolatedinspectedpartgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actisolateddefinitiongrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actisolatedannexgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGjiToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            itemId: 'mainButtonGroup',
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