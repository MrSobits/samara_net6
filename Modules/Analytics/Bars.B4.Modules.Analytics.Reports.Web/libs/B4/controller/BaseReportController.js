Ext.define('B4.controller.BaseReportController', {
    extend: 'B4.base.Controller',

    containerSelector: '#reportPanel tabpanel',

    /**
     * Метод валидации параметров.
     * Если возвращает строку - выведется соответствующее уведомление.
     * Если валидация прошла успешно - метод должен вернуть true.
     * @return {Boolean/String} Validate result
     */
    validateParams: function() {
        return true;
    },


    /**
     * Метод получения параметров для отправки на сервер.
     * @return {Object} JSON-объект
     */
    getParams: function() {
        return {};
    }
});