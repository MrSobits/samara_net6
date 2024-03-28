Ext.define('B4.view.presentation.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'presentationEditPanel',
    title: 'Представление',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Inspector',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.presentation.AnnexGrid',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.ux.form.field.TabularTextArea'
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
                            padding: '0 15px 25px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
                                    width: 200
                                },
                                {
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
                                    width: 295
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
                                    width: 295
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    itemId: 'presentationTabPanel',
                    flex: 1,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'presentationBaseName',
                                    itemId: 'presentationBaseNameTextField',
                                    fieldLabel: 'Документ-основание',
                                    readOnly: true,
                                    labelWidth: 140
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 130,
                                        labelAlign: 'right'
                                    },
                                    title: 'Кем вынесено',
                                    items: [
                                        {
                                            xtype: 'combobox', editable: false,
                                            floating: false,
                                            name: 'TypeInitiativeOrg',
                                            fieldLabel: 'Кем вынесено',
                                            displayField: 'Display',
                                            store: B4.enums.TypeInitiativeOrgGji.getStore(),
                                            valueField: 'Value',
                                            itemId: 'cbTypeInitiativeOrgpresentation'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            textProperty: 'Fio',
                                            name: 'Official',
                                            fieldLabel: 'Должностное лицо',
                                            itemId: 'sfPresentationOfficial',
                                            editable: false,
                                            columns: [
                                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                                                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1 }
                                            ]
                                        }
                                    ]
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
                                            fieldLabel: 'Тип контрагента',
                                            fields: ['Id', 'Name', 'Code'],
                                            url: '/ExecutantDocGji/List',
                                            queryMode: 'local',
                                            triggerAction: 'all'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                labelWidth: 130,
                                                labelAlign: 'right',
                                                disabled: true,
                                                flex: 1
                                            },
                                            items: [
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
                                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPerson',
                                                    fieldLabel: 'Физическое лицо',
                                                    itemId: 'tfPhysPerson',
                                                    maxLength: 500
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'PhysicalPersonInfo',
                                            fieldLabel: 'Реквизиты физ. лица',
                                            itemId: 'taPhysPersonInfo',
                                            disabled: true,
                                            maxLength: 500
                                        },
                                        {
                                           xtype: 'textfield',
                                           name: 'ExecutantPost',
                                           fieldLabel: 'Должность',
                                           maxLength: 200
                                        }
                                    ]
                                }

                            ]
                        },
                        {
                            xtype: 'presentationAnnexGrid',
                            flex: 1
                        },
                        {
                            layout: 'anchor',
                            title: 'Требование',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                    {
                                        xtype: 'tabtextarea',
                                        name: 'DescriptionSet',
                                        fieldLabel: 'Требование',
                                        readOnly: false,
                                        height: 300
                                    }
                            ]
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