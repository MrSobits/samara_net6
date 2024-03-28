Ext.define('B4.controller.importexport.UnloadCounterValues', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.TypeServiceGis',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'importexport.UnloadCounterValues'
    ],

    stores: [
        'importexport.UnloadCounterValues'
    ],

    views: [
        'importexport.unloadcountervalues.Grid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'importexport.unloadcountervalues.Grid',
    mainViewSelector: 'unloadcountervaluesgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'unloadcountervaluesgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gis.ImportExportData.UnloadCounterValues',
                    selector: 'unloadcountervaluesgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }],

    init: function () {
        var me = this;
        me.control({
            'unloadcountervaluesgrid button[name=Unload]': {
                'click': { fn: me.onUnloadBtnClick, scope: me }
            },
            'unloadcountervaluesgrid b4updatebutton': {
                'click': { fn: me.updateGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('unloadcountervaluesgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    onUnloadBtnClick: function() {
        var me = this,
            grid = me.getMainView();

        me.mask('Выгрузка...', grid);

        B4.Ajax.request({
            url: B4.Url.action('Unload', 'UnloadCounterValues'),
            timeout: 999999999
        }).next(function () {
            me.unmask();
            grid.getStore().load();
            Ext.Msg.alert('Успешно', 'Начат процесс выгрузки показаний ПУ');
        }).error(function (resp) {
            me.unmask();
            Ext.Msg.alert('Внимание!', resp.message);
        });
    },

    updateGrid: function() {
        this.getMainView().getStore().load();
    }
});