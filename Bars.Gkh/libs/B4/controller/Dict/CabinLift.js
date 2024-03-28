Ext.define('B4.controller.dict.CabinLift', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'cabinLiftGrid',
            permissionPrefix: 'Gkh.Dictionaries.CabinLift'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'cabinLiftGridAspect',
            storeName: 'dict.CabinLift',
            modelName: 'dict.CabinLift',
            gridSelector: 'cabinLiftGrid'
        }
    ],

    models: ['dict.CabinLift'],
    stores: ['dict.CabinLift'],
    views: ['dict.cabinlift.Grid'],

    mainView: 'dict.cabinlift.Grid',
    mainViewSelector: 'cabinLiftGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'cabinLiftGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('cabinLiftGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.CabinLift').load();
    }
});