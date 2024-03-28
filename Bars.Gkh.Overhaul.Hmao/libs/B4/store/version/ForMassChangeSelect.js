Ext.define('B4.store.version.ForMassChangeSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.VersionRecord'],
    autoLoad: false,
    model: 'B4.model.version.VersionRecord',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'ListForMassChangeYear'
    }
});