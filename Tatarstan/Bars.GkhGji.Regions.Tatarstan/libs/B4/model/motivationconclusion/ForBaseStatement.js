Ext.define('B4.model.motivationconclusion.ForBaseStatement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivationConclusion',
        listAction: 'ListForBaseStetement'
    },
    fields: [
        { name: 'Id' },
        { name: 'DocumentNumber' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'ManagingOrganization' },
        { name: 'Contragent' }
    ]
});