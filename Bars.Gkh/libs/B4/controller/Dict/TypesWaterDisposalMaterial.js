Ext.define('B4.controller.dict.TypesWaterDisposalMaterial', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeswaterdisposalmaterialgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesWaterDisposalMaterial'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typeswaterdisposalmaterialgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typeswaterdisposalmaterial.Grid'],

    mainView: 'dict.typeswaterdisposalmaterial.Grid',
    mainViewSelector: 'typeswaterdisposalmaterialgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typeswaterdisposalmaterialgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typeswaterdisposalmaterialgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});