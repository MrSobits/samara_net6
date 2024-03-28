Ext.define('B4.model.appealcits.AppealCitsResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsResolution'
    },
    fields: [
        { name: 'Id' },
        { name: 'AppealCits' },
        { name: 'ResolutionText' },
        { name: 'ResolutionTerm' },
        { name: 'ResolutionAuthor' },
        { name: 'ResolutionDate' },
        { name: 'ResolutionContent' },
        { name: 'ParentResolutionData', defaultValue: null },
        { name: 'ImportId'},
        { name: 'ParentId'},
        { name: 'Executed'}
    ]
});