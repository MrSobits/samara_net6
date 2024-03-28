Ext.define('B4.controller.dict.FiasOktmo', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['dict.FiasOktmo'],
    stores: ['dict.FiasOktmo'],
    views:  ['dict.fiasoktmo.Grid', 'dict.fiasoktmo.EditWindow'],

    mainView: 'dict.fiasoktmo.Grid',
    mainViewSelector: 'fiasoktmo',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'fiasoktmo'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'fiasMunicipalityGridWindowAspect',
            gridSelector: 'fiasoktmo',
            editFormSelector: '#fiasoktmoEditWindow',
            storeName: 'dict.FiasOktmo',
            modelName: 'dict.FiasOktmo',
            editWindowView: 'dict.fiasoktmo.EditWindow'
        },
         {
             xtype: 'gkhpermissionaspect',
             permissions: [
                 { name: 'Administration.Oktmo.fiasoktmo.Edit', applyTo: 'b4addbutton', selector: 'fiasoktmo' },
                 {
                     name: 'Administration.Oktmo.fiasoktmo.Delete', applyTo: 'b4deletecolumn', selector: 'fiasoktmo',
                     applyBy: function (component, allowed) {
                         if (allowed) component.show();
                         else component.hide();
                     }
                 },
                 {
                     name: 'Administration.Oktmo.fiasoktmo.Edit', applyTo: 'b4editcolumn', selector: 'fiasoktmo',
                     applyBy: function (component, allowed) {
                         if (allowed) component.show();
                         else component.hide();
                     }
                 }
             ]
         }
    ],

    index: function() {
        var view = this.getMainView() || Ext.widget('fiasoktmo');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.FiasOktmo').load();
    }
});