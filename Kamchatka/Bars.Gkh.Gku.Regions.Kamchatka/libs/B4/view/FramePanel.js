Ext.define('B4.view.FramePanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.framepanel',
    layout: 'fit',
    closable: true,
    src: '',

    initComponent: function () {
        var me = this;
        
        if (this.src) {
            Ext.applyIf(me, {
                items: [
                    {
                        xtype: "component",
                        autoEl: {
                            tag: "iframe",
                            src: me.src
                        }
                    }
                ]
            });
        }

        me.callParent(arguments);
    }
});