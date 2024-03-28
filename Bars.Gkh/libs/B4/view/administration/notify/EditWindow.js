Ext.define('B4.view.administration.notify.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.notifyeditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.administration.notify.EditPanel',
        'B4.view.administration.notify.PermissionGrid',
        'B4.view.administration.notify.StatsGrid'
    ],

    modal: true,
    maximizable: true,
    bodyStyle: Gkh.bodyStyle,

    width: 720,
    height: 500,
    border: false,

    title: 'Уведомление пользователей',
    trackResetOnLoad: true,
    layout: 'fit',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                bodyStyle: Gkh.bodyStyle
            },
            items: [
                {
                    xtype: 'tabpanel',
                    items: [
                        {
                            xtype: 'notifyeditpanel',
                            bodyPadding: 5
                        },
                        {
                            xtype: 'notifypermissiongrid'
                        },
                        {
                            xtype: 'notifystatsgrid'
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