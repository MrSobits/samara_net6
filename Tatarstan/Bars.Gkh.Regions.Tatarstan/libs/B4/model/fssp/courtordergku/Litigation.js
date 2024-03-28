Ext.define('B4.model.fssp.courtordergku.Litigation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Litigation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'JurInstitution' },
        { name: 'State' },
        { name: 'IndEntrRegistrationNumber' },
        { name: 'Debtor' },
        { name: 'DebtorAddress' },
        { name: 'DebtorFsspAddress' },
        { name: 'IsMatchAddress' },
        { name: 'EntrepreneurCreateDate' },
        { name: 'EntrepreneurDebtSum' }
    ]
});