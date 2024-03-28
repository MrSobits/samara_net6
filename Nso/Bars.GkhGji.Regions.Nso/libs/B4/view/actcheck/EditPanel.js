Ext.define('B4.view.actcheck.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'actCheckEditPanel',
    title: 'Акт проверки',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.actcheck.WitnessGrid',
        'B4.view.actcheck.PeriodGrid',
        'B4.view.actcheck.InspectedPartGrid',
        'B4.view.actcheck.DefinitionGrid',
        'B4.view.actcheck.AnnexGrid',
        'B4.view.actcheck.ActRemovalGrid',
        'B4.view.actcheck.ProvidedDocGrid',
        'B4.view.actcheck.RealityObjectGrid',
        'B4.view.actcheck.RealityObjectEditPanel',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeDocumentGji',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.actcheck.InspectionResultPanel'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'actCheckTopPanel',
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        layout: 'hbox',
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
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
                            padding: '0 15px 20px 15px',
                            bodyStyle: Gkh.bodyStyle,
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
                    itemId: 'actCheckTabPanel',
                    border: false,
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    autoScroll: true,
                    listeners: {
                        render: function (p) {
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('actCheckTopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
                    defaults: {
                        minWidth: 1100
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 10,
                            frame: true,
                            autoScroll: true,
                            items: [
                                {
                                    xtype: 'container',
                                    layout:  'hbox',
                                    defaults: {
                                        labelWidth: 180,
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'actCheckInspectors',
                                            itemId: 'trigfInspectors',
                                            fieldLabel: 'Инспекторы',
                                            flex: 2
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'AcquaintedWithDisposalCopy',
                                            fieldLabel: 'С копией приказа ознакомлен',
                                            maxLength: 250,
                                            labelWidth: 125,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'Area',
                                            itemId: 'nfArea',
                                            fieldLabel: 'Площадь'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 0 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 180,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            flex: 2,
                                            name: 'DocumentPlace',
                                            itemId: 'tfDocumentPlace',
                                            fieldLabel: 'Место составления'
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
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Flat',
                                            itemId: 'tfFlat',
                                            fieldLabel: 'Квартира',
                                            maxLength: 10,
                                            labelAlign: 'right'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'actCheckWitnessGrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                },
                                {
                                    xtype: 'actCheckPeriodGrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Передано в прокуратуру',
                                    itemId: 'fsActResol',
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    name: 'ToProsecutor',
                                                    editable: false,
                                                    itemId: 'cbToPros',
                                                    fieldLabel: 'Передано в прокуратуру',
                                                    displayField: 'Display',
                                                    store: B4.enums.YesNoNotSet.getStore(),
                                                    valueField: 'Value'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateToProsecutor',
                                                    itemId: 'dfToPros',
                                                    fieldLabel: 'Дата передачи',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ResolutionProsecutor',
                                            itemId: 'sfResolPros',
                                            labelWidth: 200,
                                            anchor: '100%',
                                            labelAlign: 'right',
                                            fieldLabel: 'Постановление прокуратуры',
                                            editable: false,
                                            isGetOnlyIdProperty: false,
                                            textProperty: 'DocumentNumber',
                                            store: 'B4.store.DocumentGji',
                                            columns: [
                                                { xtype: 'datecolumn', dataIndex: 'DocumentDate', text: 'Дата', format: 'd.m.Y', width: 100 },
                                                { text: 'Номер', dataIndex: 'DocumentNumber', flex: 1 },
                                                {
                                                    text: 'Тип документа',
                                                    dataIndex: 'TypeDocumentGji',
                                                    flex: 1,
                                                    renderer: function(val) { return B4.enums.TypeDocumentGji.displayRenderer(val); }
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            itemId: 'actCheckActRemovalTab',
                            title: 'Акты проверки предписаний',
                            border: false,
                            hidden: true,
                            layout: 'fit',
                            items: [
                                {
                                    xtype: 'actCheckActRemovalGrid',
                                    bodyStyle: 'backrgound-color:transparent;'
                                }
                            ]
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            frame: true,
                            autoScroll: true,
                            itemId: 'actCheckViolationTab',
                            title: 'Результаты проверки',
                            border: false,
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'actcheckinspectionresultpanel',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    autoScroll: true,
                                    hidden: true,
                                    flex: 1
                                },
                                {
                                    xtype: 'actCheckRealityObjectEditPanel',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    hidden: true,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'actcheckprovideddocgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actCheckInspectedPartGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actCheckDefinitionGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actCheckAnnexGrid',
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Объединить акты',
                                    textAlign: 'left',
                                    itemId: 'btnMerge'
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