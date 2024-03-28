Ext.define('B4.controller.SyncParams', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.syncparams.Panel'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainPanel',
            selector: 'syncparamspanel'
        }
    ],

    mainView: 'syncparams.Panel',
    mainViewSelector: 'syncparamspanel',

    init: function() {
        var me = this;

        me.control({
            'syncparamspanel b4savebutton': { 'click': { fn: me.saveParams, scope: me } },
            'syncparamspanel [name=StartIntegration]': { 'click': { fn: me.startIntegration, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainPanel() || Ext.widget('syncparamspanel');

        me.setParams(view);
        me.bindContext(view);
        me.application.deployView(view);
    },

    setParams: function(panel) {
        var me = this,
            form = panel.getForm();

        me.mask('Сохранение', B4.getBody());
        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'Reforma')
        }).next(function(resp) {
            var response = Ext.decode(resp.responseText),
                fields = form.getFields();
            form.setValues(response.data);
            fields.each(function(field) {
                field.originalValue = field.getValue();
            });
            me.unmask();
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'При получении параметров произошла ошибка');
        });
    },

    saveParams: function(btn) {
        var me = this,
            panel = btn.up('syncparamspanel'),
            form = panel.getForm(),
            valid = form.isValid(),
            values = form.getValues(false, false, false, true),
            user = form.findField('User'),
            address = form.findField('RemoteAddress'),
            password = form.findField('Password');

        if (!password.isDirty()) {
            delete values.Password;
        }

        if (valid) {
            if (user.isDirty() || address.isDirty()) {
                Ext.Msg.confirm('Сохранение параметров', 'При смене пользователя или адреса сервиса, скорее всего, потребуется очистка текущих привязок УО и жилых домов.<br/>Все привязки будут восстановлены' +
                    ' и актуализированы при следующей интеграции.<br/>Перед запуском интеграции не забудьте вручную настроить отчетные периоды.<br/>Произвести очистку?', function (bid) {
                        me.doSave(values, bid == 'yes');
                    });
            } else {
                me.doSave(values, false);
            }
        } else {
            Ext.Msg.alert('Ошибка', 'Проверьте правильность заполнения формы');
        }
    },

    doSave: function(values, cleanup) {
        var me = this;

        me.mask('Сохранение', B4.getBody());
        B4.Ajax.request({
            url: B4.Url.action('SaveParams', 'Reforma'),
            timeout: 999999,
            params: {
                parameters: Ext.encode(values),
                cleanup: cleanup
            }
        }).next(function() {
            me.unmask();
            B4.QuickMsg.msg('Сохранение', 'Параметры успешно сохранены', 'success');
            me.setParams(me.getMainPanel());
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'При сохранении параметров произошла ошибка');
        });
    },

    startIntegration: function() {
        var me = this;
        Ext.Msg.confirm('Запуск интеграции', 'Это действие немедленно запустит процесс интеграции. Вы уверены, что хотите продолжить?', function(bid) {
            if (bid == 'yes') {
                me.mask('Запуск', B4.getBody());
                B4.Ajax.request({
                    url: B4.Url.action('RunNow', 'Reforma')
                }).next(function() {
                    me.unmask();
                    B4.QuickMsg.msg('Запуск интеграции', 'Задача поставлена в очередь на исполнение', 'success');
                }).error(function(e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', e.message || 'При запуске задачи произошла ошибка');
                });
            }
        });
    }
});