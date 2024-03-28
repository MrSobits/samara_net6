Ext.define('B4.view.admincase.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.adminCaseEditPanel',
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'adminCaseEditPanel',
    title: 'Административное дело',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.store.Contragent',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhIntField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.admincase.DocGrid',
        'B4.view.admincase.AnnexGrid',
        'B4.view.admincase.ArticleLawGrid',
        'B4.view.admincase.ProvidedDocGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeDocumentGji',
        'B4.enums.TypeAdminCaseBase',
        'B4.view.admincase.RequirementGrid',
        'B4.ux.form.field.TabularTextArea',
        'B4.view.admincase.ViolationGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'panel',
                    layout: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    height: 80,
                    defaults: {
                        border: false,
                        labelWidth: 170,
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
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
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'admincaseTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            border: false,
                            frame: true,
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right',
                                        editable: false
                                    },
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            fieldLabel: 'Основание',
                                            name: 'TypeAdminCaseBase',
                                            store: B4.enums.TypeAdminCaseBase.getStore(),
                                            displayField: 'Display',
                                            valueField: 'Value'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Inspector',
                                            fieldLabel: 'Инспектор',
                                            store: 'B4.store.dict.Inspector',
                                            textProperty: 'Fio',
                                            columns: [
                                                { dataIndex: 'Fio', flex: 1, text: 'ФИО', filter: { xtype: 'textfield' } }
                                            ],
                                            labelWidth: 150
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ParentDocument',
                                            fieldLabel: 'Документ',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.RealityObject',
                                            textProperty: 'Address',
                                            editable: false,
                                            columns: [
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
                                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            name: 'RealityObject',
                                            fieldLabel: 'Объект недвижимости',
                                            allowBlank: false,
                                            labelWidth: 150
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Contragent',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'ShortName',
                                            fieldLabel: 'Контрагент',
                                            editable: false,
                                            columns: [
                                                { dataIndex: 'ShortName', text: 'Наименование', flex: 1, filter: { xtype: 'textfield' } },
                                                { dataIndex: 'Inn', text: 'ИНН', flex: 1, filter: { xtype: 'textfield' } },
                                                { dataIndex: 'Kpp', text: 'КПП', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'component',
                                            labelWidth: 150
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '20 0 0 0',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'textarea',
                                        labelAlign: 'right',
                                        maxLength: 2000
                                    },
                                    items: [
                                        {
                                            name: 'DescriptionQuestion',
                                            fieldLabel: 'Вопрос'
                                        },
                                        {
                                            xtype: 'tabtextarea',
                                            name: 'DescriptionSet',
                                            fieldLabel: 'Установил',
                                            readOnly: true
                                        }
                                        /*{
                                            name: 'DescriptionDefined',
                                            fieldLabel: 'Определил'
                                        }*/
                                    ]
                                }
                            ]
                        },
                        { xtype: 'admincaseViolationGrid', flex: 1 },
                        { xtype: 'admincasearticlelawgrid', flex: 1 },
                        { xtype: 'admincasedocgrid', flex: 1 },
                        { xtype: 'admincaseprovdocgrid', flex: 1 },
                        { xtype: 'admincaserequirementgrid', flex: 1 },
                        { xtype: 'admincaseannexgrid', flex: 1 }
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