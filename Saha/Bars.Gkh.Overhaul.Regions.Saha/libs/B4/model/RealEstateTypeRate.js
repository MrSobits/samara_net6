Ext.define('B4.model.RealEstateTypeRate', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateTypeRate'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealEstateType' },
        { name: 'RealEstateTypeName' },
        { name: 'SociallyAcceptableRate' },
        { name: 'SociallyAcceptableRateNotLivingArea' },
        { name: 'NeedForFunding' },
        { name: 'TotalArea' },
        { name: 'TotalNotLivingArea' },
        { name: 'ReasonableRate' },
        { name: 'ReasonableRateNotLivingArea' },
        { name: 'RateDeficit' },
        { name: 'RateDeficitNotLivingArea' },
        { name: 'Year' }
    ]
});