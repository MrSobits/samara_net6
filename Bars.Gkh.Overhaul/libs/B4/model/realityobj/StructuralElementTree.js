Ext.define('B4.model.realityobj.StructuralElementTree', {
    extend: 'Ext.data.Model',
    idProperty: 'id',
    fields: [
        {
            name: 'text'
        },
        {
            name: 'type'
        },
        {
            name: 'wearout'
        },
        {
            name: 'capacity'
        },
        {
            name: 'lastYear'
        },
        {
            name: 'UnitMeasure'
        },
        {
            name: 'multiple'
        },
        {
            name: 'count'
        },
        {
            name: 'required'
        },
        {
            name: 'added'
        },
        {
            name: 'groupid'
        },
        {
            name: 'ElemId'
        },
        {
            name: 'Exists',
            defaultValue: true,
            type: 'boolean'
        }
    ]
});