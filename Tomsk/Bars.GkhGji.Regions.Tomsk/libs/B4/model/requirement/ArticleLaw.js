Ext.define('B4.model.requirement.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RequirementArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Requirement', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null }
    ]
});