Ext.define('B4.view.supplyresourceorg.ContractEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.supplyresorgcontracteditwindow',

    requires: [
        'B4.form.SelectField',
        'B4.view.realityobj.Grid',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.supplyresourceorg.BySupplyResOrg',
        'B4.form.FileField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closable: 'hide',
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 800,
    maxWidth: 800,
    width: 800,
    height: 250,
    bodyPadding: 3,
    
    title: 'Договор с жилым домом',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 160
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    itemId: 'sfRealityObject',
                    fieldLabel: 'Жилой дом',
                    labelWidth: 150,
                   

                    store: 'B4.store.supplyresourceorg.BySupplyResOrg',
                    textProperty: 'Address',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Муниципальный район',
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
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ContractNumber',
                            fieldLabel: 'Номер',
                            labelWidth: 150,
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'ContractDate',
                            fieldLabel: 'от',
                            format: 'd.m.Y',
                            maxWidth: 150,
                            labelWidth: 50
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelAlign: 'right',
                        flex: 1,
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            name: 'DateStart',
                            fieldLabel: 'Дата начала',
                            labelWidth: 150,
                            allowBlank: false
                        },
                        {
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания',
                            labelWidth: 170
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    fieldLabel: 'Файл',
                    labelWidth: 150
                },
                {
                    xtype: 'textarea',
                    name: 'Note',
                    fieldLabel: 'Примечание',
                    height: 60,
                    labelWidth: 150,
                    maxLength: 300
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});