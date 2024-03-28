/**
* модель связи тематики и подтематики обращения
*/
Ext.define('B4.model.dict.StatSubjectSubsubjectGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StatSubjectSubsubjectGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Subject', defaultValue: null },
        { name: 'Subsubject', defaultValue: null },
        { name: 'Name' },
        { name: 'ISSOPR' },
        { name: 'Code' }
    ]
});