Ext.define('B4.model.smev.SMEVMVD', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVMVD'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'RequestPerson' },
        { name: 'AddressAdditional' },
        { name: 'AddressPrimary' },
        { name: 'CalcDate' },
        { name: 'BirthDate' },
        { name: 'MVDTypeAddressAdditional', defaultValue: 0},
        { name: 'MVDTypeAddressPrimary' },
        { name: 'PatronymicName' },
        { name: 'RegionCodeAdditional'},
        { name: 'RegionCodePrimary'},
        { name: 'SNILS' },
         { name: 'Name' },
        { name: 'Surname' },
        { name: 'AnswerInfo' },
        { name: 'Answer' }
    ]
});