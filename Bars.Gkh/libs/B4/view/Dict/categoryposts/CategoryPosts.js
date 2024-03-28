Ext.define('B4.view.dict.categoryposts.CategoryPosts', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.view.dict.categoryposts.CategoryPostsGrid',
        'B4.view.dict.categoryposts.MessageSubjectGrid'
    ],

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    alias: 'widget.categorypostsPanel',
    title: 'Категории сообщений',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'categorypostsgrid',
                border: false,
                flex: 2
            },
            {
                xtype: 'messagesubjectgrid',
                style: 'border-left: solid #99bce8 1px',
                border: false,
                disabled: true,
                flex: 3
            }]
        });

        me.callParent(arguments);
    }
});