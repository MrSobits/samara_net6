Ext.Ajax.addListener("requestexception", function (response, obj) {
    if (obj.responseText.indexOf('AjaxAuthError') > 0) {
        Ext.select("#b4-loading-mask").hide();
        Ext.Msg.alert('Истекло время сеанса', 'Вы не работали с системой продолжительное время, в связи с чем сеанс работы был прекращен. Для продолжения работы Вам необходимо в новом открывшемся окне ввести Ваш логин и пароль. После авторизации Вы можете вернутся в текущую вкладку и продолжить работу.', function () {
            window.open(rootUrl + 'login');
        });
    }
});

Ext.Ajax.addListener("requestcomplete", function (response, obj) {
    if (obj.responseText.indexOf('TableLockedException') > -1) {
        var resp = Ext.JSON.decode(obj.responseText);
        Ext.select("#b4-loading-mask").hide();
        Ext.Msg.alert('Блокировка данных', resp.message);
    }
});

// эта возможно неочевидная вещь прибавляет ко всем (ну многим) POST-запросам параметр b4_pseudo_xhr=true
// это необходимо для корректной обработки ошибок на клиенте.
// если быть точным, b4_pseudo_xhr=true делает обернуть ошибку, возникшую в ходе POST-запроса, в красивую,
// а главное удобную JSON обертку, а не выплевывать IIS-ную страницу с ошибкой
Ext.override(Ext.form.action.Submit, {
    getUrl: function () {
        var url = this.url || this.form.url;
        return this.getMethod() == 'POST' && url.indexOf('b4_pseudo_xhr') == -1 ? Ext.urlAppend(url, 'b4_pseudo_xhr=true') : url;
    }
});