Ext.define('B4.model.regop.owner.LegalAccountOwner', {
    extend: 'B4.model.regop.owner.PersonalAccountOwner',

    fields: [
        { name: 'Contragent' },
        { name: 'PrintAct' },
        { name: 'Address' },
        { name: 'Name' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LegalAccountOwner'
    }
});