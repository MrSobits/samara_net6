Ext.define('B4.controller.dict.PeriodNormConsumption', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
         {
             xtype: 'gkhinlinegridaspect',
             name: 'unitMeasureGridAspect',
             storeName: 'dict.PeriodNormConsumption',
             modelName: 'dict.PeriodNormConsumption',
             gridSelector: 'periodnormconsumptiongrid',
             deleteRecord: function (record) {
                 var me = this,
                     store = me.getStore();

                 Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись? Все связанные нормативы потребления будут удалены', function (result) {
                     if (result == 'yes') {
                         store.remove(record);
                     }
                 }, me);
             }
         }
    ],

    models: ['dict.PeriodNormConsumption'],
    stores: ['dict.PeriodNormConsumption'],
    views: ['dict.PeriodNormConsumption'],

    mainView: 'dict.PeriodNormConsumption',
    mainViewSelector: 'periodnormconsumptiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'periodnormconsumptiongrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('periodnormconsumptiongrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PeriodNormConsumption').load();
    }
});