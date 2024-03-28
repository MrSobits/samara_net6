Ext.define('B4.controller.MassCalcReport731', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.QuickMsg'
    ],

    stores: [],
    
    views: ['MassCalcReport731Grid'],
    
    mainView: 'MassCalcReport731Grid',
    mainViewSelector: 'masscalcreport731grid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    init: function() {
        var me = this;

        me.control({
            'masscalcreport731grid b4combobox[name=PeriodDi]': {
                change: function() {
                    me.getMainView().getStore().load();
                }
            },
            'masscalcreport731grid button[action=Generate]' : {
                click: function() {
                    var mainView = me.getMainView(),
                        selected = mainView.getSelectionModel().getSelection(),
                        ids = [];
                    
                    if (selected.length < 1) {
                        B4.QuickMsg.msg('Не выбраны управляющие организации', message, 'warning');
                        return;
                    }

                    Ext.each(selected, function(item) {
                        ids.push(item.get('ManOrgId'));
                    });
                    try {
                        me.mask('Генерация...', mainView);

                        B4.Ajax.request({
                            url: B4.Url.action('MassGenerate', 'DiReport'),
                            method: 'POST',
                            timeout: 9999999,
                            params: {
                                objectIds: Ext.encode(ids),
                                periodId: mainView.down('b4combobox[name=PeriodDi]').getValue()
                            }
                        }).next(function (resp) {
                            var obj = Ext.decode(resp.responseText);
                            me.unmask();
                            mainView.getStore().load();
                            B4.QuickMsg.msg('Успешно', obj.data || 'Отчеты успешно сгенерированы', 'success');
                        }).error(function (er) {
                            me.unmask();
                            B4.QuickMsg.msg('Ошибка', er.message ? er.message : 'Во время генерации произошла ошибка', 'error');
                        });
                    } catch(e) {
                        me.unmask();
                    } 
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView();
        if (!view) {
            view = Ext.widget('masscalcreport731grid');
            this.bindContext(view);
            this.application.deployView(view);
        }
    }
});