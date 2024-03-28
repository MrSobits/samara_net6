Ext.define('B4.store.regop.SpecialAccount', {
    extend: 'B4.base.Store',
    model: 'B4.model.regop.SpecialAccountNonRegop',
    requires: ['B4.model.regop.SpecialAccountNonRegop'],
    autoLoad: false
});