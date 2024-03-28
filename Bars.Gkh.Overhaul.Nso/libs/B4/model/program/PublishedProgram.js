Ext.define('B4.model.program.PublishedProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublishedProgram'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProgramVersion' },
        { name: 'State' },
        { name: 'EcpSigned' },
        { name: 'FileSign' },
        { name: 'FileXml' },
        { name: 'FilePdf' },
        { name: 'FileCertificate' },
        { name: 'SignDate' },
        { name: 'PublishDate' }
    ]
});