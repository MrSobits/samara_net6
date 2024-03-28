Ext.define('B4.view.protocolmhc.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'protocolMhcEditPanel',
    title: 'Протокол МЖК',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.protocolmhc.AnnexGrid',
        'B4.view.protocolmhc.ArticleLawGrid',
        'B4.view.protocolmhc.RealityObjectGrid',
        'B4.view.protocolmhc.DefinitionGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeDocumentGji'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        border: false,
                        labelWidth: 170,
                        xtype: 'panel',
                        layout: 'hbox',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '10px 15px 5px 15px',
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
                                    labelWidth: 80,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    readOnly: true,
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                }
                            ]
                        },
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '0 15px 20px 15px',
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 80,
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
                    itemId: 'protocolMhcTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.Municipality',
                                    name: 'Municipality',
                                    labelWidth: 280,
                                    fieldLabel: 'Орган МЖК, оформивший протокол',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ],
                                    itemId: 'sfMunicipality'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 280,
                                    name: 'DateSupply',
                                    fieldLabel: 'Дата поступления в ГЖИ',
                                    format: 'd.m.Y',
                                    itemId: 'dfDateSupply',
                                    maxWidth: 400
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right'
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
                                            disabled: true,
                                            editable: false,
                                            columns: [
                                                {
                                                    header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                disabled: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPerson',
                                                    fieldLabel: 'Физическое лицо',
                                                    itemId: 'tfPhysPerson',
                                                    maxLength: 300,
                                                    labelWidth: 120
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'PhysicalPersonInfo',
                                                    fieldLabel: 'Реквизиты физ. лица',
                                                    itemId: 'taPhysPersonInfo',
                                                    maxLength: 500,
                                                    labelWidth: 150
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        { xtype: 'protocolMhcArticleLawGrid', flex: 1 },
                        { xtype: 'protocolMhcRealityObjectGrid', flex: 1 },
                        { xtype: 'protocolMhcDefinitionGrid', flex: 1 },
                        { xtype: 'protocolMhcAnnexGrid', flex: 1 }
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