Ext.define('B4.view.constructionobject.smr.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.constructionobjsmreditpanel',
    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    title: 'Мониторинг СМР',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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