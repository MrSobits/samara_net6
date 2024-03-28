Ext.define('B4.view.documentsgjiregister.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: 'documentsgjiregister.NavigationMenu',
    title: 'Реестр документов ГЖИ',
    itemId: 'docsGjiRegisterNavigationPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.MenuTreePanel',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.RealityObject',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'docsGjiRegisterInfoLabel'
                },
                {
                    id: 'docsGjiRegisterMenuTree',
                    xtype: 'menutreepanel',
                    hidden: true, //пункты меню переехали в выпадающий список
                    title: 'Разделы',
                    width: 250,
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'border'
                    },
                    region: 'center',
                    items: [
                        {
                            xtype: 'container',
                            anchor: '100%',
                            region: 'north',
                            border: true,
                            itemId: 'docsGjiRegisterFilterPanel',
                            height: 90,
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    itemId: 'cbTypeDocument',
                                    labelWidth: 130,
                                    fieldLabel: 'Тип документа',
                                    labelAlign: 'right',
                                    width: 500,
                                    operand: CondExpr.operands.eq,
                                    storeAutoLoad: false,
                                    editable: false,
                                    displayField: 'text',
                                    valueField: 'text',
                                    fields: ['moduleScript', 'text', 'options'],
                                    url: '/MenuGji/GetDocumentsGjiRegisterMenu',
                                    padding: '5 0 0 0'
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    width: 600,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        format: 'd.m.Y'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 130,
                                            fieldLabel: 'Период с',
                                            width: 290,
                                            itemId: 'dfDateStart',
                                            value: new Date(new Date().getFullYear(), 0, 1)
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 50,
                                            fieldLabel: 'по',
                                            width: 210,
                                            itemId: 'dfDateEnd',
                                            value: new Date()
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 0 0',
                                    anchor: '100%',
                                    width: 610,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        format: 'd.m.Y'
                                    },
                                    items: [
                                        {
                                            anchor: '100%',
                                            flex:1,
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                            itemId: 'sfRealityObject',
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.RealityObject',
                                            textProperty: 'Address',
                                            columns: [
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
                                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            editable: false,
                                            fieldLabel: 'Жилой дом'
                                        },
                                        {
                                            width: 10,
                                            xtype: 'component'
                                        },
                                        {
                                            width: 100,
                                            itemId: 'updateActiveGrid',
                                            xtype: 'b4updatebutton'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            region: 'center',
                            id: 'docsGjiRegisterMainTab'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
