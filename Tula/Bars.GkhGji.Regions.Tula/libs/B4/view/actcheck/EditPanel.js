﻿Ext.define('B4.view.actcheck.EditPanel', {
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
        'B4.view.actcheck.RealityObjectGrid',
        'B4.view.actcheck.RealityObjectEditPanel',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeDocumentGji',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: 'form',
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        layout: 'hbox',
                        xtype: 'container'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '10px 15px 5px 15px',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                            xtype: 'container',
                            padding: '0 15px 20px 15px',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 10,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            items: [
                                {
                                    xtype: 'container',
                                    layout:  'hbox',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'actCheckInspectors',
                                            itemId: 'trigfInspectors',
                                            fieldLabel: 'Инспекторы',
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
                                    layout: {
                                        type: 'hbox',
                                        pack: 'end'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Акт направлен в прокуратуру',
                                            name: 'chbActToPres',
                                            labelAlign: 'right'
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
                            itemId: 'actCheckViolationTab',
                            title: 'Результаты проверки',
                            border: false,
                            layout: 'fit',
                            items: [
                                {
                                    xtype: 'actCheckRealityObjectGrid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    hidden: true
                                },
                                {
                                    xtype: 'actCheckRealityObjectEditPanel',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    hidden: true
                                }
                            ]
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
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    itemId: 'btnPrintActView'
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