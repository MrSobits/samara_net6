Ext.define('B4.model.personalaccount.ServiceSubsidy', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceSubsidyRegister',
        listAction: 'ListByApartmentId'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Pss' },
        { name: 'Service' },
        { name: 'AccruedBenefitSum' },
        { name: 'AccruedEdvSum' },
        { name: 'RecalculatedBenefitSum' },
        { name: 'RecalculatedEdvSum' }
    ]
});