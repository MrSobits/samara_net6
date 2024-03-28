Ext.define('B4.model.admincase.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCaseArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AdministrativeCase', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null }
    ]
});