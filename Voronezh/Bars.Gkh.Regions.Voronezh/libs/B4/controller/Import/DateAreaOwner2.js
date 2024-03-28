Ext.define('B4.controller.Import.DateAreaOwner', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.DateAreaOwnerImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.DateAreaOwner'],

    mainView: 'Import.DateAreaOwner',
    mainViewSelector: 'dateareaownerimport',

    aspects: [
    {
        xtype: 'dateareaownerimportaspect',
        viewSelector: 'dateareaownerimport',
        importId: 'Bars.Gkh.Import.DateAreaOwnerImport'
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});