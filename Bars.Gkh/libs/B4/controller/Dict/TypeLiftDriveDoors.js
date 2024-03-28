Ext.define('B4.controller.dict.TypeLiftDriveDoors', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeLiftDriveDoorsGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeLiftDriveDoors'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeLiftDriveDoorsGridAspect',
            storeName: 'dict.TypeLiftDriveDoors',
            modelName: 'dict.TypeLiftDriveDoors',
            gridSelector: 'typeLiftDriveDoorsGrid'
        }
    ],

    models: ['dict.TypeLiftDriveDoors'],
    stores: ['dict.TypeLiftDriveDoors'],
    views: ['dict.typeliftdrivedoors.Grid'],

    mainView: 'dict.typeliftdrivedoors.Grid',
    mainViewSelector: 'typeLiftDriveDoorsGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'typeLiftDriveDoorsGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeLiftDriveDoorsGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeLiftDriveDoors').load();
    }
});