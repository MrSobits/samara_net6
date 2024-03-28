Ext.define('B4.controller.claimwork.PartialClaimWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['claimwork.FlattenedClaimWork'],
    stores: ['claimwork.PartialClaimWork'],
    views: [
        'claimwork.partialclaimwork.EditWindow',
        'claimwork.partialclaimwork.Grid'
    ],
    parentId: null,
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'claimwork.partialclaimwork.Grid',
    mainViewSelector: 'partialclaimworkgrid',
    editWindowSelector: 'partialclaimworkEditWindow',
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'partialclaimworkGridWindowAspect',
            gridSelector: 'partialclaimworkgrid',
            editFormSelector: '#partialclaimworkEditWindow',
            storeName: 'claimwork.PartialClaimWork',
            modelName: 'claimwork.FlattenedClaimWork',
            editWindowView: 'claimwork.partialclaimwork.EditWindow',
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
            selector: 'partialclaimworkgrid'
        }
    ],
    init: function() {
        var me = this;

        me.control(actions);
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('partialclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);
        //view.getStore().load();
    }
});