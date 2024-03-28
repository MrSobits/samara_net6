Ext.define('B4.model.smev2.SMEVStayingPlace', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVStayingPlace'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState', defaultValue: 0 },
        { name: 'CalcDate' },
        { name: 'MessageId' },
        { name: 'CitizenLastname' },
        { name: 'CitizenFirstname' },
        { name: 'CitizenGivenname' },
        { name: 'CitizenBirthday' },
        { name: 'CitizenSnils' },
        { name: 'DocType' },
        { name: 'DocSerie' },
        { name: 'Answer' },
        { name: 'DocNumber' },
        { name: 'DocIssueDate' },
        { name: 'RegionCode' },
        { name: 'LPlaceDistrict' },
        { name: 'LPlaceCity' },
        { name: 'LPlaceStreet' },
        { name: 'LPlaceHouse' },
        { name: 'LPlaceBuilding' },
        { name: 'LPlaceFlat' },
        { name: 'RegStatus' },
        { name: 'FIO' },
        { name: 'SerNumber' },
        { name: 'LPlaceRegion' },
        { name: 'DocCountry' }
    ]
});