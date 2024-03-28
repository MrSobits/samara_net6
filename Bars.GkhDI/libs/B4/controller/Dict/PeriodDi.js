Ext.define('B4.controller.dict.PeriodDi', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.PeriodDi'],
    stores: ['dict.PeriodDi'],
    views: ['dict.perioddi.Grid'],

    mixins: {
        context: 'B4.mixins.Context'
    },
    mainView: 'dict.perioddi.Grid',
    mainViewSelector: 'periodDiGrid',
    refs: [{
        ref: 'mainView',
        selector: 'periodDiGrid'
    }],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'periodDiGrid',
            permissionPrefix: 'GkhDi.Dict.PeriodDi'
        },
        {
        xtype: 'inlinegridaspect',
        name: 'periodDiGridAspect',
        storeName: 'dict.PeriodDi',
        modelName: 'dict.PeriodDi',
        gridSelector: 'periodDiGrid',
        listeners: {
            beforesave: function (asp, store) {
                var result = true;
                store.each(function (record) {
                    if (Ext.isEmpty(record.get('DateStart')) || Ext.isEmpty(record.get('DateEnd'))) {
                        Ext.Msg.alert('Ошибка сохранения', 'Необходимо заполнить дату начала и дату окончания периода');
                        result = false;
                    }
                });

                return result;
            }
        }
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('periodDiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PeriodDi').load();
    }
});