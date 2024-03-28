Ext.define('B4.view.prescription.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'prescriptionEditPanel',
    title: 'Предписание',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.prescription.AnnexGrid',
        'B4.view.prescription.CancelGrid',
        'B4.view.prescription.ArticleLawGrid',
        'B4.view.prescription.RealityObjListPanel',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.prescription.ClosePanel'
    ],

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        border: false,
                        labelWidth: 170,
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
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
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
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 50,
                                    width: 200
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
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    itemId: 'prescriptionTabPanel',
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 5,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 140
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'prescriptionBaseName',
                                    itemId: 'prescriptionBaseNameTextField',
                                    fieldLabel: 'Документ-основание',
                                    readOnly: true
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'prescriptionInspectors',
                                    itemId: 'prescriptionInspectorsTrigerField',
                                    fieldLabel: 'Инспектор',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Примечание',
                                    itemId: 'taDescriptionPrescription',
                                    maxLength: 2000
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    title: 'Документ выдан',
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            itemId: 'cbExecutant',
                                            name: 'Executant',
                                            allowBlank: false,
                                            editable: false,
                                            fieldLabel: 'Тип исполнителя',
                                            fields: ['Id', 'Name', 'Code'],
                                            url: '/ExecutantDocGji/List',
                                            queryMode: 'local',
                                            triggerAction: 'all'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'ShortName',
                                            name: 'Contragent',
                                            fieldLabel: 'Контрагент',
                                            itemId: 'sfContragent',
                                            editable: false,
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
                                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 130,
                                                disabled: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPerson',
                                                    fieldLabel: 'Физическое лицо',
                                                    itemId: 'tfPhysPerson',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'PhysicalPersonInfo',
                                                    fieldLabel: 'Реквизиты физ. лица',
                                                    itemId: 'taPhysPersonInfo',
                                                    maxLength: 500
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'prescriptionRealObjListPanel',
                            flex: 1
                        },
                        {
                            itemId: 'prescriptionArticleLawTab',
                            layout: 'fit',
                            title: 'Статьи закона',
                            items: [
                                {
                                    xtype: 'prescriptionArticleLawGrid',
                                    border: false
                                }
                            ],
                            flex: 1
                        },
                        {
                            xtype: 'prescriptionCancelGrid',
                            flex: 1
                        },
                        {
                            xtype: 'prescrclosepanel',
                            tabConfig: {
                                hidden: true
                            },
                            flex: 1
                        },
                        {
                            xtype: 'prescriptionAnnexGrid',
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