/**
* фейковая модель для отображения в тематиках обращений граждан
*/
Ext.define('B4.model.dict.Subj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Subj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Subject', defaultValue: null },
        { name: 'Subsubject', defaultValue: null },
        { name: 'Feature', defaultValue: null }
    ]
});