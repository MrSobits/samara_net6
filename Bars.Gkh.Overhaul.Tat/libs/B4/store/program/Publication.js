Ext.define('B4.store.program.Publication', {
    extend: 'B4.base.Store',
    requires: ['B4.model.program.PublishedProgramRecord'],
    autoLoad: false,
    model: 'B4.model.program.PublishedProgramRecord'
});