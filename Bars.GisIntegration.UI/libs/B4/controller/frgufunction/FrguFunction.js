Ext.define('B4.controller.frgufunction.FrguFunction', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'FrguFunction'
    ],
    stores: [
        'FrguFunction'
    ],
    views: [
        'frgufunction.Grid',
        'frgufunction.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'frgufunction.Grid',
    mainViewSelector: 'frgufunctionGrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'frgufunctionGridWindowAspect',
            gridSelector: 'frgufunctionGrid',
            editFormSelector: 'frgufunctioneditwindow',
            storeName: 'FrguFunction',
            modelName: 'FrguFunction',
            editWindowView: 'frgufunction.EditWindow',
            onSaveSuccess: function (aspect) {
                var me = aspect,
                    store = me.getGrid().getStore();

                me.getForm().close();

                store.load();
            },
            deleteRecord: function (record) {
                var me = this,
                    recId = record.get('Id'),
                    model = me.controller.getModel(this.modelName),
                    rec = new model({ Id: recId });

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        me.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function () {
                                me.getGrid().getStore().load();
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
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        view.getStore().load();
    }
});