Ext.define('B4.view.realityobj.room.EntranceWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minWidth: 500,
    height: 500,
    bodyPadding: 5,
    
    title: 'Выбор подъезда',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    alias: 'widget.entranceselectwindow',

    requires: [
        'B4.form.FiasSelectAddress',

        'B4.enums.TypeHouse',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.realityobj.room.EntranceGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'entrancegrid',
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
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Выбрать'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        'click': function() {
                                            me.close();
                                        }
                                    }
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