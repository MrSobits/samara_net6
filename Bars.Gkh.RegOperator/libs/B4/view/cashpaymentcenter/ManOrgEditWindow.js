Ext.define('B4.view.cashpaymentcenter.ManOrgEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.cashpaymentcentermanorgeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minHeight: 500,
    maxHeight: 500,
    width: 818,
    minWidth: 818,
    bodyPadding: 5,
    title: 'Договор с объектами',
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.ManagingOrganization',
        'B4.view.cashpaymentcenter.ManOrgRealObjGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    padding: '5 5 5 5',
                    defaults: {
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        }
                    },
                    items: [
                        {
                            xtype: 'container',
                            defaults: {
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'NumberContract',
                                    allowBlank: false,
                                    labelWidth: 170,
                                    flex: 1,
                                    fieldLabel: 'Номер договора'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateStart',
                                    allowBlank: false,
                                    labelWidth: 220,
                                    flex: 1,
                                    fieldLabel: 'Дата начала действия договора',
                                    format: 'd.m.Y'
                                }
                                
                            ]
                        },
                        {
                            xtype: 'container',
                            defaults: {
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateContract',
                                    allowBlank: false,
                                    labelWidth: 170,
                                    flex: 1,
                                    fieldLabel: 'Дата договора',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateEnd',
                                    labelWidth: 220,
                                    flex: 1,
                                    fieldLabel: 'Дата окончания действия договора',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            defaults: {
                                labelAlign: 'right'
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ManOrg',
                                    textProperty: 'ContragentName',
                                    fieldLabel: 'Управляющая организация',
                                    store: 'B4.store.ManagingOrganization',
                                    allowBlank: false,
                                    editable: false,
                                    labelAlign: 'right',
                                    labelWidth: 170,
                                    flex: 1,
                                    columns: [
                                        { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'cashpaymentcentermanorgrealobjgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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