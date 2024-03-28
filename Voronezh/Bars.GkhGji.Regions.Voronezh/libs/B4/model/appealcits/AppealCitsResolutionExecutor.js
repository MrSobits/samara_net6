Ext.define('B4.model.appealcits.AppealCitsResolutionExecutor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsResolutionExecutor'
    },
    fields: [
        { name: 'Id' },
        { name: 'AppealCitsResolution' },
        { name: 'Name' },
        { name: 'Surname' },
        { name: 'Patronymic' },
        { name: 'PersonalTerm' },
        { name: 'Comment' },
        { name: 'IsResponsible', defaultValue: 20 }
    ]
});