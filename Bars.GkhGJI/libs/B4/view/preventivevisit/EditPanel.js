Ext.define('B4.view.preventivevisit.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'preventivevisitEditPanel',
    title: 'Акт профилактического визита',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.enums.KindKND',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.preventivevisit.WitnessGrid',
        'B4.view.preventivevisit.PeriodGrid',
        'B4.view.preventivevisit.ResultGrid',
        'B4.view.preventivevisit.AnnexGrid',
        'B4.view.preventivevisit.RealityObjectGrid',
        'B4.enums.YesNoNotSet',
        'B4.enums.PersonInspection',
        'B4.enums.TypeDocumentGji'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ERKNMID',
                                    itemId: 'dfERKNMID',
                                    fieldLabel: 'Номер в ЕРКНМ',
                                    allowBlank: true,
                                    flex: 1,
                                    readOnly: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'button',
                                    text: 'Разместить в ЕРКНМ',
                                    name: 'calculateButton',
                                    tooltip: 'Разместить в ЕРКНМ',
                                    action: 'ERKNMRequest',
                                    iconCls: 'icon-accept',
                                    itemId: 'calculateButton'
                                }
                            ]
                        },
                        {
                            layout: 'hbox',
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    width: 200,
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
                                    hideTrigger: true
                                },
                                {
                                    width: 295,
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
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
                    itemId: 'preventivevisitTabPanel',
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
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Реквизиты',
                            border: false,
                            itemId: 'requsitePanel',
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            name: 'KindKND',
                                            fieldLabel: 'Вид контроля (надзора)',
                                            displayField: 'Display',
                                            itemId: 'cbKindKND',
                                            flex: 0.5,
                                            store: B4.enums.KindKND.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                            readOnly: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4combobox',
                                            name: 'PersonInspection',
                                            fieldLabel: 'Тип проверяемого лица',
                                            displayField: 'Display',
                                            labelWidth: 120,
                                            flex: 0.5,
                                            itemId: 'cbPersonInspection',
                                            store: B4.enums.PersonInspection.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                            readOnly: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            fieldLabel: 'Место составления акта',
                                            name: 'ActAddress',
                                            flex:1,
                                            labelWidth: 120,
                                            maxLength: 300
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Contragent',
                                            textProperty: 'ShortName',
                                            fieldLabel: 'Юридическое лицо',
                                            store: 'B4.store.Contragent',
                                            flex: 1,
                                            editable: false,
                                            itemId: 'sfContragent',
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                {
                                                    text: 'Муниципальное образование',
                                                    dataIndex: 'Municipality',
                                                    flex: 1,
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
                                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        }
                                    ]
                                },                             
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margin: '5 0 5 0',
                                    defaults: {
                                        allowBlank: true,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            itemId: 'tfPhysicalPerson',
                                            name: 'PhysicalPerson',
                                            flex: 1,
                                            fieldLabel: 'Физ. лицо',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'textfield',
                                            itemId: 'tfPhysicalPersonInfo',
                                            flex: 1,
                                            name: 'PhysicalPersonInfo',
                                            labelWidth: 140,
                                            fieldLabel: 'Реквизиты физ. лица',
                                            maxLength: 1500
                                        },
                                        {
                                            xtype: 'textfield',
                                            itemId: 'tfPhysicalPersonINN',
                                            flex: 1,
                                            name: 'PhysicalPersonINN',
                                            labelWidth: 90,
                                            fieldLabel: 'ИНН физлица',
                                            maxLength: 15
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    margin: '5 0 5 0',
                                    defaults: {
                                        allowBlank: true,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            itemId: 'tfPhysicalPersonAddress',
                                            name: 'PhysicalPersonAddress',
                                            flex: 1,
                                            fieldLabel: 'Адрес физ. лица',
                                            maxLength: 1500
                                        }
                                    ]
                                },
                                {
                                    width: 10,
                                    xtype: 'component'
                                },
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
                                            name: 'preventivevisitInspectors',
                                            itemId: 'trigFInspectors',
                                            fieldLabel: 'Инспекторы',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    margin: '5 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            itemId: 'cbUsedDistanceTech',
                                            flex: 0.5,
                                            name: 'UsedDistanceTech',
                                            fieldLabel: 'Дистанционное',
                                            disabled: false,
                                            editable: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DistanceCheckDate',
                                            itemId: 'dfDistanceCheckDate',
                                            fieldLabel: 'Дата дистанционного ПМ',
                                            flex: 1,
                                            format: 'd.m.Y',
                                            allowBlank: true,
                                            labelWidth: 130,
                                        },
                                        {
                                            xtype: 'textfield',
                                            fieldLabel: 'Способ дистанционного ПМ',
                                            name: 'DistanceDescription',
                                            flex: 2,
                                            labelWidth: 130,
                                            allowBlank: true,
                                            maxLength: 1500
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    margin: '5 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [    
                                        {
                                            xtype: 'textfield',
                                            name: 'VideoLink',
                                            itemId: 'tfDistanceCheckDate',
                                            fieldLabel: 'Ccылка',
                                            flex: 1,
                                            allowBlank: true,
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Просмотр',
                                            name: 'viewButton',
                                            tooltip: 'Посмотреть видео мериприятия',
                                            action: 'ViewVideo',
                                            iconCls: 'icon-accept',
                                            itemId: 'viewButton'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'preventivevisitwitnessgrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                },
                                {
                                    xtype: 'preventivevisitperiodgrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                },
                                {
                                    xtype: 'preventivevisitrealityobjectgrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                }
                            ]
                        },
                        {
                            xtype: 'preventivevisitannexgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'preventivevisitresultgrid',
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