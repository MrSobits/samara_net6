Ext.define('B4.model.regop.owner.PersonalAccountOwner', {
    extend: 'B4.base.Model',

    idProperty: 'Id',

    fields: [
        { name: 'Id' },
        { name: 'Name', persist: false },
        { name: 'OwnerType' },
        { name: 'PrivilegedCategory' },
        { name: 'AccountsCount' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'BillingAddressType' },
        { name: 'Address' },
        { name: 'ActiveAccountsCount' },
        { name: 'TotalAccountsCount' },
        { name: 'FiasFactAddress' },
        { name: 'FactAddrDoc'},
        { name: 'BirthDate', type: 'date', useNull: true }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountOwner'
    }
});