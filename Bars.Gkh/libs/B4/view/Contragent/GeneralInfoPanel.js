Ext.define('B4.view.contragent.GeneralInfoPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.contragentgeneralinfopanel',
    title: 'Общие сведения',

    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoScroll: true,

    requires: [
        'B4.view.contragent.EditPanel',
        'B4.view.contragent.AdditionRoleGrid',
        'B4.ux.grid.EntityChangeLogGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [
                        {
                            xtype: 'contragentEditPanel',
                            closable: false,
                            autoScroll: true
                        },
                        {
                            xtype: 'contragentAdditionRoleGrid',
                            closable: false,
                            autoScroll: true
                        },
                        {
                             xtype: 'entitychangeloggrid',
                             autoScroll: true
                        }
                    ]
                }]
        });

        me.callParent(arguments);
    }
});