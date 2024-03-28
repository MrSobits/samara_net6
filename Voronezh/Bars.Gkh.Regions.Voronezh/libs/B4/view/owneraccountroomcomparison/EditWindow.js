Ext.define('B4.view.owneraccountroomcomparison.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.Panel'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 750,
    height: 450,
    itemId: 'owneraccountroomcomparisonEditWindow',
    title: 'Собственник',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'vbox',
                    defaults: {
                        xtype: 'textfield',
                        //     margin: '10 0 5 0',
                        labelWidth: 150,
                        labelAlign: 'right',
                        allowBlank: true,
                        editable: true,
                        width: 700
                    },
                    items: [

                       {
                           xtype: 'container',
                           layout: 'hbox',
                           defaults: {
                               xtype: 'combobox',
                               //     margin: '10 0 5 0',
                               labelWidth: 150,
                               labelAlign: 'right',
                           },
                           items: [
                                   {
                                       xtype: 'textfield',
                                       name: 'AddressContent',
                                       fieldLabel: 'Адрес в выписке',
                                       flex: 1,
                                       maxLength: 20,
                                       regex: /^(\d{10})$/
                                   },
                                   {
                                       xtype: 'textfield',
                                       name: 'ROAddress',
                                       fieldLabel: 'Адрес в системе',
                                       disabled: false,
                                       flex: 1,
                                       maxLength: 20
                                   }

                           ]
                       },

                    ]
                },
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