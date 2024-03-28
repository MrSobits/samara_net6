Ext.define('B4.view.dict.tariff.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.tariffdicteditwin',
    mixins: ['B4.mixins.window.ModalMask'],

    width: 820,
    height: 462,
    border: false,
    layout: 'fit',
    resizeHandles: 's',

    title: 'Добавление тарифа ЖКУ',

    requires: [
        'B4.view.dict.tariff.EditPanel',
        'B4.ux.grid.EntityChangeLogGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'tariffdicteditpanel'
                        },
                        {
                            xtype: 'entitychangeloggrid',
                            autoScroll: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    setTitleAction: function (actionName) {
        var me = this;

        me.setTitle(actionName + ' тарифа ЖКУ');
    }
});