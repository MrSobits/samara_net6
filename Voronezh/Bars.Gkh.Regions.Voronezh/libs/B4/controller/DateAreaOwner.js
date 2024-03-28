Ext.define('B4.controller.DateAreaOwner', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
         'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['DateAreaOwner'],
    stores: ['DateAreaOwner'],
    views: [
        'dateareaowner.Grid',
        'dateareaowner.EditWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'dateareaowner.Grid',
    mainViewSelector: 'dateareaownergrid',
    editWindowSelector: 'dateareaownerEditWindow',
    aspects: [

    {
        xtype: 'grideditwindowaspect',
        name: 'dateAreaOwnerGridWindowAspect',
        gridSelector: 'dateareaownergrid',
        editFormSelector: '#dateareaownerEditWindow',
        storeName: 'DateAreaOwner',
        modelName: 'DateAreaOwner',
        editWindowView: 'dateareaowner.EditWindow',
  
    }
],


    refs: [
        {
            ref: 'mainView',
            selector: 'dateareaownergrid'
        }
    ],


    init: function () {
        var me = this;
        me.control({
          
            'dateareaownergrid button[action="Merge"]': {
                click: me.onMerge
            },
          
        });
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('dateareaownergrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onMerge: function () {
        debugger;
        B4.Ajax.request({
            url: B4.Url.action('Merge', 'DataAreaOwnerMerger')
        });
    },
});