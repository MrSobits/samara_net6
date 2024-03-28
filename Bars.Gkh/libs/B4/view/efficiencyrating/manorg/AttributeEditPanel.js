Ext.define('B4.view.efficiencyrating.manorg.AttributeEditPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.ux.button.Save'
    ],

    title: 'Настройка параметров',
    alias: 'widget.efAttributeEditPanel',
    closable: false,

    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
                trackResetOnLoad: true
            },
            me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    name: 'dynamicForm',
                    border: null,
                    layout: { type: 'vbox', align: 'stretch' },
                    bodyPadding: 10,
                    margin: '0 5px 0 5px',
                    bodyStyle: Gkh.bodyStyle,
                    defaults: { flex: 1 }
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});