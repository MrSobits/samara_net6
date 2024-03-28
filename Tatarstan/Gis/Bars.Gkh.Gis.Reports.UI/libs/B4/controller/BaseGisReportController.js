Ext.define('B4.controller.BaseGisReportController', {
    extend: 'B4.controller.BaseReportController',

    //метод валидации параметров
    validateParams: function () {
        return this.getMainView().getForm().isValid();
    }
});
