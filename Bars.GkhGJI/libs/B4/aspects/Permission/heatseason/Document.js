Ext.define('B4.aspects.permission.heatseason.Document', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.heatseasondocumentperm',

    permissions: [
        { name: 'GkhGji.HeatSeason.Register.Document.Edit', applyTo: 'b4savebutton', selector: '#heatSeasonDocEditWindow' },

        //поля панели редактирования
        { name: 'GkhGji.HeatSeason.Register.Document.Field.TypeDocument_Edit', applyTo: '#cbTypeDocument', selector: '#heatSeasonDocEditWindow' },
        { name: 'GkhGji.HeatSeason.Register.Document.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#heatSeasonDocEditWindow' },
        { name: 'GkhGji.HeatSeason.Register.Document.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#heatSeasonDocEditWindow' },
        { name: 'GkhGji.HeatSeason.Register.Document.Field.File_Edit', applyTo: '#ffFile', selector: '#heatSeasonDocEditWindow' },
        { name: 'GkhGji.HeatSeason.Register.Document.Field.Description_Edit', applyTo: '#taDescription', selector: '#heatSeasonDocEditWindow' }
    ]
});