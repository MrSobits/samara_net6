Ext.define('B4.view.administration.notify.Window', {
    extend: 'B4.form.Window',
    alias: 'widget.notifywindow',

    requires: [
        'B4.enums.ButtonType'
    ],

    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 360,
    minHeight: 200,
    maxWidth: 720,
    maxHeight: 480,
    resizable: false,
    closable: false,

    title: 'Уведомление системы',
    icon: 'content/img/icons/error.png',

    buttonSet: 0,

    getBody: function () {
        return Ext.get('notify-body');
    },

    getMask: function () {
        return Ext.get('notify-mask');
    },

    getWindowBody: function () {
        return Ext.get('notify-window');
    },

    initComponent: function () {
        var me = this,
            body = me.getBody(),
            mask = me.getMask(),
            window = me.getWindowBody(),
            buttonSet = me.buttonSet || 1,
            buttonItems = [];

        Ext.each(B4.enums.ButtonType.getItemsMeta(),
            function(meta) {
                if ((buttonSet & meta.Value) !== 0) {
                    buttonItems.push(
                        {
                            xtype: 'button',
                            minWidth: 75,
                            action: meta.Name,
                            margin: '2 5',
                            text: meta.Display,
                            handler: me.onButtonClick,
                            style: {
                                background: 'LemonChiffon'
                            }
                        }
                    );
                }
            });

        Ext.apply(me, {
            renderTo: window,
            items: [
                {
                    xtype: 'panel',
                    padding: 5,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    overflowY: 'auto',
                    flex: 1,
                    html: me.text
                },
            ],
            dockedItems: [
                {
                    xtype: 'container',
                    dock: 'bottom',
                    layout: { type: 'hbox', pack: 'center' },
                    flex: 1,
                    items: buttonItems
                }
            ],
            listeners: {
                show: function () {
                    if (body.select('.x-window').getCount() === 1) {
                        window.setStyle('z-index', 1000000);
                        mask.show();
                    }
                },
                destroy: function () {
                    if (body.select('.x-window').getCount() === 1) {
                        window.setStyle('z-index', -1);
                        mask.hide();
                    }
                }
            }
        });

        me.callParent(arguments);
    },

    onButtonClick: function (button, event) {
        var id = button.up('window').messageId,
            button = B4.enums.ButtonType[button.action];

        Gkh.signalR.onNotifyButtonClick(id, button)
    }
});