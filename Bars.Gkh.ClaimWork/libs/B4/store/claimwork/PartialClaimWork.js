﻿Ext.define('B4.store.claimwork.PartialClaimWork', {
    extend: 'B4.base.Store',
    model: 'B4.model.claimwork.FlattenedClaimWork',
    requires: ['B4.model.claimwork.FlattenedClaimWork'],
    autoLoad: true,
    filters: [
        {
            property: 'Archived',
            value: false
        }]
});