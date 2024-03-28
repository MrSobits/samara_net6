Ext.define('B4.controller.dict.YearCorrection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    models: ['dict.YearCorrection'],
    stores: ['dict.YearCorrection'],
    views: ['dict.yearcorrection.Grid'],

    mainView: 'dict.yearcorrection.Grid',
    mainViewSelector: 'yearCorrectionGrid',

    refs: [{
        ref: 'mainView',
        selector: 'yearCorrectionGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'yearCorrectionGridAspect',
            storeName: 'dict.YearCorrection',
            modelName: 'dict.YearCorrection',
            gridSelector: 'yearCorrectionGrid',
            deleteRecord: function (record, grid) {
                var me = this,
                    recId = record.get('Id');

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (res) {
                    if (res == 'yes') {
                        var model = me.controller.getModel('dict.YearCorrection');

                        var rec = new model({ Id: recId });
                        me.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function () {
                                me.getGrid().getStore().load();;
                                me.unmask();
                            }, this)
                            .error(function (failure) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(failure.responseData) ? failure.responseData : failure.responseData.message);
                                me.unmask();
                            }, this);
                    }
                }, me);
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('yearCorrectionGrid');
        this.bindContext(view);
        this.getStore('dict.YearCorrection').load();
    }
});