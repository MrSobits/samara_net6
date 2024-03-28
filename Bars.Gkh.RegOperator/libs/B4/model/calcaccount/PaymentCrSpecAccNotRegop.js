Ext.define('B4.model.calcaccount.PaymentCrSpecAccNotRegop', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialCalcAccount',
        listAction: 'ListPaymentCrSpecAccNotRegop',
        readAction: 'GetPaymentCrSpecAccNotRegop',
        updateAction: 'EditPaymentCrSpecAccNotRegop'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Period' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'Address' },
        { name: 'InputDate' },
        { name: 'AmountIncome' },
        { name: 'EndYearBalance' },
        { name: 'File', defaultValue: null }
    ]
});