Ext.define('B4.controller.integrations.ExternalRis', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['integrations.ExternalRis'],

    refs: [
        {
            ref: 'mainView',
            selector: 'ris-externalris-frame'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('ris-externalris-frame'),
            currentContextKey = me.getCurrentContextKey(),
            state = b4app.getState(),
            activeToken = state.getActiveToken();

        B4.Ajax.request({
            url: B4.Url.action('GetRisUrl', 'RisSettings'),
            params: {
                redirect: currentContextKey
            }
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            if (!data) {
                alert('Пользователь не был аутентифицирован.');
                return;
            }

            var win = window.open(data, '_blank');
            me.application.redirectTo('#');

            if (win) {
                //Browser has allowed it to be opened
                win.focus();
            } else {
                //Browser has blocked it
                alert('Всплывающее окно было заблокировано. Разрешите показ всплывающих окон.');
            }
        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || 'Не найден путь до РИС'));
        });

        // Удаление токена, чтобы не было перенаправления на РИС при закрытии вкладок ЖКХ
        state.remove(activeToken);
    }
});