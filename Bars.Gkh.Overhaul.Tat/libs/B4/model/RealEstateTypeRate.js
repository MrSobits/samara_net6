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
        { name: 'NeedForFunding' },
        { name: 'TotalArea' },
        { name: 'ReasonableRate' },
        { name: 'RateDeficit' }
    ]
});