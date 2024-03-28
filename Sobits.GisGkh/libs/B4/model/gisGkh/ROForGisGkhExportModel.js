Ext.define('B4.model.gisGkh.ROForGisGkhExportModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    fields: [
        //{ name: 'HouseGuid' },
        { name: 'Address' },
        { name: 'NumberGisGkhMatchedApartments' },
        { name: 'NumberGisGkhMatchedNonResidental' },
        { name: 'NumberApartments' },
        { name: 'NumberNonResidentialPremises' },
        { name: 'MatchedRooms'}
    ]
});