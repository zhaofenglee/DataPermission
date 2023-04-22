import { ModuleWithProviders, NgModule } from '@angular/core';
import { DATA_PERMISSION_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class DataPermissionConfigModule {
  static forRoot(): ModuleWithProviders<DataPermissionConfigModule> {
    return {
      ngModule: DataPermissionConfigModule,
      providers: [DATA_PERMISSION_ROUTE_PROVIDERS],
    };
  }
}
