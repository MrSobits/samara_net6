Ext.define('B4.store.ContributionCollection', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ContributionCollection'],
    autoLoad: false,
    groupField: 'Date',
    model: 'B4.model.ContributionCollection'
});