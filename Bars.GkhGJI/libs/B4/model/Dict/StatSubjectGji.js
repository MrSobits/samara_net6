/**
* модель тематики обращения
*/
Ext.define('B4.model.dict.StatSubjectGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StatSubjectGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'ISSOPR' },
        { name: 'SSTUName' },
        { name: 'SSTUCode' },
        { name: 'TrackAppealCits' },
    ]
});