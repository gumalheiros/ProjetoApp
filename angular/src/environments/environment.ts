import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'ProjetoApp',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44396/',
    redirectUri: baseUrl,
    clientId: 'ProjetoApp_App',
    responseType: 'code',
    scope: 'offline_access ProjetoApp',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44396',
      rootNamespace: 'ProjetoApp',
    },
  },
} as Environment;
