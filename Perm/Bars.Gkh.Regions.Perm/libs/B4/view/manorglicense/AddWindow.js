Ext.define('B4.view.manorglicense.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorglicenseaddwindow',
    
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 200,
    maxHeight: 200,
    bodyPadding: 5,
    itemId: 'manOrgLicenseAddWindow',
    title: 'Заявление',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.manorglicense.ListManOrg',
        'B4.store.manorglicense.License',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.LicenseRequestType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
        {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateRequest',
                    allowBlank: false,
                    fieldLabel: 'Дата заявления',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4combobox',
                    name: 'Type',
                    items: B4.enums.LicenseRequestType.getItems(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Тип заявления',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    fieldLabel: 'Управляющая организация',
                    store: 'B4.store.manorglicense.ListManOrg',
                    editable: false,
                    allowBlank: false,
                    hidden: true,
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
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ManOrgLicense',
                    fieldLabel: 'Лицензия',
                    store: 'B4.store.manorglicense.License',
                    editable: false,
                    allowBlank: false,
                    hidden: true,
                    textProperty: 'LicNum',
                    columns: [
                        {
                            xtype: 'b4gridstatecolumn',
                            dataIndex: 'State',
                            text: 'Статус',
                            menuText: 'Статус',
                            width: 150,
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
                            }
                        },
                        { text: 'Номер лицензии', dataIndex: 'LicNum', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Муниципальное образование', dataIndex: 'ContragentMunicipality', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Краткое наименование УО', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
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