Ext.define('B4.store.suggestion.CitizenSuggestion', {
    extend: 'B4.base.Store',
    requires: ['B4.model.suggestion.CitizenSuggestion'],
    autoLoad: false,
    storeId: 'citizenSuggestionStore',
    model: 'B4.model.suggestion.CitizenSuggestion'
});