Ext.define('B4.controller.dict.CategoryPosts', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.CategoryPosts', 'dict.MessageSubject'],
    stores: ['dict.CategoryPosts', 'dict.MessageSubject'],
    views: ['dict.categoryposts.CategoryPostsGrid', 'dict.categoryposts.MessageSubjectGrid', 'dict.categoryposts.CategoryPosts'],

    mainView: 'dict.categoryposts.CategoryPosts',
    mainViewSelector: 'categorypostsPanel',

    refs: [{
        ref: 'categoryView',
        selector: 'categorypostsgrid'
    },
    {
        ref: 'messageView',
        selector: 'messagesubjectgrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'categoryPostsGridAspect',
            storeName: 'dict.CategoryPosts',
            modelName: 'dict.CategoryPosts',
            gridSelector: 'categorypostsgrid'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'messageSubjectGridAspect',
            storeName: 'dict.MessageSubject',
            modelName: 'dict.MessageSubject',
            gridSelector: 'messagesubjectgrid',
            listeners: {
                beforesave: function (asp, recs) {
                    var me = this;
                    var categoryId = me.controller.getContextValue('categoryId');
                    recs.each(function (rec) { rec.set('CategoryPosts', categoryId); });
                }
            }

        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
            { name: 'Gkh.Dictionaries.CategoryPosts.Create', applyTo: 'b4addbutton', selector: 'categorypostsPanel' },
            { name: 'Gkh.Dictionaries.CategoryPosts.Edit', applyTo: 'b4savebutton', selector: 'categorypostsPanel' }]
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('categorypostsPanel');
        this.bindContext(view);
        this.application.deployView(view);
        view.down('categorypostsgrid').getStore().load();
    },

    init: function () {
        var me = this;

        me.control({
            'categorypostsgrid': {
                itemclick: me.onCategorySelect
            },
            'messagesubjectgrid': {
                render: {
                    fn: me.onMessageGridRender,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    onCategorySelect: function (view, node) {
        var me = this,
            grid = me.getMessageView(),
            store = grid.getStore();
        grid.setDisabled(false);
        me.setContextValue('categoryId', node.get('Id'));

        store.load();
    },

    onMessageGridRender: function (grid) {
        var me = this;
        grid.getStore().on('beforeload', function (store, operation) {
            operation.params = operation.params || {};
            operation.params.categoryId = me.getContextValue('categoryId');
        });
    }
});