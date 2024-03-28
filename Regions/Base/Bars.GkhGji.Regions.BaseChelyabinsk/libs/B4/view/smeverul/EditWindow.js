Ext.define('B4.view.smeverul.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.manorglicense.License',
        'B4.form.SelectField',
        'B4.form.GridStateColumn',
        'B4.view.smeverul.FileInfoGrid',
        'B4.enums.InnOgrn',
        'B4.form.EnumCombo',
        'B4.enums.RequestState'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 800,
    bodyPadding: 10,
    itemId: 'smeverulEditWindow',
    title: 'Запрос в ЕРУЛ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
             {
                 xtype: 'tabpanel',
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
                         defaults: {
                             labelWidth: 100,
                             margin: '5 0 5 0',
                             align: 'stretch',
                             labelAlign: 'right'
                         },
                         bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                         title: 'Форма запроса',
                         border: false,
                         bodyPadding: 10,
                         items: [
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 250,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты субъекта запроса',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                         
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ManOrgLicense',
                                            textProperty: 'LicNumber',
                                            itemId: 'dfManOrgLicenseRequest',
                                            fieldLabel: 'Лицензия',
                                            store: 'B4.store.manorglicense.License',
                                            flex: 1,
                                            editable: false,
                                            allowBlank: true,
                                            hidden: false,
                                            columns: [
                                                {
                                                    xtype: 'b4gridstatecolumn',
                                                    dataIndex: 'State',
                                                    text: 'Статус',
                                                    menuText: 'Статус',
                                                    width: 120,
                                                    filter: {
                                                        xtype: 'b4combobox',
                                                        url: '/State/GetListByType',
                                                        editable: false,
                                                        storeAutoLoad: false,
                                                        operand: CondExpr.operands.eq,
                                                        listeners: {
                                                            storebeforeload: function (field, store, options) {
                                                                options.params.typeId = 'gkh_manorg_license';
                                                            },
                                                            storeloaded: {
                                                                fn: function (me) {
                                                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                                                    me.select(me.getStore().data.items[0]);
                                                                }
                                                            }
                                                        }
                                                    },
                                 
                                                    scope: this
                                                },
                                                { text: 'Номер ГЖИ', dataIndex: 'LicNumber', flex: 0.5, filter: { xtype: 'textfield' } },
                                                { text: 'Номер ЕРУЛ', dataIndex: 'ERULNumber', flex: 0.5, filter: { xtype: 'textfield' } },
                                                { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата', dataIndex: 'DateIssued', flex: 0.5, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                { text: 'Контрагент', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            anchor: '100%',
                                            fieldLabel: 'Тип запроса',
                                            enumName: 'B4.enums.ERULRequestType',
                                            name: 'ERULRequestType'
                                        }
                                    ]
                                },

                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            border: false,
                            flex: 1,
                            defaults: {
                                border: false
                            },
                            items: [
                            {
                                xtype: 'smeverulfileinfogrid',
                                flex: 1
                            }
                            ]
                        }
                         ]
                     },
                    
                 ]
             }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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