Ext.define('B4.view.RoomNumsEditWindow', {
    extend: 'B4.form.Window',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    minHeight: 150,
    bodyPadding: 5,
    alias: 'widget.roomnumseditwindow',
    title: 'Добавление квартир',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    readOnly: true,
                    name: 'Address',
                    fieldLabel: 'Адрес',
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'RoomNums',
                    fieldLabel: 'Квартиры',
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
                            columns: 1,
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