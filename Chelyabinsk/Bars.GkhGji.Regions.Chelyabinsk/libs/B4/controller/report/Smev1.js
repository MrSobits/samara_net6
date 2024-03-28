Ext.define('B4.controller.report.Smev1', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.Smev1Panel',
    mainViewSelector: '#reportSmev1Panel',

    requires: [
        'B4.form.ComboBox'
    ],

    stores: [
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    DateStartFieldSelector: '#reportSmev1Panel #dfDateStart',
    DateEndFieldSelector: '#reportSmev1Panel #dfDateEnd',


    validateParams: function () {
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid());
    },

    getParams: function () {
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];

        return {
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});