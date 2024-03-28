Ext.define('B4.view.rosregextract.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.Panel',
        'B4.form.SelectField',
        'B4.view.rosregextract.OwnerGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'rosregextractEditWindow',
    title: 'Просмотр выписки',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 100,
                        margin: '5 0 5 0',
                        labelAlign: 'right',
                        //    flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Id',
                            flex: 0.5,
                            fieldLabel: 'Id',
                            hideTrigger: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'CadastralNumber',
                            flex: 1,
                            fieldLabel: 'Кадастровый номер'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 100,
                        margin: '5 0 5 0',
                        labelAlign: 'right'
                        //    flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ExtractDate',
                            flex: 0.5,
                            fieldLabel: 'Дата выписки'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ExtractNumber',
                            flex: 1,
                            fieldLabel: 'Номер выписки'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 100,
                        margin: '5 0 5 0',
                        labelAlign: 'right',
                        //    flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Address',
                            flex: 3,
                            fieldLabel: 'Адрес'
                        },
                        {
                            xtype: 'textfield',
                            name: 'RoomArea',
                            flex: 1,
                            fieldLabel: 'Площадь'
                        }
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
                            xtype: 'rosregextractownergrid',
                            flex: 1
                        }
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