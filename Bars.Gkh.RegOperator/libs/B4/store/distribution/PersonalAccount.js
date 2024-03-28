Ext.define('B4.store.distribution.PersonalAccount', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'PersonalAccountNum' },
        { name: 'RoPayAccountNum' },
        { name: 'AccountOwner' },
        { name: 'RoomAddress' },
        { name: 'OwnerType' },
        { name: 'Sum' },
        { name: 'SumPenalty' },
        { name: 'Debt' },
        { name: 'Index' },
        { name: 'State' },
        { name: 'PeriodAccumulation' }
    ]
});