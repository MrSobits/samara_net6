Ext.define('B4.model.dict.ViolationGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ViolationGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'GkRf' },
        { name: 'CodePin' },
        { name: 'Description' },
        { name: 'PpRf25' },
        { name: 'PpRf307' },
        { name: 'PpRf491' },
        { name: 'FeatViol' },
        { name: 'ActRemViol' },
        { name: 'PpRf170' },
        { name: 'OtherNormativeDocs' },
        { name: 'RuleOfLaw' },
        { name: 'NormDocNum' }
    ]
});