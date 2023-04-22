import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { DataPermissionComponent } from './components/data-permission.component';
import { DataPermissionRoutingModule } from './data-permission-routing.module';

@NgModule({
  declarations: [DataPermissionComponent],
  imports: [CoreModule, ThemeSharedModule, DataPermissionRoutingModule],
  exports: [DataPermissionComponent],
})
export class DataPermissionModule {
  static forChild(): ModuleWithProviders<DataPermissionModule> {
    return {
      ngModule: DataPermissionModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<DataPermissionModule> {
    return new LazyModuleFactory(DataPermissionModule.forChild());
  }
}
