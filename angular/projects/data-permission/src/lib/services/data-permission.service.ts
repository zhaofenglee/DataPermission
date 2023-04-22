import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class DataPermissionService {
  apiName = 'DataPermission';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/DataPermission/sample' },
      { apiName: this.apiName }
    );
  }
}
