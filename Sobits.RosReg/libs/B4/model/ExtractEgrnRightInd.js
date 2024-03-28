Ext.define('B4.model.ExtractEgrnRightInd', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ExtractEgrnRightInd'
    },
    fields: [
        { name: 'Id' },        
        { name: 'Surname' },        
        { name: 'FirstName' },        
        { name: 'Patronymic' },        
        { name: 'BirthDate' },        
        { name: 'BirthPlace' },        
        { name: 'Snils' },        
        { name: 'RightId' },
        { name: 'Number' },
        { name: 'Share' },
        { name: 'Type' },
        { name: 'DocIndCode' },
        { name: 'DocIndName' },
        { name: 'DocIndSerial' },
        { name: 'DocIndNumber' },
        { name: 'DocIndDate' },
        { name: 'DocIndIssue' }
    ]
});