Ext.define('B4.view.manorg.contract.OwnersCommunalServiceGrid', {
    extend: 'B4.view.manorg.contract.CommunalServiceGrid',
    stores: ['B4.store.manorg.contract.ManOrgComContractService'],
    gridStore: 'B4.store.manorg.contract.ManOrgComContractService',
    alias: 'widget.contractownerscommunalservicegrid'
});