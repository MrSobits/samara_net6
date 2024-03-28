Ext.define('B4.view.dateareaowner.EditWindow', {
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
    itemId: 'dateareaownerEditWindow',
    title: 'dateareaowner',
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
                            xtype: 'textfield',
                            name: 'ID_Object',
                            fieldLabel: 'ID_Object',
                        },
                        {
                            xtype: 'textfield',
                            name: 'Area',
                            fieldLabel: 'Area',
                        },
                        {
                            xtype: 'textfield',
                            name: 'Floor',
                            fieldLabel: 'Floor',
                        },
                        {
                            xtype: 'textfield',
                            name: 'CityName',
                            fieldLabel: 'CityName',
                        },
                        {
                            xtype: 'textfield',
                            name: 'StreetName',
                            fieldLabel: 'StreetName',
                        },
                        {
                            xtype: 'textfield',
                            name: 'Level1Name',
                            fieldLabel: 'Level1Name',
                        },
                        {
                            xtype: 'textfield',
                            name: 'ApartmentName',
                            fieldLabel: 'ApartmentName',
                        },
                        {
                            xtype: 'textfield',
                            name: 'ID_Subject',
                            fieldLabel: 'ID_Subject',
                        },
                        {
                            xtype: 'textfield',
                            name: 'Surname',
                            fieldLabel: 'Surname',
                        },
                        {
                            xtype: 'textfield',
                            name: 'FirstName',
                            fieldLabel: 'FirstName',
                        },
                        {
                            xtype: 'textfield',
                            name: 'Patronymic',
                            fieldLabel: 'Patronymic',
                        },
                        {
                            xtype: 'textfield',
                            name: 'ID_Record',
                            fieldLabel: 'ID_Record',
                        },
                        {
                            xtype: 'textfield',
                            name: 'RegNumber',
                            fieldLabel: 'RegNumber',
                        },
                        {
                            xtype: 'textfield',
                            name: 'RegDate',
                            fieldLabel: 'RegDate',
                        }

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