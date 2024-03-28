Ext.define('B4.controller.report.RealityObjectDataReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RealityObjectDatareportPanel',
    mainViewSelector: 'realityobjectdatareportpanel',

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox'
    ],

    stores: [
        'B4.store.RealityObject'
    ],

    refs: [
        {
            ref: 'RoSelectField',
            selector: 'realityobjectdatareportpanel b4selectfield[name=RealityObject]'
        }
    ],

    validateParams: function () {
        var roId = this.getRoSelectField();
        return roId && roId.isValid();
    },

    getParams: function () {        
        var roField = this.getRoSelectField();
        
        return {
            house: (roField ? roField.getValue() : null)
        };
    }
});