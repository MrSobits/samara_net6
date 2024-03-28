Ext.define('B4.view.gisGkh.SignWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.gisGkhSignWindow',

    requires: [
        'B4.mixins.MaskBody',
        'B4.form.SelectField'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    closeAction: 'close',
    modal: true,
    rec: undefined,
    signer: undefined,
    cadesplugin: undefined,

    layout: {
        type: 'fit',
        align: 'stretch'
    },

    width: 800,
    height: 100,

    title: 'Подписать запрос',

    maximizable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            xtype: 'container',
            layout: 'hbox',
            items: [
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            xtype: 'combo',
                            flex: 1,
                            editable: false,
                            fieldLabel: 'Выберите ЭП-СП',
                            queryMode: 'local',
                            itemId: 'dfCert',
                            displayField: 'SubjectName',
                            valueField: 'Certificate',
                            emptyItem: { SubjectName: '-' },
                        },
                        {
                            xtype: 'button',
                            text: 'Подписать и отправить',
                            iconCls: 'icon-accept',
                            handler: function (b) {
                                var win = b.up('gisGkhSignWindow');
                                win.fireEvent('createsignature', win);
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    },
});