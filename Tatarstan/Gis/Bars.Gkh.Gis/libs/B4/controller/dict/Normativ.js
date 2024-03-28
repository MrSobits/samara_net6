Ext.define('B4.controller.dict.Normativ', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.enums.TypeServiceGis',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.GisNormativ'
    ],

    stores: [
        'dict.GisNormativ'
    ],

    views: [
        'dict.normativ.Grid',
        'dict.normativ.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.normativ.Grid',
    mainViewSelector: 'normativdictgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'normativdictgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'normativDictGridEditWindowAspect',
            gridSelector: 'normativdictgrid',
            editFormSelector: 'normativdicteditwin',
            modelName: 'dict.GisNormativ',
            editWindowView: 'dict.normativ.EditWindow',
            otherActions: function (actions) {
                //actions[this.gridSelector + ' checkbox[cmd=withArchiveRecs]'] = { 'change': { fn: this.onShowWithArchiveRecs, scope: this } };
            },
            //onSaveSuccess: function (asp, record) {
            //    asp.controller.setCurrentId(record.getId());
            //},
            listeners: {
                //aftersetformdata: function (asp, record) {
                //    var me = this,
                //        editWin = me.getForm();

                //    editWin.down('bilservicedictgrid').getStore().filter('serviceId', record.getId());
                //    editWin.down('biltarifstoragedictgrid').getStore().filter('serviceId', record.getId());
                //    me.controller.setContextValue(editWin, 'serviceId', record.getId());
                //}
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('normativdictgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});