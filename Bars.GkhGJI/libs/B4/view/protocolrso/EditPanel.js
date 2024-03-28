Ext.define('B4.view.protocolrso.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'protocolRSOEditPanel',
    title: 'Протокол РСО',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.enums.TypeSupplierProtocol',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.protocolrso.AnnexGrid',
        'B4.view.protocolrso.ArticleLawGrid',
        'B4.view.protocolrso.RealityObjectGrid',
        'B4.view.protocolrso.DefinitionGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeDocumentGji'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocUIN',
                                    name: 'DocUIN',
                                    readOnly: true,
                                    fieldLabel: 'УИН',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 350
                                }
                            ]
                        },
                        {
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                    itemId: 'protocolRSOTabPanel',
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
                                      xtype: 'combobox', editable: false,
                                      name: 'TypeSupplierProtocol',
                                      fieldLabel: 'Тип РСО',
                                      allowBlank: false,
                                      disabled: true,
                                      displayField: 'Display',
                                      store: B4.enums.TypeSupplierProtocol.getStore(),
                                      valueField: 'Value'
                                  },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    name: 'GasSupplier',
                                    labelWidth: 280,
                                    fieldLabel: 'РСО предоставившая сведения',
                                    editable: false,
                                    allowBlank: false,
                                    disabled: true,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ],
                                    itemId: 'sfSupplierGas'
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
                        { xtype: 'protocolRSOArticleLawGrid', flex: 1 },
                        { xtype: 'protocolRSORealityObjectGrid', flex: 1 },
                        { xtype: 'protocolRSODefinitionGrid', flex: 1 },
                        { xtype: 'protocolRSOAnnexGrid', flex: 1 }
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