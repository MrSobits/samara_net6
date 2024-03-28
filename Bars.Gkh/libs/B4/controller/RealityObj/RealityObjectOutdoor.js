Ext.define('B4.controller.realityobj.RealityObjectOutdoor', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.realityobj.RealityObjectOutdoor',
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['realityobj.RealityObjectOutdoor'],
    stores: ['realityobj.RealityObjectOutdoor'],
    views: [
        'realityobj.realityobjectoutdoor.Grid',
        'realityobj.realityobjectoutdoor.AddWindow',
        'realityobj.realityobjectoutdoor.EditPanel'
    ],

    mainView: 'realityobj.realityobjectoutdoor.Grid',
    mainViewSelector: 'realityobjectoutdoorgrid',

    aspects: [
        {
            xtype: 'realityobjectoutdoorpermissionsaspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'RealityObjectOutdoorGridWindowAspect',
            gridSelector: 'realityobjectoutdoorgrid',
            modelName: 'realityobj.RealityObjectOutdoor',
            editWindowView: 'realityobj.realityobjectoutdoor.AddWindow',
            editFormSelector: 'realityobjectoutdooraddwindow',

            listeners: {
                'aftersetformdata': function (asp, rec) {
                    var me = this,
                        view = asp.getForm(),
                        selectField = view.down('[name=MunicipalityFiasOktmo]'),
                        store = selectField.getStore();
                    store.on('beforeload', me.onBeforeLoad, me);
                }
            },

            editRecord: function (record) {
                var me = this,
                    model = me.controller.getModel(me.modelName),
                    id = record ? record.get('Id') : null;

                if (id) {
                    me.controller.application.redirectTo(Ext.String.format('realityobjectoutdooredit/{0}', id));
                    return;
                }
                me.setFormData(new model({ Id: 0 }));
            },

            onBeforeLoad: function (store, operation) {
                operation.params.needMunicipalityFilter = true;
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'realityobjectoutdoorButtonExportAspect',
            gridSelector: 'realityobjectoutdoorgrid',
            buttonSelector: 'realityobjectoutdoorgrid button[action="export"]',
            controllerName: 'RealityObjectOutdoor',
            actionName: 'Export'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});