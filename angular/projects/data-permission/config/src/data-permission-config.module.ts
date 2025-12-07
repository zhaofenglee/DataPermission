import {makeEnvironmentProviders} from '@angular/core';
import { DATA_PERMISSION_ROUTE_PROVIDERS } from './providers/route.provider';

export function provideDataPermissionConfig() {
  return makeEnvironmentProviders([DATA_PERMISSION_ROUTE_PROVIDERS])
}
