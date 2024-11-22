import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'DataPermission',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44332/',
    redirectUri: baseUrl,
    clientId: 'DataPermission_App',
    responseType: 'code',
    scope: 'offline_access DataPermission',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44332',
      rootNamespace: 'JS.Abp.DataPermission',
    },
    DataPermission: {
      url: 'https://localhost:44399',
      rootNamespace: 'JS.Abp.DataPermission',
    },
  },
} as Environment;
