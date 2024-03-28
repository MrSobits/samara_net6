Ext.define('B4.controller.dict.ProgramCrType', {
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
        'dict.ProgramCrType'
    ],
    
    models: [
        'ProgramCrType'
    ],
    
    views: [
        'dict.programcrtype.Grid'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'programCrTypeGridAspect',
            storeName: 'dict.ProgramCrType',
            modelName: 'ProgramCrType',
            gridSelector: 'programcrtypegrid'
        },
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'programcrtypegrid',
            permissionPrefix: 'GkhRegOp.Dictionaries.ProgramCrType'
        }
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'programcrtypegrid' }
    ],

    index: function() {
        var me = this,
            view = me.getMainPanel() || Ext.widget('programcrtypegrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});