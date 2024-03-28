Ext.define('B4.model.controlorg.ControlOrganization', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlOrganization'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },

        { name: 'ContragentName' },
        { name: 'ContragentShortName' },
        { name: 'ContragentOrganizationForm' },
        { name: 'ContragentParentOrgName' },

        { name: 'ContragentInn' },
        { name: 'ContragentKpp' },
        { name: 'ContragentJurAddress' },
        { name: 'ContragentFactAddress' },
        { name: 'ContragentPostAddress' },
        { name: 'ContragentOutsideSubjectAddress' },

        { name: 'ContragentOgrn' },
        { name: 'ContragentDateRegistration' },
        { name: 'ContragentOgrnRegistration' },

        { name: 'Department' },
        { name: 'PersonPosition' },
        { name: 'ControlType' },
        { name: 'ChildOrgs' }
       
    ]
});