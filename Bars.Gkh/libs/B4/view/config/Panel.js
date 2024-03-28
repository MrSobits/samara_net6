Ext.define('B4.view.config.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.configpanel',

    closable: true,
    title: 'Единые настройки приложения',

    layout: {
        type: 'border'
    },

    border: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'confignavigationpanel',
                    region: 'west',
                    border: false,
                    width: 300,
                    split: true
                },
                {
                    xtype: 'panel',
                    layout: 'fit',
                    region: 'center',
                    itemId: 'wrapperPanel',
                    disabled: true,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'form',
                            unstyled: true,
                            border: false,
                            autoScroll: true,
                            itemId: 'dynamicPanel',
                            defaults: {
                                anchor: '100%',
                                margin: '0 0 10 0',
                                labelWidth: 250,
                                labelAlign: 'right'
                            },
                            bodyPadding: '5 5 0 5'
                        }
                    ],
                    tbar: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    name: 'configpanelb4savebutton'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    name: 'configpanelb4updatebutton'
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