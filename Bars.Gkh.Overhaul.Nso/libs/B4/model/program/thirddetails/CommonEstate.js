Ext.define('B4.model.program.thirddetails.CommonEstate', {
    extend: 'Ext.data.Model',
    
    idProperty: 'Id',
    
    proxy: {
        type: 'memory'
    },

    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Measure' },
        { name: 'Volume' },
        { name: 'Year' },
        { name: 'WorkSum' },
        { name: 'ServiceSum' },
        { name: 'ServiceAndWorkSum' }
    ]
});