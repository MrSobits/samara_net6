Ext.define('B4.controller.dict.GroupType', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.view.dict.grouptype.Grid',
        'B4.store.dict.GroupType',
        'B4.model.dict.GroupType',
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    stores: [
        'dict.GroupType'
    ],
    
    models: [
        'dict.GroupType'
    ],
    
    views: [
        'dict.grouptype.Grid'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'groupTypeGridAspect',
            storeName: 'dict.GroupType',
            modelName: 'dict.GroupType',
            gridSelector: 'grouptypepanel'
        },
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'grouptypepanel',
            permissionPrefix: 'Ovrhl.Dictionaries.Job'
        }
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'grouptypepanel' }
    ],

    index: function() {
        var view = this.getMainPanel() || Ext.widget('grouptypepanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});