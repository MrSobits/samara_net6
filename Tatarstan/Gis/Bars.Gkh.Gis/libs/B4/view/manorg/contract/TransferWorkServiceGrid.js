Ext.define('B4.view.manorg.contract.TransferWorkServiceGrid', {
    extend: 'B4.view.manorg.contract.WorkServiceGrid',
    stores: ['B4.store.manorg.contract.ManOrgAgrContractService'],
    gridStore: 'B4.store.manorg.contract.ManOrgAgrContractService',
    alias: 'widget.transferworkservicegrid'
});