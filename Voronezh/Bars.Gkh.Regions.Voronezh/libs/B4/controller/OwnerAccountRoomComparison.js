Ext.define('B4.controller.OwnerAccountRoomComparison', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
         'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['OwnerAccountRoomComparison'],
    stores: ['OwnerAccountRoomComparison'],
    views: [
        'owneraccountroomcomparison.Grid',
        'owneraccountroomcomparison.EditWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'owneraccountroomcomparison.Grid',
    mainViewSelector: 'owneraccountroomcomparisongrid',
    editWindowSelector: 'owneraccountroomcomparisonEditWindow',
    aspects: [

    {
        xtype: 'grideditwindowaspect',
        name: 'owneraccountroomcomparisonGridWindowAspect',
        gridSelector: 'owneraccountroomcomparisongrid',
        editFormSelector: '#owneraccountroomcomparisonEditWindow',
        storeName: 'OwnerAccountRoomComparison',
        modelName: 'OwnerAccountRoomComparison',
        editWindowView: 'owneraccountroomcomparison.EditWindow',
  
    }
],


    refs: [
        {
            ref: 'mainView',
            selector: 'owneraccountroomcomparisongrid'
        }
    ],


    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('owneraccountroomcomparisongrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});