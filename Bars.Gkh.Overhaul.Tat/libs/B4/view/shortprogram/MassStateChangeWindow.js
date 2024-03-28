Ext.define('B4.view.shortprogram.MassStateChangeWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.shortprogramstatechangewin',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minHeight: 150,
    maxHeight: 150,
    width: 550,
    minWidth: 550,
    bodyPadding: 5,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    itemId: 'cbCurrentState',
                    url: '/State/GetListByType',
                    fields: ['Id', 'Name'],
                    fieldLabel: 'Текущий статус',
                    editable: false
                },
                {
                    xtype: 'b4combobox',
                    itemId: 'cbNextState',
                    url: '/StateTransfer/GetStates',
                    fields: ['Id', 'Name'],
                    fieldLabel: 'Новый статус',
                    padding: '0 5 0 0',
                    disabled: true,
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'RealObjs',
                    disabled: true,
                    allowBlank: false,
                    fieldLabel: 'Жилые дома'
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