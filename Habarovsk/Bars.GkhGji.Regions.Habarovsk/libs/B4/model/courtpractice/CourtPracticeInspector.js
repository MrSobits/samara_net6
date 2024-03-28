Ext.define('B4.model.courtpractice.CourtPracticeInspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeInspector'
    },
    fields: [
        { name: 'Id' },
        { name: 'CourtPractice'},
        { name: 'Inspector'},
        { name: 'LawyerInspector'},
        { name: 'Discription' }
       
    ]
});