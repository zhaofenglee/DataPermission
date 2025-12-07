import { RouterOutletComponent } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { DataPermissionComponent } from './components/data-permission.component';

export const dataPermissionRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: RouterOutletComponent,
    children: [
      {
        path: '',
        component: DataPermissionComponent,
      },
    ],
  },
];
