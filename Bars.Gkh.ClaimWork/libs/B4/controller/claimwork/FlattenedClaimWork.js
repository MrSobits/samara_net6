Ext.define('B4.controller.claimwork.FlattenedClaimWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['claimwork.FlattenedClaimWork'],
    stores: ['claimwork.FlattenedClaimWork'],
    views: [
        //'claimwork.flattenedclaimwork.EditPanel',
        'claimwork.flattenedclaimwork.EditWindow',
        'claimwork.flattenedclaimwork.Grid'
    ],
    parentId: null,
    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'claimwork.flattenedclaimwork.Grid',
    mainViewSelector: 'flattenedclaimworkgrid',
    editWindowSelector: 'flattenedclaimworkEditWindow',
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'flattenedclaimworkGridWindowAspect',
            gridSelector: 'flattenedclaimworkgrid',
            editFormSelector: '#flattenedclaimworkEditWindow',
            storeName: 'claimwork.FlattenedClaimWork',
            modelName: 'claimwork.FlattenedClaimWork',
            editWindowView: 'claimwork.flattenedclaimwork.EditWindow',
            listeners: {
                aftersetformdata: function(asp, rec, form) {
                    var me = this;
                    //debugger;
                    parentId = rec.getId();
                }
            }
        }
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'flattenedclaimworkgrid'
        }
    ],
    init: function() {
        var me = this;

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('flattenedclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);
        //view.getStore().load();
    }
});